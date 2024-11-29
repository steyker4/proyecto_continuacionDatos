using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class LimitScrollView : MonoBehaviour
{
    public RectTransform content; // Asigna el RectTransform del contenido
    public float itemHeight = 100f; // Altura de cada elemento
    public int itemCount = 10; // Número de elementos en la lista

    void UpdateContentSize()
    {
        // Ajusta el tamaño del RectTransform del contenido
        content.sizeDelta = new Vector2(content.sizeDelta.x, itemHeight * itemCount);
    }
}
