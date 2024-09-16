using System;
using System.Collections;
using System.Collections.Generic;
using Health;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{

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
}
