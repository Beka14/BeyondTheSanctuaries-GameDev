using UnityEngine;

[RequireComponent(typeof(WeaponController))]
public class WeaponSubsystem : MonoBehaviour
{
    [HideInInspector] public WeaponController weapon;

    protected virtual void Awake()
    {
        weapon = transform.root.GetComponentInChildren<WeaponController>();
    }
}