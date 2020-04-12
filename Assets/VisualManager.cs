using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualManager : MonoBehaviour
{
    public GameObject audioPeer;
    public GameObject flowField;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (audioPeer.GetComponent<AudioSource>().isPlaying)
        {
            flowField.SetActive(true);
        }
        else
        {
            flowField.SetActive(false);
        }
    }
}
