using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool canBuild;
    public GameObject buildingToPlace;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canBuild = false;
    }
}
