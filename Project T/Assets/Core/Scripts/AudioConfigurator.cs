using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This script should be attached to a game object 
 * that is enabled at the beginning.
 */
public class AudioConfigurator : MonoBehaviour
{
    void Start()
    {
        AudioListener.volume = PlayerPrefs.GetFloat(VolumeController.PlayerPrefsKeyVolume, 1.0f);
    }
}
