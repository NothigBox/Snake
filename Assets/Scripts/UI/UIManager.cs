using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject home;
    [SerializeField] GameObject gameplay;
    [SerializeField] GameObject options;
    [SerializeField] GameObject result;

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

            case EUIPanel.Options:
                options.SetActive(true);
                break;

            case EUIPanel.Result:
                result.SetActive(true);
                break;
        }
    }

    void ClosePanels()
    {
        home.SetActive(false);
        gameplay.SetActive(false);
        options.SetActive(false);
        result.SetActive(false);
    }
}

public enum EUIPanel { Home, Gameplay, Options, Result }