using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] LevelManager level;
    [SerializeField] UIManager ui;

    private void Awake()
    {
        level.OnGameOver += GameOver;
    }

    private void Start()
    {
        ui.SetPanel(EUIPanel.Home);
    }

    public void GoPlay()
    {
        level.StartLevel();
        OptionsSetActive(false);
    }

    void GameOver()
    {
        ShowResult(1f);
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene("MainScene");
    }

    void ShowResult(float delay)
    {
        Invoke(nameof(ShowResult), delay);
    }

    void ShowResult()
    {
        ui.SetPanel(EUIPanel.Result);
    }

    public void OptionsSetActive(bool newActive)
    {
        if (newActive == true)
        {
            ui.SetPanel(EUIPanel.Options);
        }
        else
        {
            ui.SetPanel(EUIPanel.Gameplay);
        }

        Time.timeScale = newActive ? 0f : 1f;
    }
}
