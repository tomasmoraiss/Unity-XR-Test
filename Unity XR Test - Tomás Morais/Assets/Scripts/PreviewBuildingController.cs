using UnityEngine;

public class PreviewBuildingController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Material previewMaterialPrefab;
    [SerializeField] private Material previewMaterialInstance;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Transform origin;
    
    private GameObject buildingPreview;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if(gameManager.inPreview)
            UpdatePositionOfPreview();
        else
        {
            Destroy(buildingPreview);
        }
    }

    public void CreatePreview()
    {
        previewMaterialInstance = new Material(previewMaterialPrefab);
        //Create a copy of the instance and change its material
        buildingPreview = Instantiate(gameManager.buildingToPlace, gameManager.buildingToPlace.transform.position, Quaternion.identity);
        buildingPreview.GetComponent<MeshRenderer>().material = previewMaterialInstance;
    }

    //Make the preview follow the player's controller
    private void UpdatePositionOfPreview()
    {
        RaycastHit hit;
        if (Physics.Raycast(origin.position, origin.forward, out hit))
        {
            if (hit.transform.name == "City")
            {
                buildingPreview.transform.position = hit.point;
            }
        }
    }
}
