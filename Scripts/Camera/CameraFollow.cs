using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float speed;

    public PhotonConnector photonConnector;
    // Start is called before the first frame update
    private void Awake()
    {
        //target = photonConnector.localPlayer.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = Vector3.Lerp(transform.position, photonConnector.localPlayer.transform.position, Time.deltaTime * speed);
        if(target != null)
        {
            transform.position = Vector3.Lerp(transform.position, target.transform.position, Time.deltaTime * speed);
        }    
    }
}
