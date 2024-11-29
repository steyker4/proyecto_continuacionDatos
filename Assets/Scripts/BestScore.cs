using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static SaveDataScript;
using System.IO;
using UnityEngine.UI;

public class BestScore : MonoBehaviour
{
    public Text currentScoreText; // Referencia al TMP_Text que muestra el puntaje actual
    public TMP_Text bestScoreText;    // Referencia al TMP_Text que muestra el mejor puntaje

    private int currentScore = 0;     // Puntaje actual
    private int bestScore = 0;        // Mejor puntaje guardado
    private string filePath;          // Ruta del archivo JSON

    void Start()
    {
        // Definir la ruta del archivo JSON
        filePath = Application.persistentDataPath + "/savedBestScore.json";

        // Intentar cargar el mejor puntaje al iniciar
        LoadBestScore();

        // Mostrar el mejor puntaje en la UI
        UpdateBestScoreUI();
    }

    public void UpdateScore(int points)
    {
        // Incrementar el puntaje actual
        currentScore += points;

        // Actualizar la UI del puntaje actual
        currentScoreText.text = "Score: " + currentScore;

        // Actualizar el mejor puntaje si el puntaje actual es mayor
        if (currentScore > bestScore)
        {
            bestScore = currentScore;
            SaveBestScore();
            UpdateBestScoreUI();
        }
    }

    private void SaveBestScore()
    {
        // Crear un objeto ScoreData con el mejor puntaje
        ScoreData data = new ScoreData(bestScore);

        // Convertir el objeto a formato JSON
        string json = JsonUtility.ToJson(data);

        // Guardar el JSON en el archivo
        File.WriteAllText(filePath, json);

        Debug.Log("Mejor puntaje guardado: " + bestScore);
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

            // Actualizar el mejor puntaje
            bestScore = data.score;

            Debug.Log("Mejor puntaje cargado: " + bestScore);
        }
        else
        {
            Debug.Log("No se encontró un mejor puntaje guardado.");
        }
    }

    private void UpdateBestScoreUI()
    {
        // Actualizar el TMP_Text del mejor puntaje
        bestScoreText.text = "Best Score: " + bestScore;
    }

    public void ResetScore()
    {
        // Reiniciar el puntaje actual
        currentScore = 0;

        // Actualizar la UI del puntaje actual
        currentScoreText.text = "Score: " + currentScore;

        Debug.Log("Puntaje actual reiniciado.");
    }

    // Clase para guardar el puntaje en el JSON
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