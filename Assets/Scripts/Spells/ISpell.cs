using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpell
{
    // Identity / UI
    string GetName();
    int GetIcon();
    // Core combat stats
    int GetManaCost();
    int GetDamage();
    float GetCooldown();
    float GetProjectileSpeed();
    string GetProjectileTrajectory();
    float GetProjectileScale();


    // Runtime state
    float LastCast { get; set; }
    SpellCaster Owner { get; }
    Hittable.Team Team { get; }
    // Behavior
    bool IsReady();
    IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team);
    IEnumerable<Vector3> GetShotDirections(Vector3 baseDirection);
    int GetExtraCastCount();
    float GetExtraCastDelay();

}