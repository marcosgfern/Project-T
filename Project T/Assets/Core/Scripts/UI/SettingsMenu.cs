using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

/* Class SettingsMenu is a component of the settings menu.
 * Responsible both for changing settings and saving them.
 */
public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private PostProcessProfile postProcessProfile;
    [SerializeField] private Toggle
        chromaticAberrationToggle, 
        lensDistortionToggle, 
        bloomToggle;
    [SerializeField] private Button replayTutorialButton;

    private ChromaticAberration chromaticAberration;
    private LensDistortion lensDistortion;
    private Bloom bloom;

    public void Awake()
    {
        this.chromaticAberration = this.postProcessProfile.GetSetting<ChromaticAberration>();
        this.lensDistortion = this.postProcessProfile.GetSetting<LensDistortion>();
        this.bloom = this.postProcessProfile.GetSetting<Bloom>();
    }

    public void Start() 
    {
        this.chromaticAberrationToggle.isOn = this.chromaticAberration.enabled.value;
        this.lensDistortionToggle.isOn = this.lensDistortion.enabled.value;
        this.bloomToggle.isOn = this.bloom.enabled.value;

        this.replayTutorialButton.gameObject
            .SetActive(bool.Parse(PlayerPrefs.GetString("TutorialFinished", "False")));
    }

    public void SetChromaticAberration(bool enabled)
    {
        this.chromaticAberration.enabled.value = enabled;
        PlayerPrefs.SetString("ChromaticAberration", enabled.ToString());
    }

    public void SetLensDistortion(bool enabled)
    {
        this.lensDistortion.enabled.value = enabled;
        PlayerPrefs.SetString("LensDistortion", enabled.ToString());
    }

    public void SetBloom(bool enabled)
    {
        this.bloom.enabled.value = enabled;
        PlayerPrefs.SetString("Bloom", enabled.ToString());
    }
}
