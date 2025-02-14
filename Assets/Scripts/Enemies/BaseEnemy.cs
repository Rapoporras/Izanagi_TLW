using Health;
using SaveSystem;
using UnityEngine;
using Utils;
using Utils.CustomLogs;

public abstract class BaseEnemy : IdentifiableObject, ITemporalDataPersistence
{
    [Header("Reference to the player")]
    public GameObject player;
    
    [Header("Spawn Settings")]
    [SerializeField] private bool _alwaysRespawn;
    
    protected EntityHealth _entityHealth;
    protected bool _isEnemyDead;

    /// <summary>
    /// Make sure to call base.Awake() in override if you need Awake.
    /// </summary>
    protected virtual void Awake()
    {
        _entityHealth = GetComponent<EntityHealth>();
    }
    
    /// <summary>
    /// Make sure to call base.OnEnable() in override if you need OnEnable.
    /// </summary>
    protected virtual void OnEnable()
    {
        _entityHealth.ResetHealth();
        _isEnemyDead = false;
        _entityHealth.AddListenerDeathEvent(EnemyDie);
    }
    
    /// <summary>
    /// Make sure to call base.OnDisable() in override if you need OnDisable.
    /// </summary>
    protected virtual void OnDisable()
    {
        _entityHealth.RemoveListenerDeathEvent(EnemyDie);
    }

    public abstract void SetUpBehaviourTree();

    protected virtual void EnemyDie()
    {
        _isEnemyDead = true;
        ControllerVibration.Instance.TriggerInstantVibration(0.1f, 0.1f, 0.4f);
    }

    public void LoadTemporalData(TemporalDataSO temporalData)
    {
        LoadState(temporalData);
    }

    public void SaveTemporalData(TemporalDataSO temporalData)
    {
        SaveState(temporalData);
    }

    protected virtual void LoadState(TemporalDataSO temporalData)
    {
        _entityHealth.ResetHealth();
        if (_alwaysRespawn) return;
        
        if (temporalData.deadEnemies.Contains(id))
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    protected virtual void SaveState(TemporalDataSO temporalData)
    {
        bool containsId = temporalData.deadEnemies.Contains(id);
        if (_isEnemyDead && !containsId)
        {
            temporalData.deadEnemies.Add(id);
        }
        else if (!_isEnemyDead && containsId)
        {
            temporalData.deadEnemies.Remove(id);
        }
    }
}
