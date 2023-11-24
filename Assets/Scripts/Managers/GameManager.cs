using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance;
   
   [Header("Settings")]
   private GameState _gameState;
   
   [Header("Events")] 
   public static Action<GameState> OnGameStateChanged;

   private void Awake()
   {
      if (Instance==null)
      {
         Instance = this;
      }
      else
      {
         Destroy(gameObject);
      }
   }

   private void Start()
   {
      SetMenu();
   }

   private void SetMenu()
   {
      SetGameState(GameState.Menu);
   }

   public void GoToMenu()
   {
      SceneManager.LoadScene(0);
   }

   private void SetGame()
   {
      SetGameState(GameState.Game);
   }

   private void SetGameOver()
   {
      SetGameState(GameState.GameOver);
   }

   private void SetGameState(GameState gameState)
   {
      _gameState = gameState;
      OnGameStateChanged?.Invoke(_gameState);
   }

   public GameState GetGameState()
   {
      return _gameState;
   }

   public void SetGameState()
   {
      SetGame();
   }

   public bool IsGameState()
   {
      return _gameState == GameState.Game;
   }

   public void SetGameOverState()
   {
      SetGameOver();
   }
}
