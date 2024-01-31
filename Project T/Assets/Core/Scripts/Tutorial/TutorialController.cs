using Floors;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CameraController cameraController;
    [SerializeField] private Transform room;

    [SerializeField] private List<TutorialSection> sections;

    private TutorialSection currentSection;

    private void Awake()
    {
        if (sections.Count > 0)
        {
            foreach (TutorialSection section in sections)
            {
                section.SectionFinished += OnSectionFinished;
            }

            currentSection = sections[0];
        }
    }
    private void Start()
    {     
        cameraController.MoveToRoom(room);
    }

    private void OnSectionFinished()
    {

    }

}
