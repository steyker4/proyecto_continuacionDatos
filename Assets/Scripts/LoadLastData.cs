using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using static SaveDataScript;
using TMPro;

public class LoadLastData : MonoBehaviour
{
    private string filePath;

    public TMP_Text displayText; // Asigna el TMP_Text desde el Inspector

    void Start()
    {
        // Ruta del archivo JSON
        filePath = Application.persistentDataPath + "/savedData2.json";

        // Cargar y mostrar el primer y último elemento
        LoadAndShowFirstAndLastElement();
    }

    private void LoadAndShowFirstAndLastElement()
    {
        if (File.Exists(filePath))
        {
            // Leer el contenido del archivo JSON
            string json = File.ReadAllText(filePath);

            // Convertir el JSON a una lista de objetos
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            // Verificar que la lista no esté vacía
            if (data.textAndNumbers != null && data.textAndNumbers.Count > 0)
            {
                // Obtener el primer y último elemento
                TextAndNumber firstElement = data.textAndNumbers[0];
                TextAndNumber lastElement = data.textAndNumbers[data.textAndNumbers.Count - 1];

                // Mostrar los textos en el TMP_Text
                displayText.text = lastElement.text;
            }
            else
            {
                Debug.Log("La lista está vacía.");
            }
        }
        else
        {
            Debug.LogError("El archivo JSON no existe: " + filePath);
        }
    }

}
[System.Serializable]
public class TextAndNumber
{
    public string text;
    public int number;

    public TextAndNumber(string text, int number)
    {
        this.text = text;
        this.number = number;
    }
}

[System.Serializable]
public class SaveData
{
    public List<TextAndNumber> textAndNumbers = new List<TextAndNumber>();

    public SaveData(List<TextAndNumber> textAndNumbers)
    {
        this.textAndNumbers = textAndNumbers;
    }
}
