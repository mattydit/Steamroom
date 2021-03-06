﻿using Photon.Voice.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualManager : MonoBehaviour
{
    public AudioPeer audioPeer;
    public GameObject flowField;
    public GameObject flowField2;
    public GameObject speaker;
    public GameObject CubeVisual;
    public bool usingSpeaker;
    public int waitSeconds;
    public bool visualSwapping;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FindSpeakers();

        if (audioPeer.audioSwitchedOn == true)
        {
            usingSpeaker = false;

            if (visualSwapping == false)
            {
                StartCoroutine(VisualSwap());
                visualSwapping = true;
            } 

        }
        else if (audioPeer.audioSwitchedOn == false && speaker != null)
        {
            if (speaker.GetComponent<AudioSource>().isPlaying)
            {
                usingSpeaker = true;

                if (visualSwapping == false)
                {
                    StartCoroutine(VisualSwap());
                    visualSwapping = true;
                }
                
            }
        }
        else
        {
            flowField.SetActive(false);
            CubeVisual.SetActive(false);
        }

    }

    private void FindSpeakers()
    {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("speaker"))
        {
            if (go.GetComponent<AudioSource>().isPlaying)
            {
                speaker = go;
            }
        }
    }

    IEnumerator VisualSwap()
    {
        flowField.SetActive(true);
        yield return new WaitForSeconds(waitSeconds);
        flowField.SetActive(false);
        CubeVisual.SetActive(true);
        yield return new WaitForSeconds(waitSeconds);
        CubeVisual.SetActive(false);
        flowField2.SetActive(true);
        yield return new WaitForSeconds(waitSeconds);
        flowField2.SetActive(false);
        CubeVisual.SetActive(true);
        yield return new WaitForSeconds(waitSeconds);
        CubeVisual.SetActive(false);
        visualSwapping = false;
    }

}
