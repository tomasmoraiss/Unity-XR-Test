using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool canBuild;
    public GameObject waterBlockObject;
    public GameObject snowBlockObject;
    public GameObject buildingToPlace;
    public bool isRaining;
    public bool isSnowing;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isRaining = false;
        isSnowing = false;
        canBuild = false;
    }

    void Update()
    {
    }
}
