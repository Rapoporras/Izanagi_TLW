using Health;
using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    [SerializeField] private int _damage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(other.name);
            if (other.transform.root.TryGetComponent(out PlayerHealth playerHealth))
            {
                int xDirection = (int) Mathf.Sign(other.transform.position.x - transform.position.x);
                playerHealth.Damage(_damage, xDirection);
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
