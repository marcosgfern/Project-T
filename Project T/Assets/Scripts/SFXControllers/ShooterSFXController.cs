using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterSFXController : EnemySFXController {

    public AudioClip[] shotChargings;
    private RandomSFXSource randomShotChragings;

    public AudioClip[] shots;
    private RandomSFXSource randomShots;

    protected override void Awake() {
        base.Awake();
        this.randomShots = new RandomSFXSource(this.shots, gameObject.AddComponent<AudioSource>());
        this.randomShotChragings = new RandomSFXSource(this.shotChargings, gameObject.AddComponent<AudioSource>());
    }

    public void PlayShot() {
        this.randomShots.PlayRandom();
    }

    public void PlayShotCharging() {
        this.randomShotChragings.PlayRandom();
    }
}
