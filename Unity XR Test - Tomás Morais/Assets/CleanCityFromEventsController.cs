using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class CleanCityFromEventsController : MonoBehaviour
{
[Header("Shake Detection")]
    [SerializeField] private float shakeThreshold;        // Minimum velocity to count as shake
    [SerializeField] private float requiredShakeTime;   // Time needed to shake to clean
    [SerializeField] private float velocitySampleRate;  // How often to check velocity

    [Header("References")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject waterBlock;
    [SerializeField] private GameObject snowBlock;

    [SerializeField] private Vector3 previousPosition;
    [SerializeField] private float currentShakeTime;
    [SerializeField] private float velocityCheckTimer;
    
    [SerializeField] private bool isBeingHeld;
    [SerializeField] private bool isCleaningComplete;

    // VR Input references
    [SerializeField] private Rigidbody rb;
    [SerializeField] private XRGrabInteractable grabInteractable;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();
        isCleaningComplete = false;

        // Subscribe to grab events
        grabInteractable.selectEntered.AddListener(OnGrabStart);
        grabInteractable.selectExited.AddListener(OnGrabEnd);
    }

    private void OnDestroy()
    {
        // Cleanup subscribers
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrabStart);
            grabInteractable.selectExited.RemoveListener(OnGrabEnd);
        }
    }

    private void Update()
    {
       
        if (!isBeingHeld || isCleaningComplete) return;

        velocityCheckTimer += Time.deltaTime;

        // Check velocity at regular intervals
        if (velocityCheckTimer >= velocitySampleRate)
        {
            velocityCheckTimer = 0f;
            CheckShakeVelocity();
        }
    }

    private void CheckShakeVelocity()
    {
        float velocity = rb.linearVelocity.magnitude;

        if (velocity >= shakeThreshold)
        {
            currentShakeTime += velocitySampleRate;
        
            // Visual feedback that shaking is working
            if (currentShakeTime % 0.3f <= velocitySampleRate)
            {
                float progressPercentage = Mathf.Min((currentShakeTime / requiredShakeTime) * 100f, 100f);
                Debug.Log($"Shaking... Progress: {progressPercentage:F0}%");
            }

            // Check if we've shaken enough
            if (currentShakeTime >= requiredShakeTime)
            {
                CleanBlocks();
                isCleaningComplete = true;
            }
        }
        else
        {
            // Reset progress if not shaking continuously
            currentShakeTime = Mathf.Max(0, currentShakeTime - (velocitySampleRate * 0.5f));
        }
    }
    
    public void CleanBlocks()
    {
        bool cleaned = false;

        // Add debug logs to check conditions
        Debug.Log($"Attempting to clean blocks - IsRaining: {gameManager.isRaining}, WaterBlock active: {waterBlock.activeSelf}");
        Debug.Log($"IsSnowing: {gameManager.isSnowing}, SnowBlock active: {snowBlock.activeSelf}");

        if (gameManager.isRaining && waterBlock && waterBlock.activeSelf)
        {
            Debug.Log("Deactivating water block");
            waterBlock.SetActive(false);
            gameManager.isRaining = false;
            cleaned = true;
        }

        if (gameManager.isSnowing && snowBlock && !cleaned && snowBlock.activeSelf)
        {
            Debug.Log("Deactivating snow block");
            snowBlock.SetActive(false);
            gameManager.isSnowing = false;
            cleaned = true;
        }

        if (cleaned)
        {
            Debug.Log("Blocks cleaned by shaking!");
            currentShakeTime = 0f;
            cleaned = false;
        }
        else
        {
            Debug.Log("No blocks needed cleaning");
        }
    }

    private void OnGrabStart(SelectEnterEventArgs args)
    {
        isBeingHeld = true;
        isCleaningComplete = false;  // Reset when grabbed
        currentShakeTime = 0f;
        velocityCheckTimer = 0f;
    }

    private void OnGrabEnd(SelectExitEventArgs args)
    {
        isBeingHeld = false;
        currentShakeTime = 0f;
    }
}
