using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class VolumeController : MonoBehaviour
{
    public static readonly string PlayerPrefsKeyVolume = "Volume";

    [SerializeField] private List<Sprite> speakerSprites;

    [Header("References")]
    [SerializeField] private UnityEngine.UI.Image speaker;
    [SerializeField] private UnityEngine.UI.Slider slider;

    private AudioSource feedbackSound;

    private void Awake()
    {
        
    }

    void Start()
    {
        feedbackSound = GetComponent<AudioSource>();

        slider.SetValueWithoutNotify(PlayerPrefs.GetFloat(PlayerPrefsKeyVolume, 1.0f) * slider.maxValue);
        UpdateSpeakerSprite();
    }

    public void OnSliderValueChange()
    {
        PlayerPrefs.SetFloat(PlayerPrefsKeyVolume, slider.value / slider.maxValue);
        AudioListener.volume = PlayerPrefs.GetFloat(PlayerPrefsKeyVolume, 1.0f);
        UpdateSpeakerSprite();
        feedbackSound.Play();

    }

    void UpdateSpeakerSprite()
    {
        float volume = PlayerPrefs.GetFloat(PlayerPrefsKeyVolume, 1.0f);
        if (volume <= 0.0f)
        {
            speaker.sprite = speakerSprites[0];
        }
        else if (volume >= 1.0f)
        {
            speaker.sprite = speakerSprites[3];
        }
        else if (volume < 0.5f)
        {
            speaker.sprite = speakerSprites[1];
        }
        else
        {
            speaker.sprite= speakerSprites[2];
        }
    }


}
