using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TutorialSection : MonoBehaviour
{
    [SerializeField] protected string tutorialMessage;
    [SerializeField] protected TutorialPlayerController playerController;

    public event Action SectionFinished;
    public event Action<string> TextUpdated;

    virtual public string Text => tutorialMessage;



    protected void UpdateText()
    {
        TextUpdated?.Invoke(Text);
    }

    protected void FinishSection()
    {
        SectionFinished?.Invoke();
    }
}
