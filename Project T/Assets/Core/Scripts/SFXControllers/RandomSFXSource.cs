using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSFXSource {

    private static System.Random IndexRandomizer = new System.Random();

    private AudioClip[] clips;
    private AudioSource effectSource;
    private int clipIndex;

    public RandomSFXSource(AudioClip[] clips, AudioSource effectSource) {
        this.clips = clips;
        this.effectSource = effectSource;
        this.effectSource.playOnAwake = false;
    }

    public void PlayRandom() {
        clipIndex = RepeatCheck(this.clipIndex, this.clips.Length);
        effectSource.PlayOneShot(this.clips[clipIndex]);
    }

    private int RepeatCheck(int previousIndex, int range) {
        int index = IndexRandomizer.Next(0, range);

        while (index == previousIndex) {
            index = IndexRandomizer.Next(0, range);
        }
        return index;
    }
}
