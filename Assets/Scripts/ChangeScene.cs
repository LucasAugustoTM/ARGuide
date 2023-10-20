using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;
using System.IO;
using UnityEngine.UI;
using System;
using System.Text;

[RequireComponent(typeof(ARTrackedImageManager))]
public class ChangeScene : MonoBehaviour
{
    public PrefabImagePairManager reseta;

    public void StartApp() {
        SceneManager.LoadScene(1);
    }

     public void RestartApp() {
        //var reseta = GetComponent<PrefabImagePairManager>();
        reseta.Limpa();
        SceneManager.LoadScene(1);
    }

    public void EndApp() {
        SceneManager.LoadScene(2);
    }

    public void QuitApp() {
        Application.Quit();
    }
}
