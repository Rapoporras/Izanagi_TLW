using UnityEngine;

namespace Bosses
{
    public class SeiryuHead : MonoBehaviour, ISeiryuAttackStateHandler
    {
        [Header("Settings")]
        [SerializeField] private float _maxRotation = 35f;
        
        private Transform _player;
        private Animator _animator;

        private int _defaultAnimHash;
        private int _angerAnimHash;
        private int _animationIddleSelect;

        private float maxAngle = 20f;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _defaultAnimHash = Animator.StringToHash("default");
            _angerAnimHash = Animator.StringToHash("anger");
            _animationIddleSelect = Animator.StringToHash("animationSelect");
        }

        public void Initialize(Transform player)
        {
            _player = player;
        }

        private void Update()
        {
            LookAtPlayer();
        }

        private void LookAtPlayer()
        {
            if (!_player) return;
            
            Vector3 dir = _player.position - transform.position;
            float angle = Vector3.SignedAngle(dir, Vector3.down, Vector3.back);
            angle = Mathf.Clamp(angle, -_maxRotation, _maxRotation);
            
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        //Llamado desde evento de fin de animación de personaje Iddle
        private void UpdateAnimationIddleSelect(int anim)
        {
            _animator.SetFloat(_animationIddleSelect, Random.Range(0f, 1f));
        }

        public void OnAttackStateChange(SeiryuAttackInfo info)
        {
            switch (info.state)
            {
                case AttackState.StartAttack:
                    _animator.SetTrigger(_angerAnimHash);
                    break;
                case AttackState.FinishAttack:
                    break;
                case AttackState.Waiting:
                    _animator.SetTrigger(_defaultAnimHash);
                    break;
            }
        }
    }
}