using UnityEngine;
using System;

public class EventBus
{
    private static EventBus theInstance;
    public static EventBus Instance
    {
        get
        {
            if (theInstance == null)
                theInstance = new EventBus();
            return theInstance;
        }
    }

    // Existing event
    public event Action<Vector3, Damage, Hittable> OnDamage;

    // New relic-relevant events
    public event Action<Damage, Hittable> OnPlayerDamaged;
    public event Action OnEnemyKilled;
    public event Action OnSpellCast;
    public event Action<Vector2> OnPlayerMoveInput; // movement vector from input

    public void DoDamage(Vector3 where, Damage dmg, Hittable target)
    {
        OnDamage?.Invoke(where, dmg, target);

        // Player took damage
        if (target != null && target.team == Hittable.Team.PLAYER)
        {
            OnPlayerDamaged?.Invoke(dmg, target);
        }
    }

    public void EnemyKilled()
    {
        OnEnemyKilled?.Invoke();
    }

    public void SpellCast()
    {
        OnSpellCast?.Invoke();
    }

    public void PlayerMoveInput(Vector2 movement)
    {
        OnPlayerMoveInput?.Invoke(movement);
    }

    public void clear()
    {
        OnDamage = null;
        OnPlayerDamaged = null;
        OnEnemyKilled = null;
        OnSpellCast = null;
        OnPlayerMoveInput = null;
    }
}