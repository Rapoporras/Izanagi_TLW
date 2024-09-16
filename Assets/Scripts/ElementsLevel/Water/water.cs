using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Health;
public class water : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.transform.root.TryGetComponent(out PlayerHealth playerHealth))
            {
                int xDirection = (int)Mathf.Sign(other.transform.position.x - transform.position.x);
                playerHealth.Damage(10000, xDirection);
            }
        }
    }
}
