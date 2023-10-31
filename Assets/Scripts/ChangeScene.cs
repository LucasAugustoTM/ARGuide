using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using Unity.XR.CoreUtils;
using Unity.Jobs;
using UnityEngine.Networking;
using System.Linq;


    public class ChangeScene: MonoBehaviour
     
    {
        public void Start() {
                Debug.Log("pog");
        }

        public void DownloadScene() {
            SceneManager.LoadScene(1);
        }

        public void DownloadLink() {
            Application.OpenURL("https://grande.ideia.pucrs.br/forms.php?form=1154");
            FileManager.Instance.flag_Download = true;
        }

        public void StartApp() {
            Debug.Log("flag: "+FileManager.Instance.flag_Download);
            if (FileManager.Instance.flag_Download == true) {
                FileManager.Instance.StartDownload();
                Debug.Log("flag2: "+FileManager.Instance.flag_Download);
                /*while (FileManager.Instance.flag_Download == true) {
                    Debug.Log("Downloading...");
                }
                SceneManager.LoadScene(2);*/
            }else{
                SceneManager.LoadScene(2);
            }
            //SceneManager.LoadScene(2);
        }

        public void ResetaFile() {
            FileManager.Instance.ResetaPadrao();
        }

        public void EndApp() {
            SceneManager.LoadScene(3);
        }

        public void QuitApp() {
            Application.Quit();
        }
    }