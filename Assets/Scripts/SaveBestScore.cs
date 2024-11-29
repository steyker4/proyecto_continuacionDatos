using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.IO;

public class SaveBestScore : MonoBehaviour
{
    public TMP_Text bestScoreText; // Referencia al TMP_Text para mostrar el mejor puntaje

    private string filePath; // Ruta del archivo JSON

    void Start()
    {
        // Definir la ruta del archivo JSON
        filePath = Application.persistentDataPath + "/savedBestScore.json";

        // Cargar el mejor puntaje y mostrarlo en la UI
        LoadBestScore();
    }

    private void LoadBestScore()
    {
        // Verificar si el archivo existe
        if (File.Exists(filePath))
        {
            // Leer el contenido del archivo JSON
            string json = File.ReadAllText(filePath);

            // Deserializar el JSON a un objeto ScoreData
            ScoreData data = JsonUtility.FromJson<ScoreData>(json);

            // Actualizar el texto del mejor puntaje
            bestScoreText.text = "Best Score: " + data.score;

            Debug.Log("Mejor puntaje cargado: " + data.score);
        }
        else
        {
            // Si no existe, mostrar un valor predeterminado
            bestScoreText.text = "Best Score: 0";
            Debug.Log("No se encontró un puntaje guardado.");
        }
    }

    // Clase para deserializar el puntaje
    [System.Serializable]
    public class ScoreData
    {
        public int score;

        public ScoreData(int score)
        {
            this.score = score;
        }
    }
}