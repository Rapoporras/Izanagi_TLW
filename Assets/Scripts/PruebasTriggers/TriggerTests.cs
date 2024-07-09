using UnityEngine;

public class TriggerTests : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"{other.name} -> {other.transform.parent.name}");
    }
}
