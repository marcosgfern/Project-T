using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TutorialSection : MonoBehaviour
{
    [SerializeField] protected TutorialPlayerController playerController;
    [SerializeField] protected Transform playerStartingPosition;

    [TextArea]
    [SerializeField] protected string tutorialMessage;
    [SerializeField] protected GameObject helpAnimationPrefab;

    [Header("Control settings")]
    [SerializeField] protected bool controlEnabled;
    [SerializeField] protected bool shootEnabled;
    [SerializeField] protected bool swipeEnabled;

    public event Action SectionFinished;
    public event Action<string> TextUpdated;

    virtual public string Text => tutorialMessage;
    public GameObject HelpAnimationPrefab => helpAnimationPrefab;

    protected virtual void OnEnable()
    {
        playerController.ControlEnabled = controlEnabled;
        playerController.ShootEnabled = shootEnabled;
        playerController.SwipeEnabled = swipeEnabled;

        UpdateInfo();
        SetPlayerStartingPosition();
    }

    protected void UpdateInfo()
    {
        TextUpdated?.Invoke(Text);
    }

    protected void SetPlayerStartingPosition()
    {
        playerController.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        playerController.gameObject.transform
            .SetPositionAndRotation(playerStartingPosition.position, 
                                    playerStartingPosition.rotation);
    }

    protected void FinishSection()
    {
        playerController.ControlEnabled = false;
        SectionFinished?.Invoke();
    }
}
