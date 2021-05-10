using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Project.DestructableElements;

public class Gun : MonoBehaviour
{
    [SerializeField] Camera FPCamera;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] GameObject hitEffect;

    [SerializeField] float shootRange = 100f;
    [SerializeField] float damageAmount = 50f;
    [SerializeField] float timeBetweenShoots = 0.3f;

    [SerializeField] Ammo ammoSlot;
    [SerializeField] AmmoType ammoType;

    [SerializeField] TextMeshProUGUI ammoText;

    bool canShoot;

    private void OnEnable()
    {
        canShoot = false;
        StartCoroutine(PrepareGunToShoot()); //on switch weapon need to wait
        //DisplayAmmo();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (canShoot)
            {
                StartCoroutine(Shoot());
            }
        }
    }

    IEnumerator Shoot()
    {
        if (ammoSlot.GetCurrentAmmo(ammoType) >= 1)
        {
            //PlayMuzzleFlash();
            ProcessRaycast();
            ammoSlot.ReduceAmmoAmount(ammoType);
            canShoot = false;
            //DisplayAmmo();
        }
        else { Debug.Log("No ammo :o"); }
        yield return new WaitForSeconds(timeBetweenShoots);
        canShoot = true;
    }

    void DisplayAmmo()
    {
        int currentAmmo = ammoSlot.GetCurrentAmmo(ammoType);
        ammoText.text = "Ammo: " + currentAmmo.ToString();
    }

    void PlayMuzzleFlash()
    {
        muzzleFlash.Play();
    }

    void ProcessRaycast()
    {
        RaycastHit hit;
        if (Physics.Raycast(FPCamera.transform.position, FPCamera.transform.forward, out hit, shootRange))
        {
            Debug.Log("I hit this: " + hit.transform.name);

            if (hit.collider.transform != null)
            {
                BodyPart enemy = hit.transform.GetComponentInChildren<BodyPart>();
                if (enemy == null) return;
                enemy.DamageBodyPart();
            }

            //CreateHitImpact(hit);

            //EnemyHealth target = hit.transform.GetComponent<EnemyHealth>();
            //if (target == null) { return; }
            //target.DecreaseHitPoints(damageAmount);

        }
        else { return; }
    }

    void CreateHitImpact(RaycastHit hit)
    {
        GameObject impact = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(impact, 0.1f);
    }

    IEnumerator PrepareGunToShoot()
    {
        yield return new WaitForSeconds(1);
        canShoot = true;
    }
}
