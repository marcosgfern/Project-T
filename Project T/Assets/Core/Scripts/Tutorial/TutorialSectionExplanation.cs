using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialSectionExplanation : MonoBehaviour
{
    private enum ExplanationStates
    {
        Ongoing,
        Finished,
        Hidden
    }

    [SerializeField] private TextMeshProUGUI tutorialTextBox;
    [SerializeField] private RectTransform animationParent;
    [SerializeField] private GameObject backgroundImage;

    private string targetText;
    private int currentTextLength;

    private ExplanationStates currentState = ExplanationStates.Hidden;

    public void GoToNextState()
    {
        switch (currentState)
        {
            case ExplanationStates.Ongoing:
                tutorialTextBox.text = targetText;
                currentState = ExplanationStates.Finished;
                break;

            case ExplanationStates.Finished:
                animationParent.gameObject.SetActive(false);
                backgroundImage.SetActive(false);
                currentState = ExplanationStates.Hidden;
                break;

            case ExplanationStates.Hidden:
                break;
        }
    }

    private void Update()
    {
        if (currentState == ExplanationStates.Ongoing)
        {
            UpdateTextLength();
        }
    }

    private void UpdateTextLength()
    {
        if (currentTextLength <= targetText.Length)
        {
            tutorialTextBox.text = targetText.Substring(0, currentTextLength);
            currentTextLength++;
        }
        else
        {
            GoToNextState();
        }
    }

    public void SetInfo(string text, GameObject helpAnimationPrefab)
    {
        animationParent.gameObject.SetActive(true);
        backgroundImage.SetActive(true);

        targetText = text;
        currentTextLength = 0;
        currentState = ExplanationStates.Ongoing;

        foreach (Transform child in animationParent.transform)
        {
            Destroy(child.gameObject);
        }

        Instantiate(helpAnimationPrefab, animationParent);
    }
}