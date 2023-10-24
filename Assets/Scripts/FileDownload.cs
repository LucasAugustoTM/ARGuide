using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class FileDownload : MonoBehaviour
{
    string url = "https://grande.ideia.pucrs.br/getPrefab.php";
    public string saveFileName = "file.txt";
    private string fullPath;

    IEnumerator Start()
    {
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
        Debug.Log("Conteudo do arquivo: " + fileContent);
        SaveFile(fullPath, fileContent);
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
