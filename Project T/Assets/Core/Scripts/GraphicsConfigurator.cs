using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

/* This script should be attached to a game object 
 * that is enabled at the beginning.
 */
public class GraphicsConfigurator : MonoBehaviour
{
    [SerializeField] private PostProcessProfile postProcessProfile;
    void Start()
    {
        this.postProcessProfile.GetSetting<ChromaticAberration>().enabled.value =
            bool.Parse(PlayerPrefs.GetString("ChromaticAberration", "True"));

        this.postProcessProfile.GetSetting<LensDistortion>().enabled.value =
            bool.Parse(PlayerPrefs.GetString("LensDistortion", "True"));

        this.postProcessProfile.GetSetting<Bloom>().enabled.value =
            bool.Parse(PlayerPrefs.GetString("Bloom", "True")); ;
    }
}
