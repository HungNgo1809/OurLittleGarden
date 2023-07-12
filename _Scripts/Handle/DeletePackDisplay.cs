using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeletePackDisplay : MonoBehaviour
{
    public Transform parrent;
    public void DeletePack()
    {
        foreach (Transform child in parrent)
        {
            Destroy(child.gameObject);
        }    
    }    
}
