using Floors;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CameraController cameraController;
    [SerializeField] private Transform room;
    [SerializeField] private TextMeshProUGUI tutorialTextBox;

    [SerializeField] private List<TutorialSection> sections;

    private int currentSectionIndex;

    private TutorialSection CurrentSection => 
        currentSectionIndex < sections.Count ? sections[currentSectionIndex] : null;

    private void Awake()
    {
        if (sections.Count > 0)
        {
            foreach (TutorialSection section in sections)
            {
                section.SectionFinished += OnSectionFinished;
                section.TextUpdated += OnTextUpdated;
            }

            currentSectionIndex = 0;
        }
    }
    private void Start()
    {     
        cameraController.MoveToRoom(room);
        InitCurrentSection();
    }

    private void InitCurrentSection()
    {
        CurrentSection.gameObject.SetActive(true);        
    }

    private void OnSectionFinished()
    {
        if (CurrentSection.Equals(sections.Last()))
        {
            FinishTutorial();
        }
        else
        {
            CurrentSection.gameObject.SetActive(false);
            currentSectionIndex++;
            InitCurrentSection();
        }
    }

    private void OnTextUpdated(string text)
    {
        tutorialTextBox.text = text;
    }

    private void FinishTutorial() { }

}
