using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;


public class BuildController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform origin;
    [SerializeField] private InputActionReference buildAction;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Canvas buildButton;
    
    private GameObject building;
    
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
                building.transform.SetParent(hit.transform, true);
                
                gameManager.canBuild = false;
                gameManager.inPreview = false;
                
                buildButton.gameObject.SetActive(true);
                
                buildAction.action.Disable();
                buildAction.action.performed -= Build;
            }
        }
    }
}
