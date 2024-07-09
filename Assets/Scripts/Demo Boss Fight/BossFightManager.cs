using System.Collections.Generic;
using UnityEngine;

public class BossFightManager : MonoBehaviour
{
    [SerializeField] private BossController _bossController;
    [SerializeField] private List<GameObject> _exitWalls;

    private bool _hasFightStarted;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !_hasFightStarted)
        {
            _hasFightStarted = true;
            _bossController.StartFight(other.transform);
            foreach (var exitWall in _exitWalls)
            {
                exitWall.SetActive(true);
            }
        }
    }

    private void OnEnable()
    {
        _bossController.OnFightFinished += FinishFight;
    }
    
    private void OnDisable()
    {
        _bossController.OnFightFinished -= FinishFight;
    }

    private void FinishFight()
    {
        foreach (var exitWall in _exitWalls)
        {
            exitWall.SetActive(false);
        }
    }
}