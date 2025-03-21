using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
public class BuildMenuManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Transform head;
    [SerializeField] private RectTransform buildMenuPanel;
    [SerializeField] private float topPositionY;
    [SerializeField] private float bottomPositionY;
    [SerializeField] private float animationDuration;
    [SerializeField] private Transform handController;
    [SerializeField] private GameManager gameManager;

    // Update is called once per frame
    private void Update()
    {
        if (buildMenuPanel is null) return;
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

    public void SpawnItemInHand(GameObject prefab)
    {
        gameManager.canBuild = true;
        gameManager.buildingToPlace = prefab;
    }
}
