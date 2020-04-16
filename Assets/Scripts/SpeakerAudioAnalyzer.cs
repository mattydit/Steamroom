using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeakerAudioAnalyzer : MonoBehaviour
{
    public AudioSource audioSrc;

    public static int frameSize = 512;
    public static float[] spectrum;
    public static float[] bands;
    public static float[] bandBuffer;
    float[] bufferDecrease;

    public float binWidth;
    public float sampleRate;

    float[] freqBandHighest;
    public float[] audioBand;
    public float[] audioBandBuffer;

    public float amplitude, amplitudeBuffer;
    float amplitudeHighest;

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
    }

    // Update is called once per frame
    void Update()
    {
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
        for (int g = 0; g < 8; g++)
        {
            if (bands[g] > bandBuffer[g])
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
        for (int i = 0; i < 8; i++)
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

        for (int i = 0; i < 8; i++)
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

}
