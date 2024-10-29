using System.Collections.Generic;
using GlobalVariables;
using UnityEngine;

namespace UI.HUD
{
    public class PlayerPotionsUI : MonoBehaviour
    {
        [SerializeField] private IntReference _potionsAvailable;
        [SerializeField] private IntReference _maxPotionsAmount;

        [Space(10)]
        [SerializeField] private List<GameObject> _dangos;

        private void Start()
        {
            UpdatePotions(_potionsAvailable.Value);
        }

        private void OnEnable()
        {
            _potionsAvailable.AddListener(UpdatePotions);
        }

        private void OnDisable()
        {
            _potionsAvailable.RemoveListener(UpdatePotions);
        }

        private void UpdatePotions(int amount)
        {
            for (int i = 0; i < _dangos.Count; i++)
            {
                if (i <= _potionsAvailable.Value - 1)
                {
                    _dangos[i].SetActive(true);
                }
                else
                {
                    _dangos[i].SetActive(false);
                }
            }
        }
    }
}
