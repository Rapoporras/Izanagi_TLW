using System;
using UnityEngine;

namespace Enemies
{
    public class PlayerDetectorEnemy : MonoBehaviour
    {
        public virtual bool IsPlayerDetected()
        {
            return true;
        }
        
        public virtual void ChangeDirection(float sign)
        {
            
        }
    }
}
