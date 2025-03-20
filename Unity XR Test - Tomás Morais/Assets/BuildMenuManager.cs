using UnityEngine;

public class BuildMenuManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform head;
    public float spawnDistance;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = head.transform.position + head.transform.forward * spawnDistance;
        transform.LookAt(new Vector3(head.position.x, transform.position.y, head.position.z));
        transform.forward *= -1f;
    }
}
