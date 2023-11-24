using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(FruitManager))]
public class FruitManagerUI : MonoBehaviour
{
    [Header("Elements")]
    FruitManager _fruitManager;
    [SerializeField] private Image nextFruitImage;

    private void OnEnable()
    {
        FruitManager.OnFruitIndexSet += UpdateNextFruitImage;
    }

    private void OnDisable()
    {
        FruitManager.OnFruitIndexSet -= UpdateNextFruitImage;
    }

    private void Start()
    {
        //_fruitManager = GetComponent<FruitManager>();
    }

    private void UpdateNextFruitImage()
    {
        if (_fruitManager==null)
        {
            _fruitManager = GetComponent<FruitManager>();
        }
        nextFruitImage.sprite = _fruitManager.GetNextFruitSprite();
    }
}
