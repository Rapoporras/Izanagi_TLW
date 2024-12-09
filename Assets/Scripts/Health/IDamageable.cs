namespace Health
{
    public interface IDamageable
    {
        void Damage(int amount, bool screenShake);
    }
}