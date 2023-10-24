using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;

namespace UnityEngine.XR.ARFoundation.Samples
{
public class Controller : MonoBehaviour
{

    public PrefabImagePairManager reseta;

    [SerializeField]
    [Tooltip("Reference Image Library")]
    XRReferenceImageLibrary m_ImageLibrary;

    public XRReferenceImageLibrary imageLibrary
        {
            get => m_ImageLibrary;
            set => m_ImageLibrary = value;
        }

    ARTrackedImageManager manager;
    // Start is called before the first frame update
    void Awake()
    {
        manager = GetComponent<ARTrackedImageManager>();
        if (manager) {
            Destroy(manager);
        }
        manager = gameObject.AddComponent<ARTrackedImageManager>();
        manager.referenceLibrary = imageLibrary;
        manager.enabled = true;
        //reseta.Comeca();
    }

}
}
