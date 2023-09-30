using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

public class ImageRecognitionExample : MonoBehaviour
{
   private ARTrackedImageManager _arTrackedImageManager;

   private void Awake() {

        _arTrackedImageManager = FindObjectOfType<ARTrackedImageManager>();

   }

    private void OnEnable() {

        _arTrackedImageManager.trackedImagesChanged += OnImagesChanged;
    }

    private void OnDisable() {

        _arTrackedImageManager.trackedImagesChanged -= OnImagesChanged;
    }
    
    public void OnImagesChanged(ARTrackedImagesChangedEventArgs args) {

        foreach (var trackedImage in args.added) {

            Debug.Log(trackedImage.name);
        }
    }

}
