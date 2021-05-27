using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Project.DestructableElements;
using RPG.Combat;

public class WeaponSystem : MonoBehaviour
{
    [SerializeField] PlayerWeaponConfig defaultWeapon = null; public PlayerWeaponConfig DefaultWeapon { get { return defaultWeapon; } }
    [SerializeField] Transform rightHandTransform = null;
    [SerializeField] Transform leftHandTransform = null;

    [SerializeField] float shootRange = 100f;
    [SerializeField] Ammo ammoSlot;

    [SerializeField] TextMeshProUGUI ammoText;

    Camera cam;
    bool canShoot;

    PlayerWeaponConfig currentWeaponConfig;
    Weapon currentWeapon;
    public Transform GetCurrentWeaponTransform()
    {
        return currentWeapon.transform;
    }

    private void Awake()
    {
        cam = GetComponentInChildren<Camera>();
        currentWeaponConfig = defaultWeapon;
        currentWeapon = SetupDefaultWeapon();
    }

    Weapon SetupDefaultWeapon()
    {
        GetComponent<WeaponChanger>().AddAtStartToWeaponList(defaultWeapon);
        return AttachWeapon(defaultWeapon);
    }

    public void EquipWeapon(PlayerWeaponConfig weapon)
    {
        currentWeaponConfig = weapon;
        currentWeapon = AttachWeapon(weapon);
    }

    Weapon AttachWeapon(PlayerWeaponConfig weapon)
    {
        StartCoroutine(PrepareGunToShoot());
        return weapon.Spawn(rightHandTransform, leftHandTransform);
    }

    void Start()
    {
        canShoot = false;
    }

    void Update()
    {
        if (!canShoot) return;

        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(Shoot());
        }
        else if (Input.GetMouseButton(0) && currentWeaponConfig.IsAutomatic())
        {
            StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot()
    {
        if (ammoSlot.GetCurrentAmmo(currentWeaponConfig.GetAmmoType()) >= 1)
        {
            PlayMuzzleFlash();
            ProcessRaycast();
            ammoSlot.ReduceAmmoAmount(currentWeaponConfig.GetAmmoType());
            canShoot = false;
            //DisplayAmmo();
        }
        else { Debug.Log("No ammo :o"); }
        yield return new WaitForSeconds(currentWeaponConfig.GetTimeBetweenShoots());
        canShoot = true;
    }

    void ProcessRaycast()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        ray.origin = cam.transform.position;

        if (Physics.Raycast(ray, out RaycastHit hit, shootRange))
        {
            Debug.Log("I hit this: " + hit.transform.name);

            if (hit.collider.transform != null)
            {
                HitBreakableEnemy(hit);

            }

            CreateHitImpact(hit);

            //EnemyHealth target = hit.transform.GetComponent<EnemyHealth>();
            //if (target == null) { return; }
            //target.DecreaseHitPoints(damageAmount);

        }
        else { return; }
    }

    void HitBreakableEnemy(RaycastHit hit)
    {
        BodyPart enemy = hit.transform.GetComponentInChildren<BodyPart>();
        if (enemy == null) return;
        enemy.DamageBodyPart(currentWeaponConfig.GetDamage());
    }

    void DisplayAmmo()
    {
        int currentAmmo = ammoSlot.GetCurrentAmmo(currentWeaponConfig.GetAmmoType());
        ammoText.text = "Ammo: " + currentAmmo.ToString();
    }

    void PlayMuzzleFlash()
    {
        if (currentWeapon.GetComponentInChildren<ParticleSystem>() == null) return;
        currentWeapon.GetComponentInChildren<ParticleSystem>().Play();
    }

    void CreateHitImpact(RaycastHit hit)
    {
        if (currentWeaponConfig.GetHitEffect() == null) return;
        GameObject impact = Instantiate(currentWeaponConfig.GetHitEffect(), hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(impact, 0.1f);
    }

    IEnumerator PrepareGunToShoot()
    {
        yield return new WaitForSeconds(0.5f);
        canShoot = true;
    }
}
