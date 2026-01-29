using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private GameObject store;

    [Header("Score")]
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI finalScore;

    [Header("Map Size Buttons")]
    [SerializeField] private Color selectedColor;
    [SerializeField] private Image[] mapSizes;

    [Header("Store")]
    [SerializeField] private RectTransform storeContent;
    [SerializeField] private float duration = 1f;

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
                finalScore.text = score.text;
                result.SetActive(true);
                break;

            case EUIPanel.Settings:
                settings.SetActive(true);
                break;

            case EUIPanel.About:
                about.SetActive(true);
                break;

            case EUIPanel.Store:
                storeContent.anchoredPosition = new Vector2(storeContent.anchoredPosition.x, 0f);
                store.SetActive(true);
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
        store.SetActive(false);
    }

    public void SetSelectedMapSize(int mapIndex)
    {
        for (int i = 0; i < mapSizes.Length; i++)
        {
            mapSizes[i].color = initialColor;
        }

        mapSizes[mapIndex].color = selectedColor;
    }

    public void UpdateScore(int currentScore)
    {
        score.text = currentScore.ToString();
    }

    public void GoDown()
    {
        StartCoroutine(nameof(GoDownCoroutine));
    }

    IEnumerator GoDownCoroutine()
    {
        float elapsed = 0f;
        float finalPosition = storeContent.sizeDelta.y;
        Vector2 initialPosition = storeContent.anchoredPosition;
        Vector2 targetPosition = new Vector2(initialPosition.x, finalPosition);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // Position interpolation
            storeContent.anchoredPosition = Vector2.Lerp(initialPosition, targetPosition, t);

            yield return null;
        }

        // Always get to the final position
        storeContent.anchoredPosition = targetPosition;
    }
}

public enum EUIPanel { Home, Gameplay, Pause, Result, Settings, About, Store }