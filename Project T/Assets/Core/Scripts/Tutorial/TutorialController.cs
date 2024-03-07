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

    private int currentSection;

    private TutorialSection CurrentSection => 
        currentSection < sections.Count ? sections[currentSection] : null;

    private void Awake()
    {
        if (sections.Count > 0)
        {
            foreach (TutorialSection section in sections)
            {
                section.SectionFinished += OnSectionFinished;
                section.TextUpdated += OnTextUpdated;
            }

            currentSection = 0;
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
        CurrentSection.gameObject.SetActive(false);
        currentSection++;
        InitCurrentSection();
    }

    private void OnTextUpdated(string text)
    {
        tutorialTextBox.text = text;
    }

}
