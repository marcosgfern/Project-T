using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using EnemyHealth;

public class SpriteManager {
    public static Color customRed = new Color(0.9411765f, 0.1176471f, 0.1882353f, 1f);
    public static Color customBlue = new Color(0.159399f, 0.4789415f, 0.801f, 1f);

    private SpriteRenderer spriteRenderer;
    private Color spriteColor;

    public SpriteManager(SpriteRenderer spriteRenderer) {
        this.spriteRenderer = spriteRenderer;
        this.spriteColor = Color.white;
    }

    public SpriteManager(SpriteRenderer spriteRenderer, Color color) {
        this.spriteRenderer = spriteRenderer;
        SetMainColor(color);
    }

    public void SetSprite(Sprite sprite) {
        this.spriteRenderer.sprite = sprite;
    }

    public void SetMainColor(Color color) {
        this.spriteColor = color;
    }

    public void SetMainColor(DamageColor damageColor) {
        switch (damageColor) {
            case DamageColor.White:
                SetMainColor(Color.white);
                break;

            case DamageColor.Red:
                SetMainColor(customRed);
                break;

            case DamageColor.Blue:
                SetMainColor(customBlue);
                break;

            default:
                SetMainColor(Color.white);
                break;
        }

        this.spriteRenderer.color = this.spriteColor;
    }

    public void SetColor(Color color) {
        this.spriteRenderer.color = spriteColor * color;
    }

    public void ResetColor() {
        this.spriteRenderer.color = spriteColor;
    }

    public IEnumerator Fading(Color startingColor, Color targetColor, float duration) {
        SetColor(startingColor);
        for (float t = 0f; t < duration; t += Time.deltaTime) {
            SetColor(Color.Lerp(startingColor, targetColor, t / duration));
            yield return Time.deltaTime;
        }
        SetColor(targetColor);
    }

    public IEnumerator HitFlash() {
        SetColor(Color.yellow);
        yield return new WaitForSeconds(0.05f);
        SetColor(Color.red);
        yield return new WaitForSeconds(0.05f);
        SetColor(Color.white);
        yield return new WaitForSeconds(0.05f);
    }

    public IEnumerator InvulnerabilityFlash(float time) {
        Color veryTransparent = new Color(1f, 1f, 1f, 0.2f);
        Color littleTransparent = new Color(1f, 1f, 1f, 0.6f);

        for (int i = 0; i < time * 5; i++) {
            SetColor(veryTransparent);
            yield return new WaitForSeconds(0.1f);
            SetColor(littleTransparent);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
