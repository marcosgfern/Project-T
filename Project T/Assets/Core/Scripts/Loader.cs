using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.SceneManagement;

/* Class Loader is used as a component for the LevelLoader.
 * Triggers crossfade animation.
 * Loads scenes.
 */
public class Loader : MonoBehaviour {

    public Animator crossfadeAnimator;

    /* Triggers load of the game's main scene */
    public void LoadGame() {
        if (IsTutorialFinished())
        {
            StartCoroutine(Load(Scene.GameScene));
        }
        else
        {
            StartCoroutine(Load(Scene.Tutorial));
        }
    }

    /* Triggers load of main menu scene */
    public void LoadMainMenu() {
        StartCoroutine(Load(Scene.MainMenu));
    }

    public void LoadTutorial()
    {
        StartCoroutine(Load(Scene.Tutorial));
    }

    public void FinishTutorial()
    {
        PlayerPrefs.SetString("TutorialFinished", true.ToString());
        LoadGame();
    }

    /* Starts crossfade animation and loads @scene afterwards. */
    private IEnumerator Load(Scene scene) {
        Time.timeScale = 1f;
        crossfadeAnimator.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(scene.ToString());
    }

    private bool IsTutorialFinished()
    {
        return bool.Parse(PlayerPrefs.GetString("TutorialFinished", "False"));
    }

    /* Closes application */
    public void Quit() {
        Application.Quit();
    }

    public enum Scene {
        GameScene,
        MainMenu,
        Tutorial
    }
}
