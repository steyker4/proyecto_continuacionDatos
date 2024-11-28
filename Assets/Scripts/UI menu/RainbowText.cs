using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RainbowText : MonoBehaviour
{

    public TextMeshProUGUI text; // Referencia al componente TextMeshPro
    public float speed = 2f;     // Velocidad del arcoíris
    public float frequency = 5f; // Frecuencia del cambio de color

    // Start is called before the first frame update
    void Start()
    {
        if (text == null)
        {
            text = GetComponent<TextMeshProUGUI>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (text == null) return;

        // Obtenemos el texto actual
        string currentText = text.text;
        int length = currentText.Length;

        // Creamos un array para los colores
        TMP_TextInfo textInfo = text.textInfo;
        text.ForceMeshUpdate();
        Color32[] newVertexColors;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            if (!textInfo.characterInfo[i].isVisible) continue;

            int vertexIndex = textInfo.characterInfo[i].vertexIndex;
            newVertexColors = textInfo.meshInfo[textInfo.characterInfo[i].materialReferenceIndex].colors32;

            // Cambiamos el color usando el arcoíris
            Color color = Color.HSVToRGB((Time.time * speed + i * frequency) % 1f, 1f, 1f);
            newVertexColors[vertexIndex + 0] = color;
            newVertexColors[vertexIndex + 1] = color;
            newVertexColors[vertexIndex + 2] = color;
            newVertexColors[vertexIndex + 3] = color;
        }

        // Actualizamos los datos del texto
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            textInfo.meshInfo[i].mesh.colors32 = textInfo.meshInfo[i].colors32;
            text.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
        }
    }
}
