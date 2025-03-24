using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class CleanCityFromEventsController : MonoBehaviour
{
    [Header("Shake and Tilt Detection")]
    //Tilt and shake thresholds
    [SerializeField] private float shakeThreshold;
    [SerializeField] private float tiltThreshold;
    //Required check times
    [SerializeField] private float requiredShakeTime;
    [SerializeField] private float requiredTiltTime;
    //Check rates
    [SerializeField] private float velocitySampleRate;
    [SerializeField] private float tiltCheckRate;

    [Header("References")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject waterBlock;
    [SerializeField] private GameObject snowBlock;

    [Header("State Variables")]
    [SerializeField] private Vector3 previousPosition;
    [SerializeField] private float currentTiltTime;
    [SerializeField] private float currentShakeTime;
    [SerializeField] private float velocityCheckTimer;
    
    [SerializeField] private bool isBeingHeld;
    [SerializeField] private bool isCleaningComplete;

    // VR Input references
    [SerializeField] private Rigidbody rb;
    [SerializeField] private XRGrabInteractable grabInteractable;
    
    [Header("Interaction Text")]
    [SerializeField] private GameObject rainText;
    [SerializeField] private GameObject snowText;

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
        
        if (gameManager.isRaining)
        {
            tiltCheckRate += Time.deltaTime;
            if (tiltCheckRate >= velocitySampleRate)
            {
                rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
                CheckTilt();
            }
        }else if (gameManager.isSnowing)
        {
            velocityCheckTimer += Time.deltaTime;
            
            // Check velocity at regular intervals
            if (velocityCheckTimer >= velocitySampleRate)
            {
                velocityCheckTimer = 0f;
                CheckShakeVelocity();
            }
        }
    }

    private void CheckTilt()
    {
        isCleaningComplete = false;
        
        // Get the Z rotation (roll/side tilt)
        float rotation = transform.eulerAngles.z;
    
        // Convert angle to 0-180 range instead of 0-360
        float sideTilt = rotation > 180 ? 360 - rotation : rotation;
    
        if (sideTilt >= tiltThreshold)
        {
            currentTiltTime += tiltCheckRate;
        
            // Visual feedback that tilting is working -- possible progress bar
            if (currentTiltTime % 0.3f <= tiltCheckRate)
            {
                float progressPercentage = Mathf.Min((currentTiltTime / requiredTiltTime) * 100f, 100f);
            }

            // Check if the player has tilted enough
            if (currentTiltTime >= requiredTiltTime)
            {
                CleanBlocks();
                isCleaningComplete = true;
                rb.constraints = RigidbodyConstraints.None;
            }
        }
        else
        {
            // Reset progress if not tilting continuously
            currentTiltTime = Mathf.Max(0, currentTiltTime - (velocitySampleRate * 0.5f));
        }
    }
    
    private void CheckShakeVelocity()
    {
        isCleaningComplete = false;
        float velocity = rb.linearVelocity.magnitude;

        if (velocity >= shakeThreshold)
        {
            currentShakeTime += velocitySampleRate;
        
            // Visual feedback that shaking is working -- Possible progress bar
            if (currentShakeTime % 0.3f <= velocitySampleRate)
            {
                float progressPercentage = Mathf.Min((currentShakeTime / requiredShakeTime) * 100f, 100f);
            }

            // Check if the player has shaken enough
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
        if (gameManager.isRaining && waterBlock && waterBlock.activeSelf)
        {
            waterBlock.SetActive(false);
            gameManager.isRaining = false;
            rainText.gameObject.SetActive(false);
            gameManager.manuallyCleared = true;
            gameManager.canBuild = true;
        }

        if (gameManager.isSnowing && snowBlock && snowBlock.activeSelf)
        {
            snowBlock.SetActive(false);
            gameManager.isSnowing = false;
            snowText.gameObject.SetActive(false);
            gameManager.manuallyCleared = true;
            gameManager.canBuild = true;
        }

        if (gameManager.manuallyCleared)
        {
            currentShakeTime = 0f;
        }
    }

    private void OnGrabStart(SelectEnterEventArgs args)
    {
        isBeingHeld = true;
        isCleaningComplete = false;
        currentShakeTime = 0f;
        currentTiltTime = 0f;
        velocityCheckTimer = 0f;
    }

    private void OnGrabEnd(SelectExitEventArgs args)
    {
        isBeingHeld = false;
        isCleaningComplete = false;
        currentShakeTime = 0f;
        currentTiltTime = 0f;
    }
}
