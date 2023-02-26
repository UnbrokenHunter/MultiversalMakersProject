using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace MultiversalMakers
{
    public class SetVolume : MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer;

        public void SetMainVolume(float volume)
        {
            audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20 + 4);
        }

        public void SetMusicVolume(float volume)
        {
            audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20 + 4);
        }

        public void SetSFXVolume(float volume)
        {
            audioMixer.SetFloat("Sound Effects", Mathf.Log10(volume) * 20 + 4);
        }
    }
}
