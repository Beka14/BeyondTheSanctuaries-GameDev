using UnityEngine;

public class FistsController : AbstractWeaponController
{
    [SerializeField] private Transform bulletSpawn;

    private void Awake()
    {
        ammoInClip = -1;
    }

    public override void Fire()
    {
        animator.SetTrigger("Paunch");
    }

    public override void StopFire()
    {

    }

    public override void Reload()
    {

    }

    public override void Aim()
    {

    }

    public override void StopAim()
    {

    }

    //-----------------------------------------------------------------------------------
    public override void FireEvent(string func)
    {
        Invoke(func, 0);
    }

    // make logic for weapon
    public void Paunch()
    {
        hack_Punch();
    }

    private void hack_Punch()
    {
        var shootingDirection = CalculateSpreadAndDirection().normalized;

        var weaponProjectile = weapon.projectile;
        var projectile = Instantiate(weaponProjectile.projectilePrefab, bulletSpawn.position,
            bulletSpawn.rotation);

        projectile.transform.forward = shootingDirection;

        projectile.GetComponent<Rigidbody>()
            .AddForce(shootingDirection * weaponProjectile.projectileSpeed, ForceMode.Impulse);

        Destroy(projectile, weapon.projectile.projectileLifeTime);
    }
    private Vector3 CalculateSpreadAndDirection()
    {
        var ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 direction = Physics.Raycast(ray, out var hit)
            ? hit.point - bulletSpawn.position
            : ray.GetPoint(100) - bulletSpawn.position;
        var x = Random.Range(-weapon.spread, weapon.spread);
        var y = Random.Range(-weapon.spread, weapon.spread);
        return direction + new Vector3(x, y);
    }
}