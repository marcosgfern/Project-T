using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBar : MonoBehaviour {
    public RectTransform currentHealthUI, maxHealthUI;
    public float heartSize = 64f;

    public void SetHealth(float health) {
        this.currentHealthUI.sizeDelta = new Vector2(health * heartSize, heartSize);
    }

    public void SetMaxHealth(float maxHealth) {
        this.maxHealthUI.sizeDelta = new Vector2(maxHealth * heartSize, heartSize);
    }
}
