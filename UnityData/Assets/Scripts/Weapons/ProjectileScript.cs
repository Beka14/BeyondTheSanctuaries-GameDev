using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    [SerializeField] private GameObject projectileWoodImpact;
    [SerializeField] private AudioClip projectileWoodImpactSound;
    [SerializeField] private GameObject projectileMetalImpact;
    [SerializeField] private AudioClip projectileMetalImpactSound;
    [SerializeField] private GameObject projectileConcreteImpact;
    [SerializeField] private AudioClip projectileConcreteImpactSound;
    [SerializeField] private GameObject projectileDirtImpact;
    [SerializeField] private AudioClip projectileDirtImpactSound;
    [SerializeField] private GameObject projectileWaterImpact;
    [SerializeField] private AudioClip projectileWaterImpactSound;
    [SerializeField] private GameObject projectileFleshImpact;
    [SerializeField] private AudioClip projectileFleshImpactSound;
    [SerializeField] private GameObject projectileGlassImpact;
    [SerializeField] private AudioClip projectileGlassImpactSound;


    private void OnCollisionEnter(Collision collision)
    {
        CreateProjectileImpactEffect(collision);
        
    }

    private void CreateProjectileImpactEffect(Collision objectWeHit)
    {
        //Debug.Log(objectWeHit.gameObject.name + " " + objectWeHit.gameObject.tag);
        var contactPoint = objectWeHit.contacts[0];
        GameObject impact = null;
        AudioClip projectileAudioSourceToPlay = null;

        // in this case we do not destroy the impact effect because prefabs have its own script for it. 
        if (objectWeHit.gameObject.CompareTag("Wood"))
        {
            projectileAudioSourceToPlay = projectileWoodImpactSound;
            impact = Instantiate(projectileWoodImpact, contactPoint.point,
                Quaternion.LookRotation(contactPoint.normal));
        }
        else if (objectWeHit.gameObject.CompareTag("Metal"))
        {
            projectileAudioSourceToPlay = projectileMetalImpactSound;
            impact = Instantiate(projectileMetalImpact, contactPoint.point,
                Quaternion.LookRotation(contactPoint.normal));
        }
        else if (objectWeHit.gameObject.CompareTag("Concrete"))
        {
            projectileAudioSourceToPlay = projectileConcreteImpactSound;
            impact = Instantiate(projectileConcreteImpact, contactPoint.point,
                Quaternion.LookRotation(contactPoint.normal));
        }
        else if (objectWeHit.gameObject.CompareTag("Dirt"))
        {
            projectileAudioSourceToPlay = projectileDirtImpactSound;
            impact = Instantiate(projectileDirtImpact, contactPoint.point,
                Quaternion.LookRotation(contactPoint.normal));
        }
        else if (objectWeHit.gameObject.CompareTag("Water"))
        {
            projectileAudioSourceToPlay = projectileWaterImpactSound;
            impact = Instantiate(projectileWaterImpact, contactPoint.point,
                Quaternion.LookRotation(contactPoint.normal));
        }
        else if (objectWeHit.gameObject.CompareTag("Flesh") || objectWeHit.gameObject.CompareTag("Player"))
        {
            projectileAudioSourceToPlay = projectileFleshImpactSound;
            impact = Instantiate(projectileFleshImpact, contactPoint.point,
                Quaternion.LookRotation(contactPoint.normal));
        }
        else if (objectWeHit.gameObject.CompareTag("Glass"))
        {
            projectileAudioSourceToPlay = projectileGlassImpactSound;
            impact = Instantiate(projectileGlassImpact, contactPoint.point,
                Quaternion.LookRotation(contactPoint.normal));
        }

        if (impact == null) return;
        var audioSource = impact.AddComponent<AudioSource>();
        SetAudioSource(audioSource, projectileAudioSourceToPlay);
        impact.transform.SetParent(objectWeHit.transform);
        
        Destroy(gameObject);
    }

    private static void SetAudioSource(AudioSource audioSource, AudioClip audioClip)
    {
        audioSource.spatialBlend = 1;
        audioSource.maxDistance = 20;
        audioSource.PlayOneShot(audioClip);
    }
}