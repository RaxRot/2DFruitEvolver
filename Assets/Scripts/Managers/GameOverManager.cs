using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    [Header("Elements")] 
    [SerializeField] private GameObject deadLine;
    [SerializeField] private Transform fruitsParent;

    [Header("Timer")]
    [SerializeField] private float durationTreshold;
    private float _timer;
    private bool _timerOn;
    private bool _isGameOver;

    private void Update()
    {
        if (!_isGameOver)
        {
            ManageGameOver();
        }
    }

    private void ManageGameOver()
    {
        if (_timerOn)
        {
            ManageTime();
        }
        else
        {
            if (IsFruitAboveLine())
            {
                StartTimer();
            }
        }
    }

    private void ManageTime()
    {
        _timer += Time.deltaTime;
        if (!IsFruitAboveLine())
        {
            StopTimer();
        }
        if (_timer>=durationTreshold)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        _isGameOver = true;
        print("GAME OVER");
        
        GameManager.Instance.SetGameOverState();
    }

    private bool IfFruitAboveLine(Transform fruit)
    {
        if (fruit.position.y>deadLine.transform.position.y)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsFruitAboveLine()
    {
        for (int i = 0; i < fruitsParent.childCount; i++)
        {
            Fruit fruit = fruitsParent.GetChild(i).GetComponent<Fruit>();
            if (!fruit.HasCollided())
            {
                continue;
            }

            if ( IfFruitAboveLine(fruitsParent.GetChild(i)))
            {
                return true;
            }
           
        }

        return false;
    }

    private void StartTimer()
    {
        _timerOn = true;
    }

    private void StopTimer()
    {
        _timer = 0;
        _timerOn = false;
    }
}
