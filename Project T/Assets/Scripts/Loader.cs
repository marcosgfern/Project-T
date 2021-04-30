using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour {

    public Scene scene;
    public void Load() {
        SceneManager.LoadScene(this.scene.ToString());
    }

    public enum Scene {
        GameScene,
        MainMenu,
        DeathScreen
    }
}
