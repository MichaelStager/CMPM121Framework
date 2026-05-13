using UnityEngine;

public class SpellCastContext
{
    public Vector3 where;
    public Vector3 target;
    public Hittable.Team team;

    public int damage = 100;
    public int manaCost = 10;
    public float cooldown = 0.75f;
    public float projectileSpeed = 15f;

    public string projectileTrajectory = "straight";

    public int extraCasts = 0;
    public float extraCastDelay = 0f;

    public int splitCount = 1;
    public float splitAngle = 0f;
}
