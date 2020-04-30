using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PastaShotConfig
{
    public bool cooked;
    public int damage;

    public Sound firingSound;

    [Header("Firing")]
    public float reloadSpeed;

    public float missileAmount;
    public float missileSpreadAngle;

    [Header("Travelling")]
    public float range;
    public float travelDuration;
    public AnimationCurve yTrajectory;
    public bool goesToGround;
    public bool straight;

    [Header("Projectile")]
    public Sprite projectileSprite;
    public Vector2 spriteObjectScale = Vector2.one;
    public Vector2 spriteObjectOffset = Vector2.one;
    public Vector2 colliderSize = Vector2.one;


}
