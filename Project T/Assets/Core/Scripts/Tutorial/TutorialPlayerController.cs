using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPlayerController : PlayerController
{
    [Header("Tutorial Parameters")]
    [SerializeField] private bool controlEnabled = true;
    [SerializeField] private bool shootEnabled = true;
    [SerializeField] private bool swipeEnabled = true;

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
