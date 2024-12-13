using System;
using UnityEngine;

namespace Bosses
{
    public class SeiryuClaw : MonoBehaviour
    {
        enum ClawState
        {
            Attacking, Recovering, Waiting
        }
        
        [Header("Phase 1")]
        [SerializeField] private Transform _firstPhaseHitPoint;

        public event Action OnClawReady;

        public void FistPunch()
        {
            
        }
    }
}