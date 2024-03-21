using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTutorialSection : TutorialSection
{
    [SerializeField] protected int requiredShots = 5;

    protected int currentShots = 0;

    override public string Text => tutorialMessage + "\n<size=140%>" + currentShots + "/" + requiredShots + "</size>";

    protected override void OnEnable()
    {
        base.OnEnable();
        playerController.Shot += OnPlayerShot;
    }

    private void OnDisable()
    {
        playerController.Shot -= OnPlayerShot;
    }

    protected void OnPlayerShot()
    {
        currentShots++;
        UpdateText();

        if (currentShots >= requiredShots)
        {
            FinishSection();
        }
    }

}
