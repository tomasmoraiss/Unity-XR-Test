using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;


public class BuildController : MonoBehaviour
{
    [SerializeField] private Transform origin;
    [SerializeField] private InputActionReference buildAction;
    [SerializeField] private GameManager gameManager;
    private GameObject building;
    private List<GameObject> buildings = new List<GameObject>();
    
    // Update is called once per frame

    void Update()
    {
        if (gameManager.canBuild)
        {
            buildAction.action.Enable();
            buildAction.action.performed += Build;
        }
    }
    
    private void Build(InputAction.CallbackContext ctx)
    {
        RaycastHit hit;
        if (Physics.Raycast(origin.position, origin.forward, out hit))
        {
            if (hit.transform.name == "City")
            {
                building = Instantiate(gameManager.buildingToPlace, hit.point, Quaternion.identity);
                buildings.Add(building);
                building.transform.SetParent(hit.transform, true);
                gameManager.canBuild = false;
                buildAction.action.Disable();
                buildAction.action.performed -= Build;
            }
        }
    }
}
