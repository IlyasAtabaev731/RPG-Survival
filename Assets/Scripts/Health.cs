using System;

public class Health
{
    private int _maxHitPoints;
    private int _hitPoints;

    public event Action OnHealthChange;
    public event Action OnDeath;

    public int MaxHp => _maxHitPoints;
    
    public int Hp
    {
        get => _hitPoints;
        private set
        {
            _hitPoints = value;
            if (Hp <= 0) OnDeath?.Invoke();
            if (Hp > _maxHitPoints) Hp = _maxHitPoints;
            OnHealthChange?.Invoke();
        }
    }

    public void Discard(int value)
    {
        Hp -= value;
    }

    public void Heal(int value)
    {
        Hp += value;
    } 

    public Health(int maxHitPoints)
    {
        _maxHitPoints = maxHitPoints;
        _hitPoints = maxHitPoints;
    }
}
