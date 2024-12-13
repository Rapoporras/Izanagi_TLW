using UnityEngine;

namespace Bosses
{
    public class SeiryuFightManager : MonoBehaviour
    {
        [SerializeField] private SeiryuController _seiryuController;
        [SerializeField] private GameObject[] _exitWalls;

        private bool _hasFightStarted;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && !_hasFightStarted)
            {
                _hasFightStarted = true;
                _seiryuController.StartFight(other.transform);
                foreach (var exitWall in _exitWalls)
                {
                    exitWall.SetActive(true);
                }
            }
        }
        
        private void OnEnable()
        {
            _seiryuController.OnFightFinished += FinishFight;
        }
        
        private void OnDisable()
        {
            _seiryuController.OnFightFinished -= FinishFight;
        }

        private void FinishFight()
        {
            foreach (var exitWall in _exitWalls)
            {
                exitWall.SetActive(false);
            }
        }
    }
}