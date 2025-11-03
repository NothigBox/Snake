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
        GoHome();
    }

    public void GoPlay()
    {
        level.StartLevel();

        //   Call this function in order to set the Time scale to 1
        OptionsSetActive(false);
    }

    public void GoHome()
    {
        level.ClearLevel();

        ui.SetPanel(EUIPanel.Home);
    }

    public void GoSettings()
    {
        ui.SetPanel(EUIPanel.Settings);
    }

    public void GoAbout()
    {
        ui.SetPanel(EUIPanel.About);
    }

    void GameOver()
    {
        ShowResult(1f);
    }

    public void ResetScene()
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
            ui.SetPanel(EUIPanel.Pause);
        }
        else
        {
            ui.SetPanel(EUIPanel.Gameplay);
        }

        Time.timeScale = newActive ? 0f : 1f;
    }

    public void ResetGame()
    {
        level.ResetLevel();

        //   Call this function in order to set the Time scale to 1
        OptionsSetActive(false);
    }

    public void SetMap(int mapIndex)
    {
        level.SetMapSize(mapIndex);
    }

    public void SetSpeed(float speed)
    {
        level.SetSnakeSpeed(speed);
    }
}
