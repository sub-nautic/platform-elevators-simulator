using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Project.DestructableElements;
using RPG.Combat;

public class WeaponSystem : MonoBehaviour
{
    [SerializeField] PlayerWeaponConfig defaultWeapon = null;
    [SerializeField] Transform rightHandTransform = null;
    [SerializeField] Transform leftHandTransform = null;

    [SerializeField] float shootRange = 100f;
    [SerializeField] Ammo ammoSlot;

    [SerializeField] TextMeshProUGUI ammoText;

    Camera cam;
    bool canShoot;

    PlayerWeaponConfig currentWeaponConfig;
    Weapon currentWeapon;

    private void Awake()
    {
        currentWeaponConfig = defaultWeapon;
        currentWeapon = SetupDefaultWeapon();
    }

    Weapon SetupDefaultWeapon()
    {
        return AttachWeapon(defaultWeapon);
    }

    public void EquipWeapon(PlayerWeaponConfig weapon)
    {
        currentWeaponConfig = weapon;
        currentWeapon = AttachWeapon(weapon);
    }

    Weapon AttachWeapon(PlayerWeaponConfig weapon)
    {
        return weapon.Spawn(rightHandTransform, leftHandTransform);
    }

    void OnEnable()
    {
        canShoot = false;
        StartCoroutine(PrepareGunToShoot()); //on switch weapon need to wait
        //DisplayAmmo();
    }

    void Start()
    {
        cam = GetComponentInChildren<Camera>();
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

    void DisplayAmmo()
    {
        int currentAmmo = ammoSlot.GetCurrentAmmo(currentWeaponConfig.GetAmmoType());
        ammoText.text = "Ammo: " + currentAmmo.ToString();
    }

    void PlayMuzzleFlash()
    {
        currentWeapon.GetComponentInChildren<ParticleSystem>().Play();
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
        enemy.DamageBodyPart();
    }

    void CreateHitImpact(RaycastHit hit)
    {
        GameObject impact = Instantiate(currentWeaponConfig.GetHitEffect(), hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(impact, 0.1f);
    }

    IEnumerator PrepareGunToShoot()
    {
        yield return new WaitForSeconds(1);
        canShoot = true;
    }
}
