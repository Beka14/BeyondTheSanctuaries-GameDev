
using UnityEngine;

public class BulletCasing : MonoBehaviour
{
    [SerializeField] private AudioSource bulletCasingSound;
    [SerializeField] private float bulletCasingLifeTime;
    private void OnCollisionEnter(Collision collision)
    {
        bulletCasingSound.Play();
        Destroy(gameObject, bulletCasingLifeTime);
    }
}
