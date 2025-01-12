using System.Collections;
using PlayerController;
using UnityEngine;
using UnityEngine.UI;

namespace Bosses
{
    public class SeiryuFightManager : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private SeiryuController _seiryuController;
        [SerializeField] private GameObject _seiryuNPC;
        [SerializeField] private GameObject[] _exitWalls;

        [Header("Transition")]
        [SerializeField] private Image _fadeImage;
        [SerializeField] private float _fadeDuration = 1f;
        [Space(10)]
        [SerializeField] private Transform _playerPosAfterFade;
        [SerializeField] private bool _playerLookingRightAfterFade;

        private Coroutine _finishFightCoroutine;
        private GameObject _player;
        
        private bool _hasFightStarted;

        private void Awake()
        {
            _seiryuNPC.gameObject.SetActive(false); // just in case
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && !_hasFightStarted)
            {
                _player = other.gameObject;
                _hasFightStarted = true;
                _seiryuController.StartFight(other.transform);
                foreach (var exitWall in _exitWalls)
                {
                    exitWall.SetActive(true);
                }
            }
        }
        
        private void OnEnable()
        {
            _seiryuController.OnFightFinished += FinishFight;
        }
        
        private void OnDisable()
        {
            _seiryuController.OnFightFinished -= FinishFight;
        }

        private void FinishFight()
        {
            if (_finishFightCoroutine != null)
                StopCoroutine(_finishFightCoroutine);
            StartCoroutine(_FinishFight());
        }

        private IEnumerator _FinishFight()
        {
            yield return Fade(1f);
            
            // replace dragon
            _seiryuController.gameObject.SetActive(false);
            _seiryuNPC.gameObject.SetActive(true);
            
            // set player position and facing direction
            _player.GetComponent<PlayerMovement>().SetPosition(_playerPosAfterFade.position, _playerLookingRightAfterFade);
            
            yield return Fade(0f);
        }

        private IEnumerator Fade(float targetAlpha)
        {
            float startAlpha = _fadeImage.color.a;
            float elapsed = 0;

            while (elapsed < _fadeDuration)
            {
                elapsed += Time.deltaTime;
                float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / _fadeDuration);
                SetImageAlpha(newAlpha);
                yield return null;
            }

            SetImageAlpha(targetAlpha);
        }
        
        private void SetImageAlpha(float alpha)
        {
            if (!_fadeImage) return;

            Color color = _fadeImage.color;
            color.a = alpha;
            _fadeImage.color = color;
        }
    }
}