using System;
using System.Text;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

namespace UnityEngine.XR.ARFoundation.Samples
{
    public class FileDownload : MonoBehaviour
    {
        string url = "https://grande.ideia.pucrs.br/getPrefab.php";
        public string saveFileName = "file.txt";
        private string fullPath;

        public List<List<string>> ordem;

        IEnumerator Start()
        {
            ordem = new List<List<string>>();

            // Cria caminho completo para o arquivo texto
            fullPath = Path.Combine(Application.persistentDataPath, saveFileName);

            // Carrega arquivo.
            LoadFile(fullPath);

            // Baixa e salva arquivo texto no dispositivo
            yield return StartCoroutine(GetText(OnDownladCompleted));

            LoadFile(fullPath);
        }

        // Baixa arquivo da url. Recebe como argumento um metodo para tratar o conteudo do arquivo apos o download
        IEnumerator GetText(Action<string> onDonwloadCompleted)
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
        }

        // exibe conteudo do arquivo e o salva localmente
        public void OnDownladCompleted(string fileContent)
        {
            Build(fileContent);

            SaveFile(fullPath, fileContent);
        }

        void Build(string fileContent) {

            string[] linesInFile = fileContent.Split('\n');
            linesInFile = linesInFile.SkipLast(1).ToArray(); 

            foreach (string line in linesInFile) {
                //Debug.Log("split: "+line);
                List<string> virgulas = new List<string>(line.Split(','));
                ordem.Add(virgulas);
                /*foreach(var l in virgulas) {
                    Debug.Log("virgulas: "+l);
                    Debug.Log("Tipo virgulas: "+ virgulas.GetType());
                    Debug.Log("Tipo cada: "+ l.GetType());
                }*/
            }

            var dynamic = GetComponent<DynamicPrefab>();
            dynamic.Comeca(ordem);
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
    }
}