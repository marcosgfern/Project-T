using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour {

    public Scene scene;
    public void Load() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(this.scene.ToString());
    }

    public void Quit() {
        Application.Quit();
    }

    public enum Scene {
        GameScene,
        MainMenu,
        DeathScreen
    }
}
