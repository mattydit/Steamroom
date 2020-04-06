using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Voice.Unity;

public class StreamAudio : MonoBehaviour
{
    public Recorder recorder;
    public bool isStreaming;

    // Start is called before the first frame update
    void Start()
    {
        recorder = GetComponent<Recorder>();

    }

    // Update is called once per frame
    void Update()
    {
        if (recorder.IsRecording && recorder.AudioClip != null)
        {
            recorder.RestartRecording();
            recorder.TransmitEnabled = true;
            isStreaming = true;
        }
    }
}
