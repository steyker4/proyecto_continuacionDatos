using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;



public class SaveDataScript : MonoBehaviour
{
    public TMP_InputField inputField;  // Asigna el InputField TMP desde el Inspector
    public Button saveButton;          // Asigna el botón para guardar el texto
    public Transform contentPanel;     // El panel del ScrollView donde se agregarán los elementos
    public GameObject itemPrefab;      // Prefab para mostrar el texto y el número

    private int currentNumber = 0;     // El número que acompañará el texto (inicia en 0)

    // Lista para almacenar los datos
    private List<TextAndNumber> savedTexts = new List<TextAndNumber>();

    private string filePath;           // Ruta del archivo JSON

    void Start()
    {
        filePath = Application.persistentDataPath + "/savedData.json";  // Ruta para guardar el archivo JSON

        saveButton.onClick.AddListener(OnSaveButtonClick);

        // Cargar los datos guardados al iniciar
        LoadDataFromJSON();
    }

    // Método para manejar la acción del botón
    private void OnSaveButtonClick()
    {
        // Obtén el texto ingresado
        string enteredText = inputField.text;

        // Agrega el texto a la lista junto con el número actual
        savedTexts.Add(new TextAndNumber(enteredText, currentNumber));

        // Crear un nuevo item en el ScrollView
        CreateItem(enteredText, currentNumber);

        // Incrementar el número para el siguiente item
        currentNumber++;

        // Guardar los datos en el archivo JSON
        SaveDataToJSON();

        // Limpiar el InputField
        inputField.text = "";
    }

    // Método para crear un item en el ScrollView
    private void CreateItem(string text, int number)
    {
        // Instancia el prefab que mostrará el texto y el número
        GameObject newItem = Instantiate(itemPrefab, contentPanel);

        // Asumiendo que el prefab tiene dos componentes de TextMeshPro: uno para el texto y otro para el número
        TMP_Text[] texts = newItem.GetComponentsInChildren<TMP_Text>();

        // Asigna el texto ingresado y el número
        texts[0].text = text;  // El primer TMP_Text es el que muestra el texto
        texts[1].text = number.ToString();  // El segundo TMP_Text es el que muestra el número
    }

    // Método para guardar los datos en un archivo JSON
    private void SaveDataToJSON()
    {
        string json = JsonUtility.ToJson(new SaveData(savedTexts));
        File.WriteAllText(filePath, json);
        Debug.Log("Datos guardados en: " + filePath);
    }

    // Método para cargar los datos desde el archivo JSON
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

            // Establecer el número de la siguiente entrada
            currentNumber = savedTexts.Count;
        }
        else
        {
            // Si no existe el archivo, crear una lista vacía
            savedTexts = new List<TextAndNumber>();
        }
    }

    // Clase para almacenar los datos (texto y número)
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
}