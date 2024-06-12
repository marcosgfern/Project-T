using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialSectionExplanation : MonoBehaviour
{
    private enum ExplanationStates
    {
        Ready,
        Started,
        TextFinished,
        Finished
    }

    [SerializeField] private float delayUntilNextCharInSeconds;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI tutorialTextBox;
    [SerializeField] private RectTransform helpAnimationParent;
    [SerializeField] private GameObject backgroundImage;

    private ExplanationStates currentState;

    private string targetText;
    private int currentTextLength;
    private float timeElapsedSinceLastChar;

    private IEnumerator updateTextCoroutine;


    private void Update()
    {
        if (currentState == ExplanationStates.Started)
        {
            if(currentTextLength <= targetText.Length)
            {
                if (timeElapsedSinceLastChar >= delayUntilNextCharInSeconds)
                {
                    timeElapsedSinceLastChar = 0;
                    tutorialTextBox.text = targetText.Substring(0, currentTextLength);
                    GoToNextNonTagCharacter();
                }
                else
                {
                    timeElapsedSinceLastChar += Time.deltaTime;
                }
            }
            else
            {
                currentState = ExplanationStates.TextFinished;
            }
        }
    }

    public void SetInfo(string text, GameObject helpAnimationPrefab)
    {
        backgroundImage.SetActive(true);

        foreach (Transform child in helpAnimationParent.transform)
        {
            Destroy(child.gameObject);
        }

        if (helpAnimationPrefab != null)
        {
            helpAnimationParent.gameObject.SetActive(true);
            Instantiate(helpAnimationPrefab, helpAnimationParent);
        }

        targetText = text;
        currentTextLength = 0;
        tutorialTextBox.text = "";
        currentState = ExplanationStates.Ready;
    }

    public void StartExplanation()
    {
        currentState = ExplanationStates.Started;
    }

    public void GoToNextStep()
    {
        switch (currentState)
        {
            case ExplanationStates.Ready:
                StartExplanation();
                break;

            case ExplanationStates.Started:
                tutorialTextBox.text = targetText;
                currentState = ExplanationStates.TextFinished;
                break;

            case ExplanationStates.TextFinished:
                helpAnimationParent.gameObject.SetActive(false);
                backgroundImage.SetActive(false);
                currentState = ExplanationStates.Finished;
                break;
            case ExplanationStates.Finished: break;
        }
    }

    private void GoToNextNonTagCharacter()
    {
        try
        {
            if (targetText[currentTextLength] == '<')
            {
                while (targetText[currentTextLength] != '>')
                {
                    currentTextLength++;
                }
            }

            currentTextLength++;
        }
        catch (IndexOutOfRangeException)
        {
            currentTextLength = targetText.Length + 1;
        }
    }

    
}