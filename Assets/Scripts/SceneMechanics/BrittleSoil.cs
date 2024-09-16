using UnityEngine;

namespace SceneMechanics
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class BrittleSoil : MonoBehaviour
    { 
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                DestroySoil();
            }
        }

        private void DestroySoil()
        {
            // add some effects here
            Destroy(gameObject);
        }
    }
}