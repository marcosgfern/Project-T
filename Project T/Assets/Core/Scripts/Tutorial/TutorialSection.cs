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
    public event Action<string, GameObject?> InfoUpdated;

    virtual public string Text => tutorialMessage;

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
        InfoUpdated?.Invoke(Text, helpAnimationPrefab);
    }

    protected void SetPlayerStartingPosition()
    {
        playerController.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        playerController.gameObject.transform.position = playerStartingPosition.position;
        playerController.gameObject.transform.rotation = playerStartingPosition.rotation;
    }

    protected void FinishSection()
    {
        playerController.ControlEnabled = false;
        SectionFinished?.Invoke();
    }
}
