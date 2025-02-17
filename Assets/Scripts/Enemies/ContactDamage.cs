using Health;
using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    [Header("Settings")]
    public bool isActive = true;
    [Space(5)]
    [SerializeField] private int _damage;
    [SerializeField] private bool _canDamageEnemies = false;
    [SerializeField] private LayerMask _hurtboxLayer;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActive) return;

        if ((1 << other.gameObject.layer) != _hurtboxLayer) return;

        if (other.CompareTag("Player"))
        {
            if (other.transform.parent.TryGetComponent(out PlayerHealth playerHealth))
            {
                int xDirection = (int)Mathf.Sign(other.transform.position.x - transform.position.x);
                playerHealth.Damage(_damage, xDirection);
                ControllerVibration.Instance.TriggerInstantVibration(0.2f, 0.3f, 0.5f);
            }
        }

        if (_canDamageEnemies && other.CompareTag("Enemy") && other.transform.parent != transform)
        {
            if (other.transform.parent.TryGetComponent(out EntityHealth enemyHealth))
            {
                enemyHealth.Damage(_damage, true);
            }
        }
    }

    private void OnValidate()
    {
        Transform hitboxTransform = transform.Find("Hitbox");
        if (!hitboxTransform)
        {
            GameObject hitbox = new GameObject("Hitbox");
            hitbox.transform.parent = transform;
            hitbox.transform.localPosition = Vector3.zero;

            hitbox.layer = LayerMask.NameToLayer("Hitbox");
            BoxCollider2D hitboxCollider = hitbox.AddComponent<BoxCollider2D>();
            hitboxCollider.isTrigger = true;
        }
    }
}
