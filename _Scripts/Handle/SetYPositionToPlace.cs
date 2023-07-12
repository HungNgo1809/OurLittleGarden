using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetYPositionToPlace : MonoBehaviour
{

    public ParticleSystem particle;
    public float minPlayDelay = 0f; 
    public float maxPlayDelay = 300f; 

    private float nextPlayTime = 0f;

    public int objAddHeight;

    public float upVector = 0.3f;
    // Start is called before the first frame update
    public void Start()
    {
        Invoke("Handle", 2.0f);
        CalculateNextPlayTime();
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


    void Update()
    {
        if (Time.time >= nextPlayTime)
        {
 
            PlayParticleSystem();
            CalculateNextPlayTime();
        }
    }

    private void PlayParticleSystem()
    {
        if (particle != null)
        {
            particle.Play();
        }
    }

    private void CalculateNextPlayTime()
    {
        float delay = Random.Range(minPlayDelay, maxPlayDelay);
        nextPlayTime = Time.time + delay;
    }
}
