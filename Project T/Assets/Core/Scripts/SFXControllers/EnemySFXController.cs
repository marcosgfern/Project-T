using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySFXController : MonoBehaviour {

    public AudioClip[] steps;
    private RandomSFXSource randomSteps;

    public AudioClip[] hurts;
    private RandomSFXSource randomHurts;


    protected virtual void Awake() {
        this.randomSteps = new RandomSFXSource(this.steps, gameObject.AddComponent<AudioSource>());
        this.randomHurts = new RandomSFXSource(this.hurts, gameObject.AddComponent<AudioSource>());
    }

    public void PlayStep() {
        this.randomSteps.PlayRandom();
    }

    public void PlayHurt() {
        this.randomHurts.PlayRandom();
    }
}
