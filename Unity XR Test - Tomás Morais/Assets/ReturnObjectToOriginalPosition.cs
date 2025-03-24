using System;
using UnityEngine;
public class ReturnObjectToOriginalPosition : MonoBehaviour
{
    private Vector3 initialObjectPosition;
    private Quaternion initialObjectRotation;
    private Rigidbody rb;

    // Unity function called once when the game starts
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        initialObjectPosition = transform.position;
        initialObjectRotation = transform.rotation;
        rb.angularDamping = 100;
        rb.linearDamping = 100;

    }
    

    public void OnRelease()
    {
        //Add a lot of damping so that the object isn't affected by gravity and drifts away
        rb.angularDamping = 100;
        rb.linearDamping = 100;
        rb.linearVelocity = new Vector3(0f, 0f, 0f);
        transform.SetPositionAndRotation(initialObjectPosition, initialObjectRotation);
    }
}
