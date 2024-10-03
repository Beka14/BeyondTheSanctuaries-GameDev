using UnityEngine;

[RequireComponent(typeof(WeaponController))]
public class WeaponLogic : WeaponSubsystem
{
    [SerializeField] protected Transform bulletSpawn;
    [SerializeField] protected Transform shellSpawn;

    static readonly protected float ttlShell = 30f;

    private void Start()
    {
        weapon.OnShoot += FireWeapon;
    }

    public virtual void FireWeapon(int ammoLeft)
    {
        if (ammoLeft <= 0)
            return;

        var shootingDirection = CalculateSpreadAndDirection().normalized;

        var weaponProjectile = weapon.weapon.projectile;
        var projectile = Instantiate(weaponProjectile.projectilePrefab, bulletSpawn.position,
            bulletSpawn.rotation);

        projectile.transform.forward = shootingDirection;

        projectile.GetComponent<Rigidbody>()
            .AddForce(shootingDirection * weaponProjectile.projectileSpeed, ForceMode.Impulse);

        Destroy(projectile, ttlShell);
        SpawnBulletShell();
    }

    private Vector3 CalculateSpreadAndDirection()
    {
        var ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 direction;
        if (Physics.Raycast(ray, out var hit))
        {
            direction = hit.point - bulletSpawn.position;
        }
        else
        {
            direction = ray.GetPoint(100) - bulletSpawn.position;
        }

        var x = Random.Range(-weapon.weapon.spread, weapon.weapon.spread);
        var y = Random.Range(-weapon.weapon.spread, weapon.weapon.spread);
        return direction + new Vector3(x, y);
    }

    private void SpawnBulletShell()
    {
        var bulletCasing = Instantiate(weapon.weapon.projectileShell, shellSpawn.position,
            shellSpawn.rotation);
        bulletCasing.GetComponent<Rigidbody>().AddForce(shellSpawn.right * weapon.weapon.shellEjectionForce,
            ForceMode.Impulse);
        Destroy(bulletCasing, ttlShell);
    }
}

