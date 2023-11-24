using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("Elements")] 
    [SerializeField] private GameObject resetProgressPrompt;
    private const string sfxActiveKey = "sfx";
    [SerializeField] private Toggle sfxToggle;
    private bool _canSave;

    [Header("Actions")]
    public static Action<bool> onSFXValueChanged;
    
    private void Start()
    {
        LoadData();
        ToggleCallback(sfxToggle.isOn);
        
        Invoke("CanSave",1f);
    }

    private void CanSave()
    {
        _canSave = true;
    }

    public void ResetProgressButtonCallback()
    {
        resetProgressPrompt.SetActive(true);
    }

    public void ResetProgressYes()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(0);
    }

    public void ResetProgressNo()
    {
        resetProgressPrompt.SetActive(false);
    }

    public void ToggleCallback(bool sfxActive)
    {
        onSFXValueChanged?.Invoke(sfxActive);
        
        SaveData();
    }

    private void LoadData()
    {
        sfxToggle.isOn=PlayerPrefs.GetInt(sfxActiveKey) == 1;
    }

    private void SaveData()
    {
        if (!_canSave)
        {
            return;
        }

        int sfxValue = sfxToggle.isOn ? 1 : 0;
        PlayerPrefs.SetInt(sfxActiveKey,sfxValue);
    }
}
