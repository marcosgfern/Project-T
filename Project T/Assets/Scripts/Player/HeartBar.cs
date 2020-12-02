using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBar : MonoBehaviour {
    public float heartSize = 64f;
    public string currentHealthUIName, maxHealthUIName;

    private RectTransform currentHealthUI, maxHealthUI;

    public void Awake() {
        this.currentHealthUI = this.transform.Find(currentHealthUIName).GetComponent<RectTransform>();
        this.maxHealthUI = this.transform.Find(maxHealthUIName).GetComponent<RectTransform>();
    }

    public void SetHealth(float health) {
        this.currentHealthUI.sizeDelta = new Vector2(health * heartSize, heartSize);
    }

    public void SetMaxHealth(float maxHealth) {
        this.maxHealthUI.sizeDelta = new Vector2(maxHealth * heartSize, heartSize);
    }
}
