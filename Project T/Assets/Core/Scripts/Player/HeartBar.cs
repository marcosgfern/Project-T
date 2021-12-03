using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Class HeartBar is used as a component for the UI element that represents player health. 
 * Updates health and maxHealth visually.
 */
public class HeartBar : MonoBehaviour {
    public float heartSize = 64f;
    public string currentHealthUIName, maxHealthUIName;

    private RectTransform currentHealthUI, maxHealthUI;

    public void Awake() {
        this.currentHealthUI = this.transform.Find(currentHealthUIName).GetComponent<RectTransform>();
        this.maxHealthUI = this.transform.Find(maxHealthUIName).GetComponent<RectTransform>();
    }

    /* Updates red hearts */
    public void SetHealth(float health) {
        this.currentHealthUI.sizeDelta = new Vector2(health * heartSize / 2, heartSize);
    }

    /* Updates heart containers */
    public void SetMaxHealth(float maxHealth) {
        this.maxHealthUI.sizeDelta = new Vector2(maxHealth * heartSize / 2, heartSize);
    }
}
