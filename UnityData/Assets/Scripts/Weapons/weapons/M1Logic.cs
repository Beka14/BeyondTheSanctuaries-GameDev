
using UnityEngine;

class M1Logic : WeaponLogic
{
    public override void FireWeapon(int ammoLeft)
    {
        base.FireWeapon(ammoLeft);
        // Last ammo in the clip
        if (ammoLeft == 1)
            ThrowClip();
    }

    void ThrowClip()
    {
        var bulletClip = Instantiate(weapon.weapon.clip, shellSpawn.position,
    shellSpawn.rotation);
        bulletClip.GetComponent<Rigidbody>().AddForce(weapon.weapon.shellEjectionForce * shellSpawn.up,
            ForceMode.Impulse);
        Destroy(bulletClip, ttlShell);
    }
}