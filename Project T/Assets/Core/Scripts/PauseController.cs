using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class PauseController is used as a component for Canvas.
 * Resumes and pauses main game.
 * Shows and hides pause menu and pause button.
 */
public class PauseController : MonoBehaviour {

    public GameObject pauseMenu;
    public GameObject pauseButton;

    public AudioClip pauseSoundEffect, resumeSoundEffect;
    private AudioSource effectSource;

    private void Awake() {
        this.effectSource = GetComponent<AudioSource>();
    }

    public void Resume() {
        pauseMenu.SetActive(false);
        pauseButton.SetActive(true);
        this.effectSource.PlayOneShot(this.resumeSoundEffect);
        Time.timeScale = 1f;
    }

    public void Pause() {
        Time.timeScale = 0f;
        pauseButton.SetActive(false);
        pauseMenu.SetActive(true);
        this.effectSource.PlayOneShot(this.pauseSoundEffect);
    }
}
