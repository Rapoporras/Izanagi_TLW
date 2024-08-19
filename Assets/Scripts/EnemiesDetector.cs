using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesDetector : MonoBehaviour
{
    public LayerMask enemyLayer;
    
    private Camera _mainCamera;

    private List<string> _enemiesFound = new List<string>();

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            LookForEnemies();
        }
    }

    private void LookForEnemies()
    {
        float height = _mainCamera.orthographicSize * 2f;
        float width = height * _mainCamera.aspect;

        Vector3 center = _mainCamera.transform.position;
        Vector2 pointA = new Vector2(
            center.x - (width / 2f),
            center.y - (height / 2f));
        Vector2 pointB = new Vector2(
            center.x + (width / 2f),
            center.y + (height / 2f));

        _enemiesFound.Clear();
        
        Collider2D[] enemies = Physics2D.OverlapAreaAll(pointA, pointB, enemyLayer);
        foreach (var enemy in enemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                _enemiesFound.Add(enemy.transform.parent.name);
            }
        }

        Debug.Log(String.Join(",", _enemiesFound));
    }
}