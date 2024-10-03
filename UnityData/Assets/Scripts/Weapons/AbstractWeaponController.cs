using UnityEngine;

public abstract class AbstractWeaponController : MonoBehaviour, IAnimEventAcceptor
{
    public System.Action OnStartReload;
    public System.Action OnFinishReload;
    public System.Action<int> OnClipAmmoChanged;

    public Weapon weapon;
    public Animator animator;

    protected int ammoInClip = 0;
    public int AmmoInClip { get => ammoInClip;
        set {
            if(ammoInClip != value)
                OnClipAmmoChanged?.Invoke(value);
            ammoInClip = value;
        }
    }

    public abstract void Fire();
    public abstract void StopFire();
    public abstract void Reload();
    public abstract void Aim();
    public abstract void StopAim();
    public abstract void FireEvent(string func);
}