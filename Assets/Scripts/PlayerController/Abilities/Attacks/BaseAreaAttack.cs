using UnityEditor;
using UnityEngine;

namespace PlayerController.Abilities
{
    public class BaseAreaAttack : MonoBehaviour
    {
        [SerializeField] protected LayerMask _hurtboxLayer;
        [SerializeField] protected Transform _pointA;
        [SerializeField] protected Transform _pointB;
        
#if UNITY_EDITOR
        #region GIZMOS
        private void OnDrawGizmos()
        {
            if (IsSelectedOrChild())
            {
                Vector3 center = GetAreaCenter();
                Vector3 size = new Vector3(
                    Mathf.Abs(_pointA.position.x - _pointB.position.x),
                    Mathf.Abs(_pointA.position.y - _pointB.position.y),
                    0f);

                Gizmos.color = new Color(0, 1, 0, 0.2f);
                Gizmos.DrawCube(center, size);

                Gizmos.color = Color.red;
                Gizmos.DrawSphere(_pointA.position, 0.1f);
                Gizmos.DrawSphere(_pointB.position, 0.1f);
            }
        }

        private bool IsSelectedOrChild()
        {
            if (!Selection.activeGameObject)
                return false;

            if (Selection.activeGameObject == gameObject)
                return true;

            if (Selection.activeGameObject.transform.IsChildOf(gameObject.transform))
                return true;
            
            return false;
        }

        private Vector3 GetAreaCenter()
        {
            Vector3 posA = _pointA.position;
            Vector3 posB = _pointB.position;
            
            Vector3 center = Vector3.zero;
            center.x = posA.x > posB.x ? (posA.x - posB.x) / 2 + posB.x : (posB.x - posA.x) / 2 + posA.x;
            center.y = posA.y > posB.y ? (posA.y - posB.y) / 2 + posB.y : (posB.y - posA.y) / 2 + posA.y;
            
            return center;
        }
        #endregion
#endif
    }
}