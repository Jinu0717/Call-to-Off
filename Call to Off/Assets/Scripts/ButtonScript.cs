using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    private void Awake() => Cursor.visible = true;

    public void click(string sceneName) => SceneManager.LoadScene(sceneName);

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
}
