using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class NubeDesvanecer : MonoBehaviour
{
    public float duracionDesvanecimiento = 2f; // Duración de cada ciclo de aparición/desaparición
    private Image imagenNube; // Referencia al componente Image

    void Start()
    {
        // Obtén el componente Image de la nube
        imagenNube = GetComponent<Image>();

        // Inicia el efecto de desvanecimiento en bucle
        DesvanecerNube();
    }

    void DesvanecerNube()
    {
        // Asegúrate de que la imagen esté visible al inicio
        imagenNube.color = new Color(imagenNube.color.r, imagenNube.color.g, imagenNube.color.b, 1);

        // Anima la opacidad a 0 (desaparece), luego a 1 (aparece) en bucle
        imagenNube.DOFade(0.8f, duracionDesvanecimiento)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo); // Repite indefinidamente

        // Aplica una oscilación en la escala (leve "estiramiento" horizontal y vertical)
        transform.DOScaleY(1, duracionDesvanecimiento / 2)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);

        transform.DOScaleX(1, duracionDesvanecimiento / 2)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);

    }
}