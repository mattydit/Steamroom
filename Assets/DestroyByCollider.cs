using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByCollider : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "TopBoundary") ;
        {
            Destroy(gameObject);
        }

        
    }
}
