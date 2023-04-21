using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiFollowObject : MonoBehaviour
{
    public Transform ObjFollow;
    public Vector3 offset;

    public Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(Quaternion.LookRotation(transform.position - cam.transform.position).eulerAngles.x, transform.rotation.y, transform.rotation.z);
        if (ObjFollow != null)
        {
            Vector3 pos = ObjFollow.position + offset;
            if(transform.position != pos)
            {
                transform.position = pos;
                //transform.LookAt(cam.transform);
            }
        }
    }
}
