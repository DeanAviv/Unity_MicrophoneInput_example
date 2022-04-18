using UnityEngine;

/// <summary>Gets the default microphone of the device, saves a recording and plays it</summary>
public class MicInputHandler : MonoBehaviour
{
    [Header("AudioSource")]
    [SerializeField] public AudioSource audioSource;

    [Header("AudioClips")]
    [SerializeField] AudioClip recordedClip;
    [SerializeField] AudioClip currentlyRecordingClip;
    [SerializeField] AudioClip emptyClip;

    [Header("Recording length control")]
    [SerializeField] private int recordingTimeLimit;

    private bool recordingStart = false;
    private bool recordingEnd = false;

    /// <summary>To get access to device microphone we need to identify it through name - so we use a string</summary>
    [SerializeField] string deviceMic;

    #region Set up: 
    void Start()
    {
        SetDeviceMic();
        SetAudioSources();
    }
    private void SetDeviceMic()
    {
        deviceMic = Microphone.devices[0];
    }

    private void SetAudioSources()
    {
        //Get the audiosource connected to this game object
        audioSource = GetComponent<AudioSource>();

        //set the audio source clip to be the recorded mic input
        audioSource.clip = currentlyRecordingClip;

        //set the parameters of the Audio Source 
        audioSource.loop = false;
        audioSource.mute = false;
    }

    #endregion

    #region Button methodes - called through the GUI on screen:
    public void StartRecording()
    {
        //If the audio source is playing - we stop it to record a new recording
        audioSource.Stop();

        //We end the last recording if still running 
        Microphone.End(deviceMic);

        //And set the current audio clip to be the microphone recording clip
        audioSource.clip = currentlyRecordingClip;

        //Sets the currently recorded clip to start recording from the device mic, with the recording time limit
        currentlyRecordingClip = Microphone.Start(deviceMic, true, recordingTimeLimit, 44100);

        recordingStart = true;
        recordingEnd = false;
    }

    public void StopRecording()
    {
        //Save the recorded clip for the next recording
        recordedClip = currentlyRecordingClip;

        //End the recording 
        Microphone.End(deviceMic);
    }

    public void PlayRecording()
    {
        //Set the recorded audio clip to the audio source and play it
        audioSource.clip = recordedClip;
        audioSource.Play();
    }

    /// <summary>Stops playing audio and clears recording</summary>
    public void StopAllAudio()
    {
        audioSource.Stop();
        Microphone.End(deviceMic);
        audioSource.clip = emptyClip;
    }

    #endregion
}
