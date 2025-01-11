using System.Collections;
using System.Collections.Generic;
using CustomAttributes;
using UnityEngine;

namespace SceneMechanics
{
    public class GhostTown : MonoBehaviour
    {
        [Header("Activation Time Range")]
        [SerializeField, MinMax(0, 5)] private Vector2 _ghostsActivationTimeRange;
        [Space(10)]
        [SerializeField] private List<Ghost> _ghosts;

        private Coroutine _activateGhostsCoroutine; 

        private IEnumerator ActivateGhosts()
        {
            foreach (var ghost in _ghosts)
            {
                ghost.IsPlayerInTown = true;
                float waitTime = Random.Range(_ghostsActivationTimeRange.x, _ghostsActivationTimeRange.y);
                yield return new WaitForSeconds(waitTime);
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                if (_activateGhostsCoroutine != null)
                    StopCoroutine(_activateGhostsCoroutine);
                StartCoroutine(ActivateGhosts());
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                foreach (var ghost in _ghosts)
                {
                    ghost.IsPlayerInTown = false;
                }
            }
        }
    }
}