using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;
public class RamaVientoSimple : MonoBehaviour
{
    public float duracionOscilacion = 2f;   // Duración de cada ciclo de oscilación
    public float anguloOscilacion = 5f;     // Ángulo máximo de oscilación
    public float escalaOscilacion = 0.05f;  // Magnitud de la oscilación de escala
    public float movimientoOscilacion = 5f; // Magnitud del movimiento horizontal

    void Start()
    {
        OscilarRama();
    }

    void OscilarRama()
    {
        // Aplica una oscilación en la rotación (balanceo) en el eje Z
        transform.DORotate(new Vector3(0, 0, anguloOscilacion), duracionOscilacion)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);

        // Aplica una oscilación en la escala (leve "estiramiento" horizontal y vertical)
        transform.DOScaleY(1 + escalaOscilacion, duracionOscilacion / 2)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);

        transform.DOScaleX(1 + escalaOscilacion, duracionOscilacion / 2)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);

        // Aplica una oscilación en la posición X para simular un leve vaivén
        transform.DOLocalMoveX(transform.localPosition.x + movimientoOscilacion, duracionOscilacion)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }
}