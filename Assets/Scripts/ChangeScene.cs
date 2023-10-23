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
    //[DefaultExecutionOrder(ARUpdateOrder.k_TrackedImageManager)]
    //[RequireComponent(typeof(ARTrackedImageManager))]
   // [RequireComponent(typeof(XROrigin))]
    //[HelpURL(typeof(ARTrackedImageManager))]
    public sealed class ChangeScene : MonoBehaviour
     
    {
        //public PrefabImagePairManager reseta;
    
        public void StartApp() {
            SceneManager.LoadScene(1);
        }

        public void RestartApp() {
            Debug.Log("aa");
            //subsystem.imageLibrary = null;
            SceneManager.LoadScene(1);
            //var reseta = GetComponent<PrefabImagePairManager>();
            Debug.Log("xxxxx");
            //reseta.Limpa();
            Debug.Log("bbbbbb");
        }

        public void EndApp() {
            SceneManager.LoadScene(2);
        }

        public void QuitApp() {
            Application.Quit();
        }
    }
}