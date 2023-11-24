using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [Header("Elements")] 
    [SerializeField] private TextMeshProUGUI gameScoreText;
    [SerializeField] private TextMeshProUGUI gameBestScore;

    [Header("Settings")]
    private int _score;
    private int _bestScore;

    [Header("Data")] 
    private const string BEST_SCORE_KEY = "bestScore";
    
    private void OnEnable()
    {
        MargeManager.OnMergeProcessed += MergeProcessCallback;
        GameManager.OnGameStateChanged += GameStateChangedCallback;
    }

    private void OnDisable()
    {
        MargeManager.OnMergeProcessed -= MergeProcessCallback;
        GameManager.OnGameStateChanged -= GameStateChangedCallback;
    }
    
    private void Start()
    {
        LoadData();
        UpdateScore();
    }

    private void MergeProcessCallback(FruitType fruitType, Vector2 unised)
    {
        int scoreToAdd = (int)fruitType;
        _score += scoreToAdd;
        
        UpdateScore();
    }

    private void UpdateScore()
    {
        gameScoreText.text = _score.ToString();
    }
    
    private void GameStateChangedCallback(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.GameOver:
                CalculateBestScore();
                break;
        }
    }

    private void CalculateBestScore()
    {
        if (_score>_bestScore)
        {
            _bestScore = _score;
            SaveData();
        }
    }

    private void LoadData()
    {
        _bestScore = PlayerPrefs.HasKey(BEST_SCORE_KEY) ? PlayerPrefs.GetInt(BEST_SCORE_KEY) : 0;
        gameBestScore.text = _bestScore.ToString();
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt(BEST_SCORE_KEY,_bestScore);
    }
}
