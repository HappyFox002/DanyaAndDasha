using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController Controller;
    [SerializeField] GameObject BuildPanel;
    [SerializeField] GameObject GamePanel;
    [SerializeField] GameObject GameOverPanel;
    [SerializeField] GameObject PausedPanel;

    private void Awake()
    {
        Controller = this;
    }

    private void DisableAll()
    {
        GamePanel.SetActive(false);
        GameOverPanel.SetActive(false);
        PausedPanel.SetActive(false);
    }

    public void ResumeGame() {
        GameState.IsPaused = false;
        DisableAll();
        GamePanel.SetActive(true);
    }

    public void PausedGame() {
        GameState.IsPaused = true;
        DisableAll();
        PausedPanel.SetActive(true);
    }

    public void ExitGame() {
        Application.Quit();
    }

    public void GameOver() {
        GameState.IsPaused = true;
        DisableAll();
        GameOverPanel.SetActive(true);
    }

    public void StartGame() {
        SceneManager.LoadScene("GameScene");
    }

    public void BackToMenu() {
        SceneManager.LoadScene("MenuScene");
    }

    public void OpenOrCloseBuildPanel() {
        BuildPanel.SetActive(!BuildPanel.activeSelf);
    }
}
