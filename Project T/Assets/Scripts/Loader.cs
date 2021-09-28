using System.Collections;
using System.Collections.Generic;
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
        StartCoroutine(Load(Scene.GameScene));
    }

    /* Triggers load of main menu scene */
    public void LoadMainMenu() {
        StartCoroutine(Load(Scene.MainMenu));
    }

    /* Starts crossfade animation and loads @scene afterwards. */
    private IEnumerator Load(Scene scene) {
        Time.timeScale = 1f;
        crossfadeAnimator.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(scene.ToString());
    }

    /* Closes application */
    public void Quit() {
        Application.Quit();
    }

    public enum Scene {
        GameScene,
        MainMenu
    }
}
