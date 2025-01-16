using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FarolEye : MonoBehaviour
{

    private Transform player;
    public float detectionRange = 5f;
    public float trackSpeed = 0.1f;
    public float rightLimit = -0.1f;
    public float leftLimit = 0.1f;

    private bool _playerDetected;

    private void Update()
    {
        if (_playerDetected && player != null)
        {
            float xDifference = player.position.x - transform.position.x;
            float playerDirection = Mathf.Sign(xDifference);
            
            float newEyePosition = transform.localPosition.x + playerDirection;
            newEyePosition = Mathf.Clamp(newEyePosition, rightLimit, leftLimit);
            
            Vector3 newPosition = new Vector3(newEyePosition, transform.localPosition.y, transform.localPosition.z);
            transform.localPosition = Vector3.Lerp(transform.localPosition, newPosition, trackSpeed);
        }
        else
        {
            Vector3 newPosition = new Vector3(0, transform.localPosition.y, transform.localPosition.z);
            transform.localPosition = Vector3.Lerp(transform.localPosition, newPosition, trackSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            _playerDetected = true;
            player = other.transform;
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerDetected = false;
            player = null;
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
