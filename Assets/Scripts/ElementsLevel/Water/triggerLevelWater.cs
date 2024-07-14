using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerLevelWater : MonoBehaviour
{
    [SerializeField] private GameObject waterBoss;
    SpriteRenderer spriteRenderer;


    private void Start()
    {

        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            spriteRenderer.color = new Color(1, 1, 1, 0.5f);


            BoxCollider2D boxCollider = waterBoss.GetComponent<BoxCollider2D>();
            BuoyancyEffector2D buoyancyEffector2D = waterBoss.GetComponent<BuoyancyEffector2D>();
            if (boxCollider != null)
            {
                // Cambia el tamaño del Box Collider
                boxCollider.size = new Vector2(boxCollider.size.x, 1.58f);
                // Cambia la posición del Box Collider
                buoyancyEffector2D.surfaceLevel = 0.81f;

                Debug.Log("Box Collider modificado correctamente");
                waterBoss.transform.localScale = new Vector3(waterBoss.transform.localScale.x, 7, 1);
                // Asegúrate de que el GameObject tiene un BoxCollider2D
            }
            else
            {
                Debug.LogError("El GameObject no tiene un componente BoxCollider2D");
            }
        }
    }
}
