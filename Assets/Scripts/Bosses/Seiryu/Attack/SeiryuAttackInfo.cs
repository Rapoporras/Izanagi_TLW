namespace Bosses
{
    [System.Serializable]
    public struct SeiryuAttackInfo
    {
        public AttackState state;
        public AttackType type;
        public ClawSide side;
    }

    public enum AttackState
    {
        StartAttack,
        FinishAttack,
        Waiting
    }

    public enum AttackType
    {
        None,
        Fist,
        Sweep,
        Transition
    }

    public enum ClawSide
    {
        None,
        Left,
        Right
    }
}