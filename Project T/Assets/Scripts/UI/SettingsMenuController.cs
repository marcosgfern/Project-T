using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenuController : MonoBehaviour {

    public GameObject settingsMenu;

    public void Open() {
        this.settingsMenu.SetActive(true);
    }

    public void Close() {
        this.settingsMenu.SetActive(false);
    }
    
}
