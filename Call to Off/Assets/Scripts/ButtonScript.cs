using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    public void click(string sceneName) => SceneManager.LoadScene(sceneName);
}
