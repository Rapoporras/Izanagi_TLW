using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Health;
public class WaterDamage : MonoBehaviour
{
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {

    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.gameObject.TryGetComponent(out PlayerHealth playerHealth))
            {
                int xDirection = (int)Mathf.Sign(other.transform.position.x - transform.position.x);
                playerHealth.Damage(100000, xDirection);
            }
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (other.gameObject.TryGetComponent(out EntityHealth entityHealth))
            {
                entityHealth.Damage(100000, false);
            }
        }
    }

}
