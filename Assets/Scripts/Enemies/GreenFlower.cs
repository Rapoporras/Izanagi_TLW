using Health;
using UnityEngine;

public class GreenFlower : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.gameObject.TryGetComponent(out PlayerHealth playerHealth))
            {
                playerHealth.FlowerDamage(1);
            }
        }
    }
}