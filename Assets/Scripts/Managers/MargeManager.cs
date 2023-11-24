using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MargeManager : MonoBehaviour
{
    [Header("Settings")]
    private Fruit _lastSender;

    [Header("Actions")] 
    public static Action<FruitType, Vector2> OnMergeProcessed;
    
    private void OnEnable()
    {
        Fruit.OnCollisionWithFruit += CollisionsBetweenFruitsCallback;
    }

    private void OnDisable()
    {
        Fruit.OnCollisionWithFruit -= CollisionsBetweenFruitsCallback;
    }

    private void CollisionsBetweenFruitsCallback(Fruit sender,Fruit otherFruit)
    {
        if (_lastSender!=null)
        {
            return;
        }
        _lastSender = sender;
        ProcessMerge(sender, otherFruit);
    }

    private void ProcessMerge(Fruit sender, Fruit otherFruit)
    {
        FruitType mergeFruitType = sender.GetFruitType();
        mergeFruitType++;
        Vector2 fruitSpawnPos = (sender.transform.position + otherFruit.transform.position) / 2;
        
        sender.Merge();
        otherFruit.Merge();

        ResetLastSender();

        OnMergeProcessed?.Invoke(mergeFruitType, fruitSpawnPos);
    }

    private void ResetLastSender()
    {
        StartCoroutine(nameof(_ResetLastSenderCo));
    }

    private IEnumerator _ResetLastSenderCo()
    {
        yield return new WaitForEndOfFrame();
        _lastSender = null;
    }
}
