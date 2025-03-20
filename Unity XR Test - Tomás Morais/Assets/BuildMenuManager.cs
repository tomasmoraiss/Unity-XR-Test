using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
public class BuildMenuManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] Transform head;
    [SerializeField] RectTransform buildMenuPanel;
    [SerializeField] float topPositionY;
    [SerializeField] float bottomPositionY;
    [SerializeField] float animationDuration;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(new Vector3(head.position.x, transform.position.y, head.position.z));
        transform.forward *= -1f;
    }

    public async void CloseMenu()
    {
        animationDuration *= 2;
        await CloseMenuAnimation();
        gameObject.SetActive(false);
        animationDuration /= 2;
    }

    public void OpenMenuAnimation()
    {
        buildMenuPanel.DOAnchorPosY(topPositionY, animationDuration);
    }

    async Task CloseMenuAnimation()
    {
        await buildMenuPanel.DOAnchorPosY(bottomPositionY, animationDuration).AsyncWaitForCompletion();
    }
}
