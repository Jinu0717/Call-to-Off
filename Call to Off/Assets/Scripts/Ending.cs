using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    private void Awake() => StartCoroutine(Click());

    private IEnumerator Click()
    {
        yield return new WaitForSeconds(1);

        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Space)) { SceneManager.LoadScene("Start"); }

            yield return null;
        }
    }
}
