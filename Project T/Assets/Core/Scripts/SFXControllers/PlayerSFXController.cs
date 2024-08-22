using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSFXController : MonoBehaviour {

    [SerializeField] private AudioClip[] shots;
    [SerializeField] private AudioClip[] flips;
    [SerializeField] private AudioClip[] hits;

    private RandomSFXSource randomShots,
                            randomFlips,
                            randomHits;

    private void Awake()
    {
        this.randomShots = new RandomSFXSource(this.shots, gameObject.AddComponent<AudioSource>());
        this.randomFlips = new RandomSFXSource(this.flips, gameObject.AddComponent<AudioSource>());
        this.randomHits  = new RandomSFXSource(this.hits,  gameObject.AddComponent<AudioSource>());
    }

    public void PlayShot()
    {
        this.randomShots.PlayRandom();
    }

    public void PlayFlip()
    {
        this.randomFlips.PlayRandom();
    }

    public void PlayHit()
    {
        this.randomHits.PlayRandom();
    }
}
