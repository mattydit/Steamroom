using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByCollider : MonoBehaviour
{
    public AudioCubes audioCubes;

    private Vector3 min;
    private Vector3 max;

    private void Start()
    {
        audioCubes = GetComponentInParent<AudioCubes>();
        min = audioCubes.min;
        max = audioCubes.max;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "TopBoundary") 
        {
            //Debug.Log("Entered collider");
            gameObject.transform.position = new Vector3(Random.Range(min.x, max.x), 1, Random.Range(min.z, max.z));
        }

        
    }
}
