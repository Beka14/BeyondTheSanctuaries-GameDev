
using UnityEngine;

public class WeaponRecoil : WeaponSubsystem
{
    [SerializeField] float recoilRecoverySpeed = 2f;
    [SerializeField] Transform cameraTransform;

    Vector3 recoilRotation;
    Vector3 recoilRotationVelocity;

    private void OnEnable()
    {
        weapon.OnShoot += GenerateRecoil;
    }

    private void OnDisable()
    {
        weapon.OnShoot -= GenerateRecoil;
    }

    void Update()
    {
        recoilRotation = Vector3.SmoothDamp(recoilRotation, Vector3.zero, ref recoilRotationVelocity, recoilRecoverySpeed);
        cameraTransform.localEulerAngles = recoilRotation;
    }

    public void GenerateRecoil(int ammo)
    {
        if(ammo <= 0)
            return;

        recoilRotation += new Vector3(-weapon.weapon.recoil, 0, 0);
    }
}