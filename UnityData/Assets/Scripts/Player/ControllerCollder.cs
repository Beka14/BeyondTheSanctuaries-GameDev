using System;
using UnityEngine;

public class ControllerCollider : MonoBehaviour
{
    public Action OnBodyHit;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            OnBodyHit?.Invoke();
        }
    }
}