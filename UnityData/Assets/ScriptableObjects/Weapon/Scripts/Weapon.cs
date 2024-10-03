using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShootingMode
{
    Single,
    Auto,
    Burst
}

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Weapon", order = 1)]
public class Weapon : ScriptableObject
{
    [Header("Projectile settings")]
    public Projectile projectile;

    [Header("Projectile shell settings")]
    public GameObject clip;
    public GameObject projectileShell;
    public float shellEjectionForce;

    [Header("Weapon stats")]
    public int maxAmmo;
    public int clipAmmo;
    public float fireRate;
    public float reloadTime;
    public float spread;
    public float recoil;
    public ShootingMode shootingMode;
    public int bulletsPerBurst;
    public ItemObject ammo;
    public ItemSO ammoType;

    [Header("Weapon visuals")]
    public string weaponName;
    public RuntimeAnimatorController animatorController;

    [Header("Weapon sounds")]
    public AudioClip fireSound;
    public AudioClip reloadSound;
    public AudioClip dryFireSound;
}
