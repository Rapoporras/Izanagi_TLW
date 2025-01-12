using UnityEngine;

namespace Utils
{
    public class DestroyBehaviour : MonoBehaviour
    {
        public void DestroyObject()
        {
            Destroy(gameObject);
        }
    }
}