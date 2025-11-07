using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject home;
    [SerializeField] private GameObject gameplay;
    [SerializeField] private GameObject pause;
    [SerializeField] private GameObject result;
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject about;

    [Header("Map Size Buttons")]
    [SerializeField] private Color selectedColor;
    [SerializeField] private Image[] mapSizes;

    private Color initialColor;

    private void Awake()
    {
        initialColor = mapSizes[0].color;
    }

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
        for (int i = 0; i < mapSizes.Length; i++)
        {
            mapSizes[i].color = initialColor;
        }

        mapSizes[mapIndex].color = selectedColor;
    }
}

public enum EUIPanel { Home, Gameplay, Pause, Result, Settings, About }