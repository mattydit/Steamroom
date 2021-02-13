/*
Class used for analysing audio and selecting audio clip from path.
*/
using GracesGames.SimpleFileBrowser.Scripts;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;


[RequireComponent (typeof (AudioSource))]
public class AudioPeer : MonoBehaviour
{
    public AudioSource audioSrc;
    public bool audioSwitchedOn;
    int index = 0;

    string file; //path to the file.
    public static float[] bandBuffer;
    float[] bufferDecrease;

    float[] freqBandHighest;
    public float[] audioBand;
    public float[] audioBandBuffer;

    public float amplitude, amplitudeBuffer;
    float amplitudeHighest;

    //Mic input
    //public AudioClip audioClip;
    public bool useMicrophone;
    public string selectedDevice;
    public UnityEngine.Audio.AudioMixerGroup mixerGroupMic, mixerGroupMaster;

    //File Browser
    public GameObject fileBrowserPrefab;
    public string[] fileExtensions;

    public bool localAudioPlaying;

    public static int frameSize = 512;
    public static float[] spectrum;
    public static float[] bands;

    public float binWidth;
    public float sampleRate;

    //error checking
    public VisualManager visualManager;
    public GameObject unableToPlay;

    private void Awake()
    {
        int logFrameSize = (int)Mathf.Log(frameSize, 2);
        spectrum = new float[frameSize];
        bands = new float[logFrameSize];
        freqBandHighest = new float[logFrameSize];
        audioBand = new float[logFrameSize];
        audioBandBuffer = new float[logFrameSize];
        bandBuffer = new float[logFrameSize];
        bufferDecrease = new float[logFrameSize];
    }

    // Start is called before the first frame update
    void Start()
    {
        sampleRate = AudioSettings.outputSampleRate;
        binWidth = AudioSettings.outputSampleRate / 2 / frameSize;
        audioSrc = GetComponent<AudioSource>();

        //Microphone input
        if(useMicrophone == true)
        {
            if(Microphone.devices.Length > 0)
            {
                selectedDevice = Microphone.devices[0].ToString();
                audioSrc.outputAudioMixerGroup = mixerGroupMic;
                audioSrc.clip = Microphone.Start(selectedDevice, true, 2400, AudioSettings.outputSampleRate);

            }
            else
            {
                useMicrophone = false;
            }
        }
        else
        {
            audioSrc.outputAudioMixerGroup = mixerGroupMaster;
            //audioSrc.clip = audioClip;
        }

        

        //audioSrc.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (audioSwitchedOn == true && audioSrc.isPlaying == false)
        {
            //audioSrc.clip = audioClip;
            audioSrc.Play();
            //localAudioPlaying = true;
        }
        else if (audioSwitchedOn == false && audioSrc.isPlaying == true)
        {
            audioSrc.Stop();
        }

        GetSpectrumAudioSource();
        GetFrequencyBands();
        BandBuffer();
        CreateAudioBands();
        GetAmplitude();
    }

    void GetSpectrumAudioSource()
    {
        audioSrc.GetSpectrumData(spectrum, 0, FFTWindow.Blackman);
    }


    void GetFrequencyBands()
    {
        for (int i = 0; i < bands.Length; i++)
        {
            int start = (int)Mathf.Pow(2, i) - 1;
            int width = (int)Mathf.Pow(2, i);
            int end = start + width;
            float average = 0;

            for (int j = start; j < end; j++)
            {
                average += spectrum[j] * (j + 1);
            }

            average /= (float)width;
            bands[i] = average;
        }

    }

    void BandBuffer()
    {
        for(int g = 0; g < bands.Length; g++)
        {
            if(bands[g] > bandBuffer[g])
            {
                bandBuffer[g] = bands[g];
                bufferDecrease[g] = 0.005f;
            }
            if (bands[g] < bandBuffer[g])
            {
                bandBuffer[g] -= bufferDecrease[g];
                bufferDecrease[g] *= 1.2f;
            }
        }
    }

    void CreateAudioBands()
    {
        for (int i = 0; i < bands.Length; i++)
        {
            if (bands[i] > freqBandHighest[i])
            {
                freqBandHighest[i] = bands[i];

            }
            audioBand[i] = (bands[i] / freqBandHighest[i]);
            audioBandBuffer[i] = (bandBuffer[i] / freqBandHighest[i]);
        }
    }

    void GetAmplitude()
    {
        float currentAmplitude = 0;
        float currentAmplitudeBuffer = 0;

        for (int i = 0; i < bands.Length; i++)
        {
            currentAmplitude += audioBand[i];
            currentAmplitudeBuffer += audioBandBuffer[i];
        }

        if (currentAmplitude > amplitudeHighest)
        {
            amplitudeHighest = currentAmplitude;
        }

        amplitude = currentAmplitude / amplitudeHighest;
        amplitudeBuffer = currentAmplitudeBuffer / amplitudeHighest;
    }

    IEnumerator GetAudioClip()
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(file, AudioType.WAV))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                audioSrc.clip = DownloadHandlerAudioClip.GetContent(www);
                audioSrc.clip.name = GetClipName(file);
                
                StartCoroutine(WaitAndStop(audioSrc.clip));
            }
        }

    }

    public void OpenFileBrowser()
    {
        GameObject fileBrowserObject = Instantiate(fileBrowserPrefab, transform);
        fileBrowserObject.name = "FileBrowser";

        FileBrowser fileBrowserScript = fileBrowserObject.GetComponent<FileBrowser>();

        fileBrowserScript.SetupFileBrowser(ViewMode.Landscape);

        fileBrowserScript.OpenFilePanel(fileExtensions);

        fileBrowserScript.OnFileSelect += LoadFileUsingPath;
    }

    private void LoadFileUsingPath(string path)
    {
        Debug.Log(path);
        if (audioSrc.isPlaying)
        {
            unableToPlay.SetActive(true);
            StartCoroutine(DisableAfterSeconds(5, unableToPlay));
        }
        else
        {
            if (path.Length != 0)
            {
                file = path;
            }

            StartCoroutine(GetAudioClip());

            audioSwitchedOn = true;
            localAudioPlaying = true;
        }
    }

    IEnumerator WaitAndStop(AudioClip audioClip)
    {
        yield return new WaitForSeconds(audioClip.length);
        audioSwitchedOn = false;
        audioSrc.Stop();
        audioSrc.clip = null;
        //audioClip = null;
    }

    private void CheckifPlaying()
    {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("speaker"))
        {
            if (go.GetComponent<AudioSource>().isPlaying)
            {
                
            }
        }
    }

    public string GetClipName(string clipPath)
    {
        string[] parts = clipPath.Split('\\');
        string clipName = "";
        
        for (int i = 0; i < parts.Length; i++)
        {
            if (parts[i].Contains(".wav") || parts[i].Contains(".ogg"))
            {
                string[] parts2 = parts[i].Split('.');
                clipName = parts2[0];
            }
        }

        return clipName;
    }

    IEnumerator DisableAfterSeconds(int seconds, GameObject go)
    {
        yield return new WaitForSeconds(seconds);
        go.SetActive(false);
    }
}
