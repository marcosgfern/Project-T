using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloorFade : MonoBehaviour
{
    public static readonly string FloorString = "FLOOR ";

    [SerializeField] private Animator animator;
    [SerializeField] private TextMeshProUGUI floorText;

    public event System.Action BlackedOut;
    public event System.Action Transparent;


    public void StartFloorTransition(int floor = -1) {
        if (floor != -1)
        {
            floorText.text = FloorString + (floor+1).ToString();
        }

        animator.SetTrigger("Start");
    }

    //Used by Animator
    private void OnCrossfadeBlack() {
        BlackedOut?.Invoke();
    }

    private void OnCrossfadeTransparent()
    {
        Transparent?.Invoke();
    }
}
