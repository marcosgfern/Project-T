using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioConfigurator : MonoBehaviour
{
    void Start()
    {
        AudioListener.volume = PlayerPrefs.GetFloat(VolumeController.PlayerPrefsKeyVolume, 1.0f);
        Debug.Log(PlayerPrefs.GetFloat(VolumeController.PlayerPrefsKeyVolume, 1.0f));
    }
}
