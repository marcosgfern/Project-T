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

    public void Start() {
        this.chromaticAberrationToggle.isOn = this.postProcessProfile.GetSetting<ChromaticAberration>().enabled.value;
        this.lensDistortionToggle.isOn = this.postProcessProfile.GetSetting<LensDistortion>().enabled.value;
        this.bloomToggle.isOn = this.postProcessProfile.GetSetting<Bloom>().enabled.value;
    }

    public void SetChromaticAberration(bool enabled) {
        this.postProcessProfile.GetSetting<ChromaticAberration>().enabled.value = enabled;
    }

    public void SetLensDistortion(bool enabled) {
        this.postProcessProfile.GetSetting<LensDistortion>().enabled.value = enabled;
    }

    public void SetBloom(bool enabled) {
        this.postProcessProfile.GetSetting<Bloom>().enabled.value = enabled;
    }
}
