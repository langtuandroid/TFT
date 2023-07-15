using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIResolutionChanger : MonoBehaviour
{
    #region Constants

    // Reference resolution (1920:1080)
    private const float MAIN_RESOLUTION = 1920f / 1080f;

    #endregion

    #region Private variables

    // Lista que contiene las dimensiones a las que va a cambiar el objeto dentro del canvas
    // (width y height)
    private List<Vector2> _dimensions = new List<Vector2> {
        new Vector2(360f, 202f),
        new Vector2(320f, 180f)
    };

    #endregion

    #region Unity methods

    private void Awake()
    {
        // Cambiamos la escala
        ChangeScale();
    }

    #endregion

    #region Private methods

    private void ChangeScale()
    {
        // Cogemos el canvas scaler del padre
        CanvasScaler parent = transform.parent.GetComponent<CanvasScaler>();
        // Y el rectTransform del componente
        RectTransform rect = GetComponent<RectTransform>();

        // Si entra en la parte horizontal (corte arriba y abajo)
        if (Camera.main.aspect <= MAIN_RESOLUTION)
        {
            // Dejamos que el canvas haga match con el ancho
            parent.matchWidthOrHeight = 0f;
            // Y ponemos las primeras dimensiones
            rect.sizeDelta = _dimensions[0];
        }
        // En caso de que recorte por los laterales izda y derecha
        else
        {
            // Dejamos que haga match con el alto 
            parent.matchWidthOrHeight = 1f;
            // Y ponemos las segundas dimensiones
            rect.sizeDelta = _dimensions[1];
        }
    }

    #endregion

}
