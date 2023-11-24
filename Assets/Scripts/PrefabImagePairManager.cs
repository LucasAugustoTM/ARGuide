using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;
using System.Collections;

namespace UnityEngine.XR.ARFoundation.Samples
{
    /// <summary>
    /// This component listens for images detected by the <c>XRImageTrackingSubsystem</c>
    /// and overlays some prefabs on top of the detected image.
    /// </summary>
    [RequireComponent(typeof(ARTrackedImageManager))]
    public class PrefabImagePairManager : MonoBehaviour//, ISerializationCallbackReceiver
    {
        /// <summary>
        /// Used to associate an `XRReferenceImage` with a Prefab by using the `XRReferenceImage`'s guid as a unique identifier for a particular reference image.
        /// </summary>
        [Serializable]
        struct NamedPrefab
        {
            // System.Guid isn't serializable, so we store the Guid as a string. At runtime, this is converted back to a System.Guid
            public string imageGuid;
            public GameObject imagePrefab;

            public NamedPrefab(Guid guid, GameObject prefab)
            {
                imageGuid = guid.ToString();
                imagePrefab = prefab;
            }
        }


        Dictionary<Guid, GameObject> m_PrefabsDictionary;
        Dictionary<Guid, GameObject> m_Instantiated = new Dictionary<Guid, GameObject>();
        ARTrackedImageManager m_TrackedImageManager;

        [SerializeField]
        [Tooltip("Reference Image Library")]
        XRReferenceImageLibrary m_ImageLibrary;

        /// <summary>
        /// Get the <c>XRReferenceImageLibrary</c>
        /// </summary>
        public XRReferenceImageLibrary imageLibrary
        {
            get => m_ImageLibrary;
            set => m_ImageLibrary = value;
        }

        private bool first = true;
        void Awake()
        {
            
            m_PrefabsDictionary = new Dictionary<Guid, GameObject>();
            m_TrackedImageManager = GetComponent<ARTrackedImageManager>();
        }

        void OnEnable()
        {
            m_TrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
        }

        void OnDisable()
        {
            m_TrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
        }

         void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
        {

            //Debug.Log("Trackable count no ontracked inicio: " +m_TrackedImageManager.trackables.count); 

            foreach (var trackedImage in eventArgs.added)
            {
                DadosImagem(trackedImage);
            }

            //Debug.Log("Trackable count no ontracked fim: "+m_TrackedImageManager.trackables.count);     

            if((m_TrackedImageManager.trackables.count >= imageLibrary.count) & first==true) {
                foreach(var trackedImage in m_TrackedImageManager.trackables) {
                    DadosImagem(trackedImage);
                }
            }

            // Disable instantiated prefabs that are no longer being actively tracked
            foreach (var trackedImage in eventArgs.updated) { 
                m_Instantiated[trackedImage.referenceImage.guid] 
                    .SetActive(trackedImage.trackingState == TrackingState.Tracking); 
            }
        }

        void DadosImagem(ARTrackedImage trackedImage) {
            first = false;
            // Give the initial image a reasonable default scale
            var minLocalScalar = (Mathf.Min(trackedImage.size.x, trackedImage.size.y) / 2) * 1.15f;
            trackedImage.transform.localScale = new Vector3(minLocalScalar, minLocalScalar, minLocalScalar);
            AssignPrefab(trackedImage);
        }


        public void InitPrefabForReferenceImage(XRReferenceImage referenceImage, GameObject alternativePrefab)
        {
            Debug.Log("Trackable count no init: " +m_TrackedImageManager.trackables.count); 
            m_PrefabsDictionary.Add(referenceImage.guid, alternativePrefab);
            Debug.Log("---------------------------");    
        }

       
        
        void AssignPrefab(ARTrackedImage trackedImage)
        {
            if (m_PrefabsDictionary.TryGetValue(trackedImage.referenceImage.guid, out var prefab)) {
                m_Instantiated[trackedImage.referenceImage.guid] = Instantiate(prefab, trackedImage.transform);

                /*foreach(var k in m_Instantiated.Keys) {
                    Debug.Log("Instantiated dps de mudar imagem: "+m_Instantiated[k]);
                }
                Debug.Log("$$$$$$$$$$$$$$$$$$$$$$$$$$$");*/      
            }    
        }
        
        public GameObject GetPrefabForReferenceImage(XRReferenceImage referenceImage)
            => m_PrefabsDictionary.TryGetValue(referenceImage.guid, out var prefab) ? prefab : null;
        
        public void SetPrefabForReferenceImage(XRReferenceImage referenceImage, GameObject alternativePrefab)
        {
            m_PrefabsDictionary[referenceImage.guid] = alternativePrefab;
            if (m_Instantiated.TryGetValue(referenceImage.guid, out var instantiatedPrefab))
            {
                m_Instantiated[referenceImage.guid] = Instantiate(alternativePrefab, instantiatedPrefab.transform.parent);
                Destroy(instantiatedPrefab);
                /*foreach(var k in m_Instantiated.Keys) {
                    Debug.Log("Instantiated dps de Set: "+m_Instantiated[k]);
                }      
                Debug.Log("######################################");*/
            }
        }

       /* IEnumerator Espera(ARTrackedImage trackedImage) {
            //foreach(var trackedImage in m_TrackedImageManager.trackables) {
            //        m_Instantiated[trackedImage.referenceImage.guid].transform.position = m_PrefabsDictionary[trackedImage.referenceImage.guid].transform.position;
            //        //trackedImage.trackingstate.None;
            //    }
            yield return new WaitForSecondsRealtime(5.0f);
            m_Instantiated[trackedImage.referenceImage.guid] 
                    .SetActive(trackedImage.trackingState == TrackingState.Tracking); 
            //m_TrackedImageManager.SetTrackablesActive(true);
        } 
        public void Limpa() {
            Debug.Log("Limpou!");
            m_TrackedImageManager.SetTrackablesActive(false);
            StartCoroutine(Espera());
            Debug.Log("Limpou 2!");
        } */
    }
}