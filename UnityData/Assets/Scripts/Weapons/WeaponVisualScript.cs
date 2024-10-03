using UnityEngine;

public class WeaponVisualScript : WeaponSubsystem
{
    [SerializeField] private AudioSource weaponSound;
    [SerializeField] private ParticleSystem muzzleFlash;

    private void Start()
    {
        weapon.OnShoot += PlayFireVisual;
        weapon.OnStartReload += PlayReloadVisual;
    }

    private void PlayFireVisual(int ammo)
    {
        if (ammo > 0)
        {
            muzzleFlash.Play();
            weaponSound.PlayOneShot(weapon.weapon.fireSound);
        }
        else
        {
            weaponSound.PlayOneShot(weapon.weapon.dryFireSound);
        }

    }

    private void PlayReloadVisual()
    {
        weaponSound.PlayOneShot(weapon.weapon.reloadSound);
    }
}