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
        int passo = 0;

        public TextAsset ordem;
        List<List<string>> m_Ordem;

        /*[SerializeField]
        GameObject m_AlternativePrefab;

        public GameObject alternativePrefab
        {
            get => m_AlternativePrefab;
            set => m_AlternativePrefab = value;
        }*/

        enum State
        {
            OriginalPrefab,
            ChangeToOriginalPrefab,
            AlternativePrefab,
            ChangeToAlternativePrefab,
            NextPrefab,
            LastPrefab,
            MudaPrefab,
            Error
        }

        State m_State;

        string m_ErrorMessage = "";

        void ChangePrefab() {
            var manager = GetComponent<PrefabImagePairManager>();
            var library = manager.imageLibrary;

            if(passo < m_Ordem.Count) {
                var yesPrefab = (GameObject)Resources.Load(m_Ordem[passo][1]); 
                Debug.Log(yesPrefab);
                
                var noPrefab = (GameObject)Resources.Load(m_Ordem[passo][2].Trim()); 
                Debug.Log(noPrefab);

                Debug.Log("Tipo ordem: "+m_Ordem.GetType());
                for(var i=0; i<m_Ordem.Count; i++){ 
                    Debug.Log("primero: "+m_Ordem[i][1]+"]");
                    Debug.Log("segundo: "+m_Ordem[i][2]+"]"); }
                foreach (var l in m_Ordem) {
                     Debug.Log("Tipo l: "+l.GetType());
                     foreach (var l2 in l) {
                        Debug.Log("l2: "+l2);
                        Debug.Log("Tipo l2: "+l2.GetType());}
                }
                Debug.Log("yesprefab: "+yesPrefab.GetType());
                try {
                Debug.Log("noprefab: "+noPrefab.GetType()); }
                catch (Exception e){Debug.Log("pinto "+noPrefab);}
                Debug.Log("passo: "+m_Ordem[passo][2].GetType());

                foreach (var referenceImage in library) {
                    Debug.Log(referenceImage.name);
                    if(String.Equals(m_Ordem[passo][0], referenceImage.name)) {
                        Debug.Log("Nome certo!");
                        if(passo==0) {
                            manager.InitPrefabForReferenceImage(referenceImage, yesPrefab);
                            Debug.Log("Inicializou o certo!"); }
                        else
                            manager.SetPrefabForReferenceImage(referenceImage, yesPrefab);
                    }else{
                        Debug.Log("Nome errado!");
                        if(passo==0) {
                            manager.InitPrefabForReferenceImage(referenceImage, noPrefab);
                            Debug.Log("Inicializou o errado!"); }
                        else
                            manager.SetPrefabForReferenceImage(referenceImage, noPrefab);
                    }    
                }
            }
        }

        void Start() {

            m_Ordem = new List<List<string>>();
            string[] linesInFile = ordem.text.Split('\n');
            foreach (string line in linesInFile)
            {
                List<string> virgulas = new List<string>(line.Split(','));
                m_Ordem.Add(virgulas);
                foreach(var l in virgulas) {
                    Debug.Log("virgulas: "+l);
                    Debug.Log("Tipo virgulas: "+ virgulas.GetType());
                    Debug.Log("Tipo cada: "+ l.GetType());
                }
            }

            //string p = m_Ordem[2][1];
            //Debug.Log(p);
    
            /*var manager = GetComponent<PrefabImagePairManager>();
            var library = manager.imageLibrary;
            foreach (var referenceImage in library) {   
                    var newPrefab = (GameObject)Resources.Load("Cubes");                 
                    manager.InitPrefabForReferenceImage(referenceImage, newPrefab);
            }*/
            ChangePrefab();
            
        }

        void OnGUI()
        {
            var fontSize = 50;
            GUI.skin.button.fontSize = fontSize;
            GUI.skin.label.fontSize = fontSize;

            float margin = 100;

            GUILayout.BeginHorizontal("box");

            //GUILayout.BeginArea(new Rect(margin, margin, Screen.width - margin * 2, Screen.height - margin * 2));
            //GUILayout.BeginArea(new Rect(margin, margin, Screen.width - margin * 2, Screen.height - margin * 2));

            switch (m_State)
            {
                case State.OriginalPrefab:
                { //if (GUILayout.Button($"Alternative Prefab for {GetComponent<PrefabImagePairManager>().imageLibrary[0].name}"))
                        if (GUILayout.Button($"Próximo"))
                        {
                            passo +=1;
                            m_State = State.MudaPrefab;
                        }
                        else if (GUILayout.Button($"Anterior")) {
                            passo -=1;
                            m_State = State.MudaPrefab;
                        }

                    break;
                    
                }

                case State.Error:
                {
                    GUILayout.Label(m_ErrorMessage);
                    break;
                }
            }
            GUILayout.EndHorizontal();
            //GUILayout.EndArea();
        
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
                case State.MudaPrefab:
                {

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

                    ChangePrefab();
                    m_State = State.OriginalPrefab;   

                    break;
                }

            }
        }
    }
}
