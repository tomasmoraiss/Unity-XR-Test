using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool canBuild;
    public bool inPreview;
    public GameObject buildingToPlace;
    public bool isRaining;
    public bool isSnowing;
    public bool manuallyCleared;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isRaining = false;
        isSnowing = false;
        canBuild = false;
        inPreview = false;
        manuallyCleared = false;
        buildingToPlace = null;
    }
}
