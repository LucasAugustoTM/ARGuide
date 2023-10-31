using System;
using System.Text;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
using UnityEngine.SceneManagement;


    public class FileManager: MonoBehaviour
    {
        string url = "https://grande.ideia.pucrs.br/getPrefab.php";
        private string saveFileName = "file.txt";
        private string fullPath;

        public bool flag_Download;

        public List<List<string>> ordem;

        private List<List<string>> defaultOrder;

        public TextAsset defaulText;

        public static FileManager Instance;
        
        void Awake() {

            // start of new code
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            // end of new code

            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            Scene currentScene = SceneManager.GetActiveScene();
            defaultOrder = new List<List<string>>();
            flag_Download = false;

            Build(defaulText.text,defaultOrder);
            ordem = new List<List<string>>(defaultOrder);

            fullPath = Path.Combine(Application.persistentDataPath, saveFileName);

            printOrdem(defaultOrder);
            Debug.Log("***************************");
            printOrdem(ordem);
        }


        public void StartDownload()
        {
            // Carrega arquivo.
            LoadFile(fullPath);

            // Baixa e salva arquivo texto no dispositivo
             StartCoroutine(GetText(OnDownladCompleted));
        }

        // Baixa arquivo da url. Recebe como argumento um metodo para tratar o conteudo do arquivo apos o download
        public IEnumerator GetText(Action<string> onDonwloadCompleted)
        {
            UnityWebRequest www = new UnityWebRequest(url);
            www.downloadHandler = new DownloadHandlerBuffer();

            yield return www.SendWebRequest();

            // Se ocorrer um erro no download
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else // Download concluido
            {
                // Chama metodo para lidar com conteudo do arquivo baixado
                onDonwloadCompleted?.Invoke(www.downloadHandler.text);
            }
            SceneManager.LoadScene(2);
        }

        // exibe conteudo do arquivo e o salva localmente
        public void OnDownladCompleted(string fileContent)
        {
            ordem.Clear();
            Build(fileContent,ordem);
            SaveFile(fullPath, fileContent);
            flag_Download = false;
        }

        void Build(string fileContent, List<List<string>> order) {

            string[] linesInFile = fileContent.Split('\n');

            foreach (string line in linesInFile) {
                Debug.Log("split: "+line);
                List<string> virgulas = new List<string>(line.Split(','));
                order.Add(virgulas);
                /*foreach(var l in virgulas) {
                    Debug.Log("virgulas: "+l);
                    Debug.Log("Tipo virgulas: "+ virgulas.GetType());
                    Debug.Log("Tipo cada: "+ l.GetType());
                }*/
            }
        }

        // Salva o arquivo
        public void SaveFile(string filePath, string fileCotent)
        {
            File.WriteAllText(filePath, fileCotent);

            Debug.Log($"Arquivo salvo em {filePath}");
        }

        // Carrega arquivo localmente
        public void LoadFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Debug.Log($"Arquivo {filePath} nao encontrado!");
                return;
            }

            string fileContent = File.ReadAllText(filePath);
            Debug.Log($"Arquivo carregado: {fileContent}");
        }

        public void ResetaPadrao() {

            printOrdem(defaultOrder);
            Debug.Log("wwwwwwwwwwwwwwwwwww");
            printOrdem(ordem);
            ordem = new List<List<string>>(defaultOrder);
            Debug.Log("wwwwwwwwwwwwwwwwwww");
            printOrdem(ordem);
        }

        public void uga() {
            Debug.Log("uga buga");
        }

        void printOrdem(List<List<string>> order) {
            foreach(var line in order) {
                foreach(var l in line) {
                    Debug.Log("elemento: "+l);
                }
            }
        }
    }
