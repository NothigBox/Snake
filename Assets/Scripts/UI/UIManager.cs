using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] GameObject home;
    [SerializeField] GameObject gameplay;
    [SerializeField] GameObject pause;
    [SerializeField] GameObject result;
    [SerializeField] GameObject settings;
    [SerializeField] GameObject about;

    [Header("Map Size Buttons")]
    [SerializeField] Color SelectedColor;
    [SerializeField] Button[] mapSizes;

    public void SetPanel(EUIPanel panel)
    {
        ClosePanels();

        switch (panel)
        {
            case EUIPanel.Home:
                home.SetActive(true);
                break;

            case EUIPanel.Gameplay:
                gameplay.SetActive(true);
                break;

            case EUIPanel.Pause:
                pause.SetActive(true);
                break;

            case EUIPanel.Result:
                result.SetActive(true);
                break;

            case EUIPanel.Settings:
                settings.SetActive(true);
                break;

            case EUIPanel.About:
                about.SetActive(true);
                break;
        }
    }

    void ClosePanels()
    {
        home.SetActive(false);
        gameplay.SetActive(false);
        pause.SetActive(false);
        result.SetActive(false);
        settings.SetActive(false);
        about.SetActive(false);
    }

    public void SetSelectedMapSize(int mapIndex)
    {
        
    }
}

public enum EUIPanel { Home, Gameplay, Pause, Result, Settings, About }