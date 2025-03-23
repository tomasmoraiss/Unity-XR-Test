using UnityEngine;

public class PreviewBuildingController : MonoBehaviour
{
    [SerializeField] private Material previewMaterialPrefab;
    [SerializeField] private Material previewMaterialInstance;
    
    [SerializeField] private GameManager gameManager;
    private GameObject buildingPreview;
    
    [SerializeField] private Transform origin;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        previewMaterialInstance = new Material(previewMaterialPrefab);
        buildingPreview = Instantiate(gameManager.buildingToPlace, gameManager.buildingToPlace.transform.position, Quaternion.identity);
        buildingPreview.GetComponent<MeshRenderer>().material = previewMaterialInstance;
        //CreatePreview();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.inPreview)
            UpdatePositionOfPreview();
        else
        {
            Destroy(buildingPreview);
        }
    }

    void UpdatePositionOfPreview()
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
