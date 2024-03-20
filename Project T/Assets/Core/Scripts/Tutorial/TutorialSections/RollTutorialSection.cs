using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class RollTutorialSection : TutorialSection
{
    [SerializeField] protected int requiredRolls = 5;
    [SerializeField] protected float minTimeBetweenRolls = 1f;

    protected int currentRolls = 0;
    protected float timeSinceLastValidRoll = 0f;

    override public string Text => tutorialMessage + "\n<size=140%>" + currentRolls + "/" + requiredRolls + "</size>";

    private void Update()
    {
        timeSinceLastValidRoll += Time.deltaTime;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        playerController.Rolled += OnPlayerRolled;
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
