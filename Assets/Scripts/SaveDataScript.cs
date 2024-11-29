using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using UnityEngine.SceneManagement;



public class SaveDataScript : MonoBehaviour
{
    

    public TMP_InputField inputField;
    public Button saveButton;
    public Transform contentPanel;
    public GameObject itemPrefab;

    private int currentNumber = 0;
    private List<TextAndNumber> savedTexts = new List<TextAndNumber>();
    private string filePath;
    private string scorePath;

    private int currentScore = 0;

    void Start()
    {
        filePath = Application.persistentDataPath + "/savedData2.json";
        saveButton.onClick.AddListener(OnSaveButtonClick);
        LoadDataFromJSON();
        LoadBestScore(currentScore);
        scorePath = Application.persistentDataPath + "/savedBestScore.json";
    }

    // M�todo para manejar la acci�n del bot�n
    private void OnSaveButtonClick()
    {
        // Obt�n el texto ingresado
        string enteredText = inputField.text;
        
        // Agrega el texto a la lista junto con el n�mero actual
        savedTexts.Add(new TextAndNumber(enteredText, currentScore));

        // Crear un nuevo item en el ScrollView
        CreateItem(enteredText, currentScore);
        Debug.Log("Mejor puntuacion: "+currentScore);
        // Incrementar el n�mero para el siguiente item
        currentNumber = currentScore;

        // Guardar los datos en el archivo JSON
        SaveDataToJSON();

        // Limpiar el InputField
        inputField.text = "";
        SceneManager.LoadScene(1);
    }

    // M�todo para crear un item en el ScrollView
    public void CreateItem(string text, int number)
    {
        // Instancia el prefab que mostrar� el texto y el n�mero
        GameObject newItem = Instantiate(itemPrefab, contentPanel);

        // Asumiendo que el prefab tiene dos componentes de TextMeshPro: uno para el texto y otro para el n�mero
        TMP_Text[] texts = newItem.GetComponentsInChildren<TMP_Text>();

        // Asigna el texto ingresado y el n�mero
        texts[0].text = text;  // El primer TMP_Text es el que muestra el texto
        texts[1].text = number.ToString();  // El segundo TMP_Text es el que muestra el n�mero
    }

    // M�todo para guardar los datos en un archivo JSON
    public void SaveDataToJSON()
    {
        string json = JsonUtility.ToJson(new SaveData(savedTexts));
        File.WriteAllText(filePath, json);
        Debug.Log("Datos guardados en: " + filePath);
    }

    // M�todo para cargar los datos desde el archivo JSON
    private void LoadDataFromJSON()
    {
        if (File.Exists(filePath))
        {
            // Leer el contenido del archivo JSON
            string json = File.ReadAllText(filePath);

            // Convertir el JSON de vuelta a la lista de objetos
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            savedTexts = data.textAndNumbers;

            // Mostrar los datos cargados en el ScrollView
            foreach (var item in savedTexts)
            {
                CreateItem(item.text, item.number);
            }

            // Establecer el n�mero de la siguiente entrada
            currentNumber = savedTexts.Count;
        }
        else
        {
            // Si no existe el archivo, crear una lista vac�a
            savedTexts = new List<TextAndNumber>();
        }
    }
    private void LoadBestScore(int score)
    {
        // Verificar si el archivo existe
        if (File.Exists(scorePath))
        {
            // Leer el contenido del archivo JSON
            string json = File.ReadAllText(scorePath);

            // Deserializar el JSON a un objeto ScoreData
            ScoreData data = JsonUtility.FromJson<ScoreData>(json);
            currentScore = data.score;
            // Actualizar el texto del mejor puntaje

            Debug.Log("Mejor puntaje cargado: " + data.score);
        }
        else
        {
            // Si no existe, mostrar un valor predeterminado   
            Debug.Log("No se encontr� un puntaje guardado.");
        }
    }

    public void ClearJsonFile()
    {
        currentNumber = 0;  
        savedTexts.Clear(); // Borra todos los elementos de la lista

        // Guardar los datos vac�os en el archivo JSON
        SaveDataToJSON();
    }


    // Clase para almacenar los datos (texto y n�mero)
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

    // Clase para envolver la lista de datos y poder serializarla
    [System.Serializable]
    public class SaveData
    {
        public List<TextAndNumber> textAndNumbers;

        public SaveData(List<TextAndNumber> textAndNumbers)
        {
            this.textAndNumbers = textAndNumbers;
        }
    }
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
