using UnityEngine;

public class Projectile 
{
    public string trajectory = "straight";
    public string speed = "10"; //IDk why this is a sting, prob rpn eval
    public int sprite = 0;
    public string lifetime= "15"; //same string thing as speed, prob rpn eval
    public GameObject projectileObject; //This is the prefab that will be spawned when this projectile is cast. It should be set in the inspector.
}
