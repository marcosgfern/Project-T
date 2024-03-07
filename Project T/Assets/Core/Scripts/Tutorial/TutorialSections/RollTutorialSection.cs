using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class RollTutorialSection : TutorialSection
{
    protected int requiredRolls = 3;
    protected float minTimeBetweenRolls = 1f;

    protected int currentRolls = 0;
    protected float timeSinceLastValidRoll = 0f;

    override public string Text => tutorialMessage + "\n<size=140%>" + currentRolls + "/" + requiredRolls + "</size>";

    private void Update()
    {
        timeSinceLastValidRoll += Time.deltaTime;
    }

    private void OnEnable()
    {
        playerController.Rolled += OnPlayerRolled;

        playerController.ControlEnabled = true;
        playerController.ShootEnabled = false;
        playerController.SwipeEnabled = true;

        UpdateText();
    }

    private void OnDisable()
    {
        playerController.Rolled -= OnPlayerRolled;
    }

    protected void OnPlayerRolled()
    {
        if (timeSinceLastValidRoll >= minTimeBetweenRolls)
        {
            currentRolls++;
            UpdateText();

            if (currentRolls >= requiredRolls)
            {
                FinishSection();
            }
            else
            {
                timeSinceLastValidRoll = 0f;
            }           
        }
    }
}
