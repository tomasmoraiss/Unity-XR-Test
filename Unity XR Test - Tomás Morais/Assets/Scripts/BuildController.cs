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
    [SerializeField] private Grid grid;
    [SerializeField] private GameObject gridVisualizer;
    
    private GameObject building;
    
    // Update is called once per frame

    void Update()
    {
        if (gameManager.canBuild)
        {
            buildAction.action.Enable();
            buildAction.action.performed += Build;
            gridVisualizer.SetActive(true);
        }
    }
    
    private void Build(InputAction.CallbackContext ctx)
    {
        RaycastHit hit;
        if (Physics.Raycast(origin.position, origin.forward, out hit))
        {
            if (hit.transform.name == "City")
            {
                Vector3Int gridPosition = grid.WorldToCell(hit.point);
                building = Instantiate(gameManager.buildingToPlace, hit.point, Quaternion.identity);
                building.transform.SetParent(hit.transform, true);
                building.transform.position = grid.CellToWorld(gridPosition);
                grid.gameObject.SetActive(false);
                gameManager.canBuild = false;
                gameManager.inPreview = false;
                buildAction.action.Disable();
                buildAction.action.performed -= Build;
            }
        }
    }
}
