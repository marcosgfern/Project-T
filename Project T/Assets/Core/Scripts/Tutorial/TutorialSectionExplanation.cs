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

    [SerializeField] private float delayUntilNextCharInSeconds;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI tutorialTextBox;
    [SerializeField] private RectTransform animationParent;
    [SerializeField] private GameObject backgroundImage;

    private string targetText;
    private int currentTextLength;

    private ExplanationStates currentState = ExplanationStates.Hidden;

#nullable enable
    public void SetInfo(string text, GameObject? helpAnimationPrefab)
    {
        backgroundImage.SetActive(true);

        foreach (Transform child in animationParent.transform)
        {
            Destroy(child.gameObject);
        }

        if (helpAnimationPrefab != null)
        {
            animationParent.gameObject.SetActive(true);
            Instantiate(helpAnimationPrefab, animationParent);
        }

        targetText = text;
        currentTextLength = 0;
        currentState = ExplanationStates.Ongoing;

        StartCoroutine(UpdateTextLength());
    }
#nullable disable

    public void GoToNextState()
    {
        switch (currentState)
        {
            case ExplanationStates.Ongoing:
                StopCoroutine(UpdateTextLength());
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

    private IEnumerator UpdateTextLength()
    {
        while (currentTextLength <= targetText.Length)
        {
            tutorialTextBox.text = targetText.Substring(0, currentTextLength);
            currentTextLength++;
            yield return new WaitForSeconds(delayUntilNextCharInSeconds);
        }
        
        GoToNextState();
        yield return null;
    }

    
}