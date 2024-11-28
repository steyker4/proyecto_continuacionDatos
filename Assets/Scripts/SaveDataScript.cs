using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;



public class SaveDataScript : MonoBehaviour
{
    public TMP_InputField inputField;  // Asigna el InputField TMP desde el Inspector
    public Button saveButton;          // Asigna el bot�n para guardar el texto
    public Transform contentPanel;     // El panel del ScrollView donde se agregar�n los elementos
    public GameObject itemPrefab;      // Prefab para mostrar el texto y el n�mero

    private int currentNumber = 0;     // El n�mero que acompa�ar� el texto (inicia en 0)

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

    // M�todo para manejar la acci�n del bot�n
    private void OnSaveButtonClick()
    {
        // Obt�n el texto ingresado
        string enteredText = inputField.text;

        // Agrega el texto a la lista junto con el n�mero actual
        savedTexts.Add(new TextAndNumber(enteredText, currentNumber));

        // Crear un nuevo item en el ScrollView
        CreateItem(enteredText, currentNumber);

        // Incrementar el n�mero para el siguiente item
        currentNumber++;

        // Guardar los datos en el archivo JSON
        SaveDataToJSON();

        // Limpiar el InputField
        inputField.text = "";
    }

    // M�todo para crear un item en el ScrollView
    private void CreateItem(string text, int number)
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
    private void SaveDataToJSON()
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
}