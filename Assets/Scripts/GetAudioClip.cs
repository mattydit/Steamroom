using Photon.Voice.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAudioClip : MonoBehaviour
{
    public AudioSource audioSrc;
    //public GameObject audioPeer;
    public Recorder recorder;
    //public bool streamAudio;

    // Start is called before the first frame update
    void Start()
    {
        //audioSrc = audioPeer.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (audioSrc != null && audioSrc.isPlaying == true)
        {
            recorder.SourceType = Recorder.InputSourceType.AudioClip;
            recorder.AudioClip = audioSrc.clip;
        }


    }
}
