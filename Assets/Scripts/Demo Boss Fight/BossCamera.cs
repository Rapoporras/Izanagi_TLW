using UnityEngine;

public class BossCamera : MonoBehaviour
{
    [SerializeField] private GameObject _bossCamera;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _bossCamera.SetActive(!_bossCamera.activeInHierarchy);
        }
    }
}
