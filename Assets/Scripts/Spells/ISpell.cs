using UnityEngine;
using System.Collections;

public interface ISpell
{
    // Identity / UI
    string GetName();
    int GetIcon();
    // Core combat stats
    int GetManaCost();
    int GetDamage();
    float GetCooldown();

    // Runtime state
    float LastCast { get; set; }
    SpellCaster Owner { get; }
    Hittable.Team Team { get; }
    // Behavior
    bool IsReady();
    IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team);
   
}