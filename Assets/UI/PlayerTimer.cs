﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTimer : MonoBehaviour
{
    public float playTime = 0;
    public bool timerIsRunning = false;
    public Text timeText;

    private void Start()
    {
        // Starts the timer automatically
        timerIsRunning = true;
    }

    void Update()
    {

        if (timerIsRunning)
        {
            playTime += Time.deltaTime;
            if (playTime > 60)
            {
                DisplayTime(playTime - 60);
            }
            else
            {
                timeText.text = "HIDE!";
            }
        }
        else
        {
            DisplayTime(playTime - 60);
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}