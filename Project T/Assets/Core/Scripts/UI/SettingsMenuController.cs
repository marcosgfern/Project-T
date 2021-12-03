using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Mini class responsible for opening and closing the settingsMenu. */
public class SettingsMenuController : MonoBehaviour {

    public GameObject settingsMenu;

    private AudioSource audioSource;

    private void Awake() {
        this.audioSource = GetComponent<AudioSource>();
    }

    public void Open() {
        this.settingsMenu.SetActive(true);
    }

    public void Close() {
        this.settingsMenu.SetActive(false);
        this.audioSource.Play();
    }
    
}
