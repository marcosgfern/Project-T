using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSFXController : MonoBehaviour {

    public AudioClip[] shots;
    private RandomSFXSource randomShots;

    public AudioClip[] flips;
    private RandomSFXSource randomFlips;


    private void Awake() {
        this.randomShots = new RandomSFXSource(this.shots, gameObject.AddComponent<AudioSource>());
        this.randomFlips = new RandomSFXSource(this.flips, gameObject.AddComponent<AudioSource>());
    }

    public void PlayShot() {
        this.randomShots.PlayRandom();
    }

    public void PlayFlip() {
        this.randomFlips.PlayRandom();
    }
}
