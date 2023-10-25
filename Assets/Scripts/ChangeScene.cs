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


    public sealed class ChangeScene : MonoBehaviour
     
    {
        public void DownloadScene() {
            SceneManager.LoadScene(1);
        }

        public void DownloadLink() {
            Application.OpenURL("https://grande.ideia.pucrs.br/forms.php?form=1154");
        }

        public void StartApp() {
            SceneManager.LoadScene(2);
        }

        public void EndApp() {
            SceneManager.LoadScene(3);
        }

        public void QuitApp() {
            Application.Quit();
        }
    }