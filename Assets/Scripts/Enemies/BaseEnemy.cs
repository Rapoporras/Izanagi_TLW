using System.Collections;
using Health;
using SaveSystem;
using UnityEngine;
using Utils;

public class BaseEnemy : IdentifiableObject, IDataPersistence
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

    public void LoadData(GameData data)
    {
        if (_alwaysRespawn) return;
        
        data.deadEnemies.TryGetValue(id, out _isEnemyDead);
        if (_isEnemyDead)
        {
            gameObject.SetActive(false);
        }
    }

    public void SaveData(ref GameData data)
    {
        if (data.deadEnemies.ContainsKey(id))
        {
            data.deadEnemies.Remove(id);
        }
        
        if (!_alwaysRespawn)
            data.deadEnemies.Add(id, _isEnemyDead);
    }
}
