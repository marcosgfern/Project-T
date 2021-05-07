using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour {

    public Animator crossfadeAnimator;

    public void LoadGame() {
        StartCoroutine(Load(Scene.GameScene));
    }

    public void LoadMainMenu() {
        StartCoroutine(Load(Scene.MainMenu));
    }

    private IEnumerator Load(Scene scene) {
        Time.timeScale = 1f;
        crossfadeAnimator.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(scene.ToString());
    }

    public void Quit() {
        Application.Quit();
    }

    public enum Scene {
        GameScene,
        MainMenu
    }
}
