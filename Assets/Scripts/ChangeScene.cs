using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.IO;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Unity.XR.CoreUtils;
using Unity.Jobs;

namespace UnityEngine.XR.ARFoundation.Samples
{
    public sealed class ChangeScene : MonoBehaviour
     
    {
        public void StartApp() {
            SceneManager.LoadScene(1);
        }

        public void RestartApp() {
            SceneManager.LoadScene(1);
        }

        public void EndApp() {
            SceneManager.LoadScene(2);
        }

        public void QuitApp() {
            Application.Quit();
        }
    }
}