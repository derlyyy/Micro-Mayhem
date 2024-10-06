using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
    public AudioMixer audioMixer;

    public void SetMasterVolume(float value)
    {
        audioMixer.SetFloat("MasterPrivateVolume", value);
    }
    
    public void SetEnvironmentVolume(float value)
    {
        audioMixer.SetFloat("MusicVolume", value);
    }
}
