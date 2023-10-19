using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void StartApp() {
        SceneManager.LoadScene(1);
    }

    public void EndApp() {
        SceneManager.LoadScene(2);
    }

    public void QuitApp() {
        Application.Quit();
    }
}
