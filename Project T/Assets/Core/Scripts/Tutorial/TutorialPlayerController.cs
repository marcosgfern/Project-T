using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPlayerController : PlayerController
{
    [Header("Tutorial Parameters")]
    [SerializeField] private bool controlEnabled = true;
    [SerializeField] private bool shootEnabled = true;
    [SerializeField] private bool swipeEnabled = true;

    public event Action Rolled;

    public bool ControlEnabled
    {
        get => controlEnabled;
        set => controlEnabled = value;
    }

    public bool ShootEnabled
    {
        get => shootEnabled;
        set => shootEnabled = value;
    }

    public bool SwipeEnabled
    {
        get => swipeEnabled;
        set => swipeEnabled = value;
    }

    // Update is called once per frame
    void Update()
    {
        if (controlEnabled)
        {
            touchManager.Update();

            switch (touchManager.GetPhase())
            {
                case TouchPhase.Moved:
                    if (swipeEnabled)
                    {
                        ChargeAttack();
                    }
                    break;
                case TouchPhase.Ended:
                    if (swipeEnabled && touchManager.IsSwipe())
                    {
                        DoMeleeAttack();
                        Rolled?.Invoke();
                    }
                    else if (shootEnabled && !touchManager.IsSwipe())
                    {
                        DoRangedAttack();
                    }
                    break;
            }
        }
    }
}
