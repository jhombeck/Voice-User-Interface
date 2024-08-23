using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class MicrophoneBehavior : MonoBehaviour
{
    AudioSource _audioSource;
    int lastPosition, currentPosition;
    static bool _isMuted = false;

    MicrophoneStreamingBehavior websocket;

    private void Awake()
    {
        websocket = GameObject.Find("WebsocketObject").GetComponent<MicrophoneStreamingBehavior>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        Debug.Log("Sample Rate: " + AudioSettings.outputSampleRate);

        // Select correct microphone
        foreach (var x in Microphone.devices)
        {
            Debug.Log("Mics:" + x.ToString());
        }

        //select correct sampling rate
        int sampling_rate = 16000;

        if (Microphone.devices.Length > 0)
        {

            //_audioSource.clip = Microphone.Start("Microphone (Realtek(R) Audio) ", true, 10, sampling_rate); //AudioSettings.outputSampleRate);
            _audioSource.clip = Microphone.Start(Microphone.devices[0], true, 10, sampling_rate); //AudioSettings.outputSampleRate);
                                                                                                  //Debug.Log("To Sting Audio Source : " + Microphone.devices[0]);

            //          _audioSource.clip = Microphone.Start("Digitale Audioschnittstelle (Valve VR Radio & HMD Mic)", true, 10, sampling_rate); //AudioSettings.outputSampleRate);
        }
        else
        {
            Debug.Log("This will crash!");
        }
        Debug.Log("Selected Audio Source : " + _audioSource.name);
        Debug.Log("Active Audio Source : " + _audioSource.isActiveAndEnabled);
        Debug.Log("To Sting Audio Source : " + Microphone.devices[0]);

        _audioSource.Play();


    }

    public static void MuteMic(bool mute)
    {
        if (mute)
        {
            _isMuted = true;
            Debug.Log("Mic Muted");
        }
        if (!mute)
        {
            _isMuted = false;
            Debug.Log("Mic Listening");
        }

    }

    public static bool IsMuted()
    {
        return _isMuted;
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(_audioSource.volume);
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    _isMuted = !_isMuted;
        //    Debug.Log("Changed to" + _isMuted);
        //}


            if ((currentPosition = Microphone.GetPosition(null)) > 0)
            {
                if (lastPosition > currentPosition)
                    lastPosition = 0;

                if (currentPosition - lastPosition > 0)
                {
                    //Debug.Log("Voice found");
                    float[] samples = new float[(currentPosition - lastPosition) * _audioSource.clip.channels];
                    _audioSource.clip.GetData(samples, lastPosition);

                    short[] samplesAsShorts = new short[(currentPosition - lastPosition) * _audioSource.clip.channels];
                    for (int i = 0; i < samples.Length; i++)
                    {
                        if(!_isMuted)
                        {
                            samplesAsShorts[i] = f32_to_i16(samples[i]);
                        }
                        else
                        {
                        samplesAsShorts[i] = (short)0;
                        }
                    }

                    var samplesAsBytes = new byte[samplesAsShorts.Length * 2];
                    System.Buffer.BlockCopy(samplesAsShorts, 0, samplesAsBytes, 0, samplesAsBytes.Length);
                    websocket.ProcessAudio(samplesAsBytes);

                    if (!GetComponent<AudioSource>().isPlaying)
                        GetComponent<AudioSource>().Play();
                    lastPosition = currentPosition;
                }
            }
        
    }

    short f32_to_i16(float sample)
    {
        sample = sample * 32768;
        if (sample > 32767)
        {
            return 32767;
        }
        if (sample < -32768)
        {
            return -32768;
        }
        return (short)sample;
    }

}
