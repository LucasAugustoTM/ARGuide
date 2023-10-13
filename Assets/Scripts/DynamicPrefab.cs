using System;
using System.Text;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.IO;
using System.Collections.Generic;


namespace UnityEngine.XR.ARFoundation.Samples
{
    /// <summary>
    /// Change the prefab for the first image in library at runtime.
    /// </summary>
    [RequireComponent(typeof(ARTrackedImageManager))]
    public class DynamicPrefab : MonoBehaviour
    {
        GameObject m_OriginalPrefab;
        bool first = true;

        [SerializeField]
        GameObject m_AlternativePrefab;

        public TextAsset ordem;
        List<List<string>> m_Ordem;

        public GameObject alternativePrefab
        {
            get => m_AlternativePrefab;
            set => m_AlternativePrefab = value;
        }

        enum State
        {
            OriginalPrefab,
            ChangeToOriginalPrefab,
            AlternativePrefab,
            ChangeToAlternativePrefab,
            Error
        }

        State m_State;

        string m_ErrorMessage = "";

        void Start() {

            m_Ordem = new List<List<string>>();
            string[] linesInFile = ordem.text.Split('\n');
            foreach (string line in linesInFile)
            {
                List<string> virgulas = new List<string>(line.Split(','));
                m_Ordem.Add(virgulas);
            }

            //string p = m_Ordem[2][1];
            //Debug.Log(p);
    
            var manager = GetComponent<PrefabImagePairManager>();
            var library = manager.imageLibrary;
            foreach (var referenceImage in library) {   
                    var newPrefab = (GameObject)Resources.Load("Cubes");                 
                    manager.InitPrefabForReferenceImage(referenceImage, newPrefab);
            }
        }

        void OnGUI()
        {
            var fontSize = 50;
            GUI.skin.button.fontSize = fontSize;
            GUI.skin.label.fontSize = fontSize;

            float margin = 100;

            GUILayout.BeginArea(new Rect(margin, margin, Screen.width - margin * 2, Screen.height - margin * 2));

            switch (m_State)
            {
                case State.OriginalPrefab:
                { //if (GUILayout.Button($"Alternative Prefab for {GetComponent<PrefabImagePairManager>().imageLibrary[0].name}"))
                        if (GUILayout.Button($"Alternative Prefab for all images"))
                        {
                            m_State = State.ChangeToAlternativePrefab;
                        }

                    break;
                    
                }
                case State.AlternativePrefab:
                {
                    if (GUILayout.Button($"Original Prefab for all images"))
                    {
                        m_State = State.ChangeToOriginalPrefab;
                    }

                    break;
                }
                case State.Error:
                {
                    GUILayout.Label(m_ErrorMessage);
                    break;
                }
            }
            GUILayout.EndArea();
        }

        void SetError(string errorMessage)
        {
            m_State = State.Error;
            m_ErrorMessage = $"Error: {errorMessage}";
        }

        void Update()
        {
            switch (m_State)
            {
                case State.ChangeToAlternativePrefab:
                {
                    if (!alternativePrefab)
                    {
                        SetError("No alternative prefab is given.");
                        break;
                    }

                    var manager = GetComponent<PrefabImagePairManager>();
                    if (!manager)
                    {
                        SetError($"No {nameof(PrefabImagePairManager)} available.");
                        break;
                    }

                    var library = manager.imageLibrary;
                    if (!library)
                    {
                        SetError($"No image library available.");
                        break;
                    }

                    foreach (var referenceImage in library) { 
                        if (first==true) {                                                     
                            m_OriginalPrefab = manager.GetPrefabForReferenceImage(referenceImage);
                        }    

                        var newPrefab = (GameObject)Resources.Load("qr");                 
                        manager.SetPrefabForReferenceImage(referenceImage, newPrefab);
                        m_State = State.AlternativePrefab;                  
                    }
                    first = false;
                    break;
                }

                case State.ChangeToOriginalPrefab:
                {
                    if (!m_OriginalPrefab)
                    {
                        SetError("No original prefab is given.");
                        break;
                    }

                    var manager = GetComponent<PrefabImagePairManager>();
                    if (!manager)
                    {
                        SetError($"No {nameof(PrefabImagePairManager)} available.");
                        break;
                    }

                    var library = manager.imageLibrary;
                    if (!library)
                    {
                        SetError($"No image library available.");
                        break;
                    }
                    foreach (var referenceImage in library) { 
                        manager.SetPrefabForReferenceImage(referenceImage, m_OriginalPrefab);
                        m_State = State.OriginalPrefab;
                    }    
                    break;
                }
            }
        }
    }
}
