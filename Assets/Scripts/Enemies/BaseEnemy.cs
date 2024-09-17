using System.Collections;
using Health;
using SaveSystem;
using UnityEngine;

public class BaseEnemy : MonoBehaviour, IDataPersistence
{
    [SerializeField, ReadOnly] private string id;
    [Space(10)]
    
    [Header("Spawn Settings")]
    [SerializeField] private bool _alwaysRespawn;
    
    protected EntityHealth _entityHealth;
    protected bool _isEnemyDead;
    
    protected virtual void OnDisable()
    {
        Debug.Log("script was disabled");
        _entityHealth.RemoveListenerDeathEvent(() => EnemyDie());
    }

    protected void EnemyDie()
    {
        Debug.Log("Enemy slain");
        _isEnemyDead = true;
        StartCoroutine(PlayDeathAnimation());
    }

    private IEnumerator PlayDeathAnimation()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
    
    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
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
