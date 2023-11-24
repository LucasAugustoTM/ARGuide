using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Networking;

namespace UnityEngine.XR.ARFoundation.Samples
{
    /// <summary>
    /// Change the prefab for the first image in library at runtime.
    /// </summary>
    [RequireComponent(typeof(ARTrackedImageManager))]
    public class DynamicPrefab : MonoBehaviour
    {
        public FadeInOut m_Fade;

        public ChangeScene scene;
       
        int passo = 0;
        bool first = true;

        public GameObject score;

        public List<List<string>> m_Ordem;

        enum State
        {
            OriginalPrefab,
            MudaPrefab,
            Error
        }

        State m_State;

        string m_ErrorMessage = "";

        void ChangePrefab() {
            var manager = GetComponent<PrefabImagePairManager>();
            var library = manager.imageLibrary;

            Debug.Log("passo no change: "+passo);
            if((passo >= 0) & (passo < m_Ordem.Count)) {
                var yesPrefab = (GameObject)Resources.Load(m_Ordem[passo][1]); 
                Debug.Log(yesPrefab);
                
                var noPrefab = (GameObject)Resources.Load(m_Ordem[passo][2].Trim()); 
                Debug.Log(noPrefab);

                foreach (var referenceImage in library) {
                    if(first==true) {
                        if(String.Equals(m_Ordem[passo][0], referenceImage.name)) {
                            manager.InitPrefabForReferenceImage(referenceImage, yesPrefab);
                            //Debug.Log("Inicializou o certo!"); 
                        }else{
                            manager.InitPrefabForReferenceImage(referenceImage, noPrefab);
                            //Debug.Log("Inicializou o errado!"); 
                        }  
                    }else{
                        if(String.Equals(m_Ordem[passo][0], referenceImage.name)) {
                            //Debug.Log("Nome certo!");
                            manager.SetPrefabForReferenceImage(referenceImage, yesPrefab);
                        }else{
                            //Debug.Log("Nome errado!");
                            manager.SetPrefabForReferenceImage(referenceImage, noPrefab);
                        } 
                    }                     
                }
            }
        }
        public void Start() {

            m_Ordem =  FileManager.Instance.ordem;
            ChangePrefab();
            first = false;
        }

        

        public void nextStep() {
            passo +=1;
            if(passo==m_Ordem.Count)
                scene.EndApp();
            else    
                m_State = State.MudaPrefab;
            var scoreColor = score.GetComponent<Image>();
            StartCoroutine(Blink(scoreColor,Color.green));   
        }

        public void lastStep() {
            if (passo!=0) {
                passo -=1;
                m_State = State.MudaPrefab;
            } else {
                StartCoroutine(m_Fade.initFade());        
            }
            var scoreColor = score.GetComponent<Image>();
            StartCoroutine(Blink(scoreColor,Color.yellow));
        }

        IEnumerator Blink(Image image, Color c) {
            image.color = c;
            yield return new WaitForSeconds(0.3f);
            image.color = Color.white;
        }

        void SetError(string errorMessage)
        {
            m_State = State.Error;
            m_ErrorMessage = $"Error: {errorMessage}";
        }

        void Update()
        {
            var scoreText = score.transform.GetChild(0).gameObject.GetComponent<TMPro.TMP_Text>();
            scoreText.text = "Passo: " + (passo+1).ToString();
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


/*Debug.Log("Tipo ordem: "+m_Ordem.GetType());
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
                Debug.Log("passo jklkljk: "+m_Ordem[passo][2].GetType()); */