using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSFXController : MonoBehaviour {

    public AudioClip[] shots;
    private RandomSFXSource randomShots;


    private void Awake() {
        this.randomShots = new RandomSFXSource(this.shots, gameObject.AddComponent<AudioSource>());
    }

    public void PlayShot() {
        this.randomShots.PlayRandom();
    }
}
