using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProjectileType
{
    Bullet,
    Explosive,
}

[CreateAssetMenu(fileName = "Projectile", menuName = "ScriptableObjects/Projectile", order = 2)]

public class Projectile : ScriptableObject
{
    [Header("Projectile settings")]
    public GameObject projectilePrefab;
    public float damage;
    public float projectileSpeed;
    public float projectileLifeTime;
    
    public ProjectileType projectileType;
    
    [Header("Explosive settings")]
    public GameObject explosionPrefab;
    public float explosionRadius;
    public float explosionForce;
    public float explosionDamage;
    
}
