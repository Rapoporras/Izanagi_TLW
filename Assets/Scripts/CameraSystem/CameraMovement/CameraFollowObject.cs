using System.Collections;
using UnityEngine;

namespace CameraSystem
{
    public class CameraFollowObject : MonoBehaviour
    {
        [Header("Flip Rotation Stats")]
        [SerializeField] private float _flipYRotationTime = 0.5f;

        private Coroutine _turnCoroutine;

        private Rigidbody2D _playerRigidbody2D;
        private bool _isFacingRight;

        public void Initialize(Rigidbody2D playerRigidbody2D, bool isFacingRight)
        {
            _playerRigidbody2D = playerRigidbody2D;
            _isFacingRight = isFacingRight;
        }

        private void FixedUpdate()
        {
            transform.position = _playerRigidbody2D.position;
        }

        public void CallTurn()
        {
            if (_turnCoroutine != null)
                StopCoroutine(_turnCoroutine);
            _turnCoroutine = StartCoroutine(FlipYLerp());
        }

        private IEnumerator FlipYLerp()
        {
            float startRotation = transform.localEulerAngles.y;
            float endRotationAmount = DetermineEndRotation();

            float elapsedTime = 0f;
            while (elapsedTime < _flipYRotationTime)
            {
                elapsedTime += Time.deltaTime;
                
                float yRotation = Mathf.Lerp(startRotation, endRotationAmount, elapsedTime / _flipYRotationTime);
                transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
                
                yield return null;
            }
        }

        private float DetermineEndRotation()
        {
            _isFacingRight = !_isFacingRight;
            return _isFacingRight ? 0f : 180f;
        }
    }
}