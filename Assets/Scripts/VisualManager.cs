using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualManager : MonoBehaviour
{
    public GameObject audioPeer;
    public GameObject flowField;
    public GameObject speaker;


    // Start is called before the first frame update
    void Start()
    {
        speaker = GameObject.FindGameObjectWithTag("speaker");
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

        if (audioPeer.GetComponent<AudioSource>().isPlaying == false && speaker.GetComponent<AudioSource>().isPlaying == true)
        {
            audioPeer = speaker;
            flowField.SetActive(true);
        }

    }
}
