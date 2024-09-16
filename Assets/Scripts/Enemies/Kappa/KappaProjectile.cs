using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KappaProjectile : MonoBehaviour
{
    private float _speed;
    private float _lifetime;
    private Rigidbody2D _rb;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.velocity = new Vector2(_speed, 0);
        Destroy(gameObject, _lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    public float Speed
    {
        get => _speed;
        set => _speed = value;
    }

    public float Lifetime
    {
        get => _lifetime;
        set => _lifetime = value;
    }
}
