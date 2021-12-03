using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour {

    private SpriteManager spriteManager;

    public AudioClip[] audioClips;
    private RandomSFXSource randomSFXSource;

    void Awake() {
        this.spriteManager = new SpriteManager(GetComponent<SpriteRenderer>());
        this.randomSFXSource = new RandomSFXSource(this.audioClips, gameObject.AddComponent<AudioSource>());
    }

    public void SetColor(Color color) {
        this.spriteManager.SetMainColor(color);
        this.spriteManager.ResetColor();
    }

    public void PlayAudioClip() {
        this.randomSFXSource.PlayRandom();
    }

    public void DestroyParticle() {
        Destroy(this.gameObject);
    }
}
