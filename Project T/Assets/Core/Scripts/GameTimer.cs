using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimer : MonoBehaviour
{

    private float timeCounter;
    private bool pauseTimer;

    public float CurrentTime { get => timeCounter; }

    void Start()
    {
        timeCounter = 0;
        pauseTimer = true;
    }

    void Update()
    {
        if (!pauseTimer)
        {
            timeCounter += Time.deltaTime;
        }
    }

    public void Pause()
    {
        pauseTimer = true;
    }

    public void Resume()
    {
        pauseTimer = false;
    }

    public void Restart()
    {
        timeCounter = 0;
    }
}
