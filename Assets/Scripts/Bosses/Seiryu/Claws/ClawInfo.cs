namespace Bosses
{
    public struct ClawInfo
    {
        public ClawState state;
    }

    public enum ClawState
    {
        StartAttack,
        Attacking,
        FinishAttack,
        Recovering,
        Waiting
    }
}