using UnityEngine;
using UnityEngine.Audio;

public class VolumeSettings : MonoBehaviour
{
   [SerializeField] private AudioMixer audioMixer;


    // The volume is set in decibels, so we convert the linear volume (0.0 to 1.0) to decibels using the formula: dB = 20 * log10(linearVolume). 
    // This way, when the slider is at 0.5, the volume will be -6 dB, which is half the perceived loudness.
    // The minimum volume cannot be set to 0 dB directly, as that would mute the audio. Instead, we set it to -80 dB, which is effectively silent for most practical purposes.
   public void SetMusicVolume(float volume)
   {
      audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20f);
   }

      public void SetSFXVolume(float volume)
   {
      audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20f);
   }
}
