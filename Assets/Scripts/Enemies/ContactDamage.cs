using PlayerController;
using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    [SerializeField] private int _damage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.transform.root.TryGetComponent(out PlayerMovement playerMovement))
            {
                int xDirection = (int) Mathf.Sign(other.transform.position.x - transform.position.x);
                playerMovement.ApplyDamageKnockBack(xDirection);
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
