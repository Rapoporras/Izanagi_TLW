using Health;
using UnityEngine;

public class BossClaw : MonoBehaviour
{
    public enum ClawState
    {
        Attacking, Recovering, Waiting
    }
    
    [Header("Attack Settings")]
    [SerializeField] private AnimationCurve _attackCurve;
    [SerializeField] private float _attackSpeed;
    
    [Header("Recover Settings")]
    [SerializeField] private AnimationCurve _recoverCurve;
    [SerializeField] private float _recoverSpeed;

    [Space(10)]
    [SerializeField] private Color _deactiveColor;
    
    [HideInInspector] public BossController controller;
    
    public bool IsActive { get; private set; }
    public ClawState State { get; private set; }

    private Vector3 _originPosition;
    private Vector3 _targetPosition;
    private float _current;
    
    private EntityHealth _health;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _health = GetComponent<EntityHealth>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        State = ClawState.Waiting;
        IsActive = true;

        _originPosition = transform.position;
        _current = 0f;
        
        _health.AddListenerDeathEvent(Deactivate);
    }

    private void Update()
    {
        if (!IsActive) return;
        
        switch (State)
        {
            case ClawState.Waiting:
                break;
            
            case ClawState.Attacking:
                _current = Mathf.MoveTowards(_current, 1f, _attackSpeed * Time.deltaTime);
                transform.position = Vector3.Lerp(_originPosition, _targetPosition, _attackCurve.Evaluate(_current));
                if (transform.position == _targetPosition)
                    State = ClawState.Recovering;
                break;
            
            case ClawState.Recovering:
                _current = Mathf.MoveTowards(_current, 0f, _recoverSpeed * Time.deltaTime);
                transform.position = Vector3.Lerp(_originPosition, _targetPosition, _recoverCurve.Evaluate(_current));
                if (transform.position == _originPosition)
                {
                    State = ClawState.Waiting;
                    controller.NextAttack();
                }
                break;
        }
    }

    public void Attack(Vector3 position)
    {
        if (!IsActive) return;
        
        _targetPosition = position;
        State = ClawState.Attacking;
    }

    private void Deactivate()
    {
        _spriteRenderer.color = _deactiveColor;
        IsActive = false;

        Transform hitbox = transform.Find("Hitbox");
        Destroy(hitbox.gameObject);
        
        if (State != ClawState.Waiting)
            controller.NextAttack();
    }
}