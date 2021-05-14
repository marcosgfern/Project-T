using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour {

    public PostProcessProfile postProcessProfile;

    public Toggle 
        chromaticAberrationToggle, 
        lensDistortionToggle, 
        bloomToggle;

    private ChromaticAberration chromaticAberration;
    private LensDistortion lensDistortion;
    private Bloom bloom;

    public void Awake() {
        this.chromaticAberration = this.postProcessProfile.GetSetting<ChromaticAberration>();
        this.lensDistortion = this.postProcessProfile.GetSetting<LensDistortion>();
        this.bloom = this.postProcessProfile.GetSetting<Bloom>();
    }

    public void Start() {
        this.chromaticAberration.enabled.value = bool.Parse(PlayerPrefs.GetString("ChromaticAberration", "True"));
        this.lensDistortion.enabled.value = bool.Parse(PlayerPrefs.GetString("LensDistortion", "True"));
        this.bloom.enabled.value = bool.Parse(PlayerPrefs.GetString("Bloom", "True"));

        this.chromaticAberrationToggle.isOn = this.chromaticAberration.enabled.value;
        this.lensDistortionToggle.isOn = this.lensDistortion.enabled.value;
        this.bloomToggle.isOn = this.bloom.enabled.value;
    }

    public void SetChromaticAberration(bool enabled) {
        this.chromaticAberration.enabled.value = enabled;
        PlayerPrefs.SetString("ChromaticAberration", enabled.ToString());
    }

    public void SetLensDistortion(bool enabled) {
        this.lensDistortion.enabled.value = enabled;
        PlayerPrefs.SetString("LensDistortion", enabled.ToString());
    }

    public void SetBloom(bool enabled) {
        this.bloom.enabled.value = enabled;
        PlayerPrefs.SetString("Bloom", enabled.ToString());
    }
}
