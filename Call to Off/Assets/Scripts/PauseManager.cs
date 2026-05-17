using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject hand;
    [Header("일시정지 패널")]
    [SerializeField] private GameObject pausePanel;

    [Header("오디오 정지")]
    [SerializeField] private bool pauseAudio = true;

    private bool isPaused = false;

    private void Awake()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        ResumeGame();
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape))
            return;

        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        isPaused = true;

        if (pausePanel != null)
            pausePanel.SetActive(true);

        Time.timeScale = 0f;

        if (!hand.activeSelf)
            Cursor.visible = true;

        if (pauseAudio)
            AudioListener.pause = true;
    }

    public void ResumeGame()
    {
        isPaused = false;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        Time.timeScale = 1f;
        Cursor.visible = false;

        if (pauseAudio)
            AudioListener.pause = false;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }
}