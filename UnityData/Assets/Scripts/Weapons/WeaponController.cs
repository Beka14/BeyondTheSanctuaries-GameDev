using System;
using UnityEngine;
using System.Linq;
using TMPro;

public class WeaponController : AbstractWeaponController
{
    public event Action<int> OnShoot;

    private bool readyToShoot = false;


    void Start()
    {
        readyToShoot = true;
    }

    public override void Aim()
    {
        animator.SetBool("Aim", true);
    }

    public override void Fire()
    {
        if (!readyToShoot)
            return;

        readyToShoot = false;
        animator.SetTrigger("Poof");
        animator.SetInteger("Ammo", AmmoInClip);
        OnShoot?.Invoke(AmmoInClip);
        AmmoInClip = Mathf.Clamp(AmmoInClip - 1, 0, weapon.clipAmmo);
    }


    public override void Reload()
    {
        OnStartReload?.Invoke();
        animator.SetTrigger("Reload");
    }

    public override void StopAim()
    {
        animator.SetBool("Aim", false);
    }

    public override void StopFire()
    {

    }
    public override void FireEvent(string func)
    {
        Debug.Log(func);
        switch (func)
        {
            case "Ready":
                Ready();
                break;
            case "ReloadFinish":
                ReloadFinish();
                break;
            default:
                break;
        }
    }

    private void Ready()
    {
        readyToShoot = true;
    }
    private void ReloadFinish()
    {
        OnFinishReload?.Invoke();
        Ready();
    }
}

