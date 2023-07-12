using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPhoton : MonoBehaviour
{
    public Transform target;

    public float speed;

    public PhotonPlaying photonPlaying;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        //transform.position = Vector3.Lerp(transform.position, photonConnector.localPlayer.transform.position, Time.deltaTime * speed);
        if (target != null)
        {
            transform.position = Vector3.Lerp(transform.position, target.transform.position, Time.deltaTime * speed);
        }
        else
        {
            if(photonPlaying.localPlayer!= null)
            {
                target = photonPlaying.localPlayer.transform;
            }    
        }    
    }
}
