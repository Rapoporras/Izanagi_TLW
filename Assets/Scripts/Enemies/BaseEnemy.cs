using System.Collections;
using Health;
using SaveSystem;
using UnityEngine;
using Utils;

public class BaseEnemy : IdentifiableObject, ITemporalDataPersistence
{
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
        _entityHealth.AddListenerDeathEvent(EnemyDie);
    }
    
    /// <summary>
    /// Make sure to call base.OnDisable() in override if you need OnDisable.
    /// </summary>
    protected virtual void OnDisable()
    {
        _entityHealth.RemoveListenerDeathEvent(EnemyDie);
    }

    protected void EnemyDie()
    {
        _isEnemyDead = true;
        StartCoroutine(PlayDeathAnimation());
    }

    private IEnumerator PlayDeathAnimation()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }

    public void LoadTemporalData(TemporalDataSO temporalData)
    {
        temporalData.EnemiesStatus.TryGetValue(id, out _isEnemyDead);
        if (_isEnemyDead)
        {
            gameObject.SetActive(false);
        }
    }

    public void SaveTemporalData(TemporalDataSO temporalData)
    {
        if (temporalData.EnemiesStatus.ContainsKey(id))
        {
            temporalData.EnemiesStatus.Remove(id);
        }
        temporalData.EnemiesStatus.Add(id, _isEnemyDead);
    }
}
