using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FruitManager : MonoBehaviour
{
   [Header("Elements")]
   [SerializeField] private Fruit[] fruitsPrefabs;
   [SerializeField] private Fruit [] spawnableFruits;
   [SerializeField] private Transform fruitsParent;
   [SerializeField] private LineRenderer fruitSpawnLine;
   private Camera _camera;
   private Fruit _currentFruit;

   [Header("Settings")] 
   [SerializeField] private float fruitsYSpawnPos;
   [SerializeField] private float spawnDelay;
   private bool _canControl;
   private bool _isControlling;

   [Header("NextFruitSettings")]
   private int _nextFruitIndex;

   [Header("Debug")]
   [SerializeField] private bool enableGizmos;

   [Header("Actions")]
   public static Action OnFruitIndexSet;

   private void Awake()
   {
      _camera=Camera.main;
   }

   private void Start()
   {
      SetNextFruitIndex();
      _canControl = true;
      HideLine();
   }

   private void OnEnable()
   {
      MargeManager.OnMergeProcessed += MergeProcessedCallback;
   }

   private void OnDisable()
   {
      MargeManager.OnMergeProcessed-=MergeProcessedCallback;
   }

   private void MergeProcessedCallback(FruitType fruitType, Vector2 spawnPos)
   {
      for (int i = 0; i < fruitsPrefabs.Length; i++)
      {
         if (fruitsPrefabs[i].GetFruitType()==fruitType)
         {
            SpawnMergeFruit(fruitsPrefabs[i], spawnPos);
            return;
         }
      }
   }

   private void SpawnMergeFruit(Fruit fruitPrefab, Vector2 spawnPos)
   {
     Fruit fruitInstance = Instantiate(fruitPrefab, spawnPos, Quaternion.identity,fruitsParent);
     fruitInstance.EnablePhysics();
   }

   private void Update()
   {
      if (!GameManager.Instance.IsGameState())
      {
         return;
      }
      
      if (_canControl)
      {
         ManagePlayerInput();
      }
   }

   private void ManagePlayerInput()
   {
      if (Input.GetMouseButtonDown(0))
      {
         MouseDownCallback();
      }else if (Input.GetMouseButton(0))
      {
         if (_isControlling)
         {
            MouseDragCallback();
         }
         else
         {
            MouseDownCallback();
         }
      }else if (Input.GetMouseButtonUp(0) && _isControlling)
      {
         MouseUpCallback();
      }
   }

   private void MouseDownCallback()
   {
      DisplayLine();
      
      SpawnFruit();

      _isControlling = true;
   }

   private void MouseDragCallback()
   {
      fruitSpawnLine.SetPosition(0,GetSpawnPosition());
      fruitSpawnLine.SetPosition(1,GetSpawnPosition()+Vector2.down*15);

      _currentFruit.MoveTo(GetSpawnPosition());
   }

   private void MouseUpCallback()
   {
     HideLine();

     if (_currentFruit != null)
     {
        _currentFruit.EnablePhysics();
     }
     
     _canControl = false;
     StartControlTimer();
     
     _isControlling = false;
   }
   
   private void StartControlTimer()
   {
      Invoke("StopControlTimer",spawnDelay);
   }

   private void StopControlTimer()
   {
      _canControl = true;
   }

   private void SpawnFruit()
   {
      Vector2 spawnPosition = GetSpawnPosition();
      Fruit fruitObjectToInstantiate = spawnableFruits[_nextFruitIndex];
     _currentFruit = Instantiate(fruitObjectToInstantiate, spawnPosition, Quaternion.identity,fruitsParent);

     SetNextFruitIndex();
   }

   private void SetNextFruitIndex()
   {
      _nextFruitIndex = Random.Range(0, spawnableFruits.Length);
      OnFruitIndexSet?.Invoke();
   }

   public string GetNextFruitName()
   {
      return spawnableFruits[_nextFruitIndex].name;
   }

   private Vector2 GetClickedWorldPosition()
   {
      return _camera.ScreenToWorldPoint(Input.mousePosition);
   }

   private Vector2 GetSpawnPosition()
   {
      Vector2 clickedWorldPosition = GetClickedWorldPosition();
      clickedWorldPosition.y = fruitsYSpawnPos;
      return clickedWorldPosition;
   }

   private void DisplayLine()
   {
      fruitSpawnLine.enabled = true;
   }
   
   private void HideLine()
   {
      fruitSpawnLine.enabled = false;
   }

#if UNITY_EDITOR //will not compile when build the game
   private void OnDrawGizmos()
   {
      if (!enableGizmos)
      {
         return;
      }

      var fromShowGizmos = new Vector3(-50, fruitsYSpawnPos, 0);
      var toShowGizmos = new Vector3(50, fruitsYSpawnPos, 0);
      Gizmos.color=Color.yellow;
      Gizmos.DrawLine(fromShowGizmos,toShowGizmos);
   }
#endif

   public Sprite GetNextFruitSprite()
   {
      return spawnableFruits[_nextFruitIndex].GetSprite();
   }
}
