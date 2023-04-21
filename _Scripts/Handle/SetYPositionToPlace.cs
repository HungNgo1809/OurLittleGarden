using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetYPositionToPlace : MonoBehaviour
{
    public int objAddHeight;

    public float upVector = 0.3f;
    // Start is called before the first frame update
    public void Start()
    {
        Invoke("Handle", 2.0f);
    }
    public void Handle()
    {
        Vector3 rayStart = transform.position + (Vector3.up * upVector);
        //Vector3 rayStart = transform.position;
        // Cast a ray from the center of the cell and see if it hits any colliders
        RaycastHit hit;
        if (Physics.Raycast(rayStart, Vector3.down, out hit) && hit.collider.tag == "Plane")
        {
            transform.position = new Vector3(transform.position.x, hit.point.y + objAddHeight, transform.position.z);
        }
    }
}
