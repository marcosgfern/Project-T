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


    public void StartFloorTransition(int floor) {
        floorText.text = FloorString + floor.ToString();
        animator.SetTrigger("Start");
    }

    private void OnCrossfadeBlack() {
        BlackedOut?.Invoke();
    }
}
