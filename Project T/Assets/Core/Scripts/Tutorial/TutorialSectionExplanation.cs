using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialSectionExplanation : MonoBehaviour
{
    [SerializeField] private float delayUntilNextCharInSeconds;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI tutorialTextBox;
    [SerializeField] private RectTransform helpAnimationParent;
    [SerializeField] private GameObject backgroundImage;

    private string targetText;
    private int currentTextLength;

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
    }

    public void StartExplanation()
    {
        StartCoroutine(UpdateTextLength());
    }

    public void FinishExplanation()
    {
        helpAnimationParent.gameObject.SetActive(false);
        backgroundImage.SetActive(false);
    }

    private IEnumerator UpdateTextLength()
    {
        while (currentTextLength <= targetText.Length)
        {
            tutorialTextBox.text = targetText.Substring(0, currentTextLength);
            GoToNextNonTagCharacter();
            yield return new WaitForSeconds(delayUntilNextCharInSeconds);
        }
        
        FinishExplanation();
        yield return null;
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