using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private AudioSource mergedSource;

    private void OnEnable()
    {
        MargeManager.OnMergeProcessed += MergeProcessCallback;
        SettingsManager.onSFXValueChanged += SFXValueChangedCallback;
    }

    private void OnDisable()
    {
        MargeManager.OnMergeProcessed -= MergeProcessCallback;
        SettingsManager.onSFXValueChanged -= SFXValueChangedCallback;
    }

    private void MergeProcessCallback(FruitType arg1, Vector2 arg2)
    {
        PlayMergeSound();
    }

    private void PlayMergeSound()
    {
        mergedSource.pitch = Random.Range(0.9f, 1.1f);
        mergedSource.Play();
    }
    
    private void SFXValueChangedCallback(bool sfxActive)
    {
        mergedSource.mute = !sfxActive;
    }
}
