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
        Attacking,
        FinishAttack,
        Recovering,
        Waiting
    }

    public enum AttackType
    {
        None,
        Fist,
        Sweep,
        WaterGun,
        Transition
    }

    public enum ClawSide
    {
        None,
        Left,
        Right
    }
}