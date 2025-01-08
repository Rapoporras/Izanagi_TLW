using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    [SerializeField, Min(1)] private int _hitsToBreak;

    [SerializeField, ReadOnly] private int _hitsReceived;
    
    public void ApplyHit()
    {
        //TODO: animations and sound effects
        
        _hitsReceived++;
        if (_hitsReceived >= _hitsToBreak)
        {
            Destroy(gameObject);
        }
    }
}