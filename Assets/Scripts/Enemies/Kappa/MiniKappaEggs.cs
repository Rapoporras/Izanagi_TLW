using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniKappaEggs : MonoBehaviour
{

    [Header("Spawn parameters")]
    [SerializeField] private GameObject entityToSpawn;
    [SerializeField] private int amountToSpawn;
    [SerializeField] private float spawnDelay;

    [Header("Detection area Parameters")]
    [SerializeField] private Transform areaBase;
    [SerializeField] private float areaWidth;
    [SerializeField] private float areaHeight;

    private Animator _animator;
    private bool _isOpened;
    private static readonly int Open = Animator.StringToHash("open");

    void Start()
    {
        _animator = GetComponent<Animator>();
        _isOpened = false;
    }
    
    void Update()
    {
        if (_isOpened) return;
        
        Collider2D[] entities = Physics2D.OverlapBoxAll(
            new Vector2(areaBase.position.x, areaBase.position.y + areaHeight / 2),
            new Vector2(areaWidth, areaHeight), 0f);
        
        foreach (var entity in entities)
        {
            if (entity.CompareTag("Player"))
            {
                StartCoroutine(SpawnEnemies(amountToSpawn, spawnDelay));
                _animator.SetTrigger(Open);
                _isOpened = true;
                return;
            }
        }
    }

    IEnumerator SpawnEnemies(int amountToSpawn, float spawnDelay)
    {
        for (int i = 0; i < amountToSpawn; i++)
        {
            Instantiate(entityToSpawn, transform.position, Quaternion.Euler(Vector3.zero));
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(new Vector2(areaBase.position.x, areaBase.position.y + areaHeight/2), new Vector2(areaWidth, areaHeight));
    }
}
