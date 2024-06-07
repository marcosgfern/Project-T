using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
//For editor use only
public class SetTutorialCompleted : MonoBehaviour
{
    [SerializeField] private bool completed;
    void Start()
    {
        PlayerPrefs.SetString("TutorialFinished", completed.ToString());
    }

    
}
#endif