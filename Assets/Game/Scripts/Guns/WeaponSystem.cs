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
    public PlayerWeaponConfig DefaultWeapon { get { return defaultWeapon; } }

    [SerializeField] Transform rightHandTransform = null;
    [SerializeField] Transform leftHandTransform = null;

    [SerializeField] float shootRange = 100f;
    [SerializeField] Ammo ammoSlot;

    [SerializeField] TextMeshProUGUI ammoText;

    Camera cam;
    bool canShoot;

    PlayerWeaponConfig currentWeaponConfig;
    Weapon currentWeapon;
    int basicWeaponAmmoCapacity;
    public int currentWeaponAmmo;
    bool isReloading;
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

        //BasicAmmoMagazineCapacity();
        //DisplayInUIAllAmmo();
    }

    Weapon AttachWeapon(PlayerWeaponConfig weapon)
    {
        StartCoroutine(PrepareGunToShoot());
        return weapon.Spawn(rightHandTransform, leftHandTransform);
    }

    public void AmmoInMagazine(int amount)
    {
        GetComponent<WeaponChanger>().weapons[GetComponent<WeaponChanger>().SelectedGun].cachedAmmo = amount;
    }

    void Start()
    {
        canShoot = false;
        //BasicAmmoMagazineCapacity();
        DisplayInUICurrentAmmo();
        DisplayInUIAllAmmo();
    }

    void Update()
    {
        if (!canShoot) return;
        if (isReloading) return;
        if (GetComponent<WeaponChanger>().weapons[GetComponent<WeaponChanger>().SelectedGun].cachedAmmo == 0 &&
            ammoSlot.GetCurrentAmmo(currentWeaponConfig.GetAmmoType()) == 0)
        {
            //play empty sound
            return;
        }

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
        if (GetComponent<WeaponChanger>().weapons[GetComponent<WeaponChanger>().SelectedGun].cachedAmmo >= 1)
        {
            PlayMuzzleFlash();
            ProcessRaycast();
            ReduceAmmoInMagazine();
            canShoot = false;


            DisplayInUICurrentAmmo();
        }
        else
        {
            StartCoroutine(Reload());
            print("auto reload");
        }

        yield return new WaitForSeconds(currentWeaponConfig.GetTimeBetweenShoots());
        canShoot = true;
    }

    void ReduceAmmoInMagazine()
    {
        GetComponent<WeaponChanger>().weapons[GetComponent<WeaponChanger>().SelectedGun].cachedAmmo -= 1;
        //instantiate ammo husk
    }

    IEnumerator Reload()
    {
        isReloading = true;
        //reloading

        yield return new WaitForSeconds(3f);

        //if(ammo < basicWeaponAmmoCapacity) ...
        if (ammoSlot.GetCurrentAmmo(currentWeaponConfig.GetAmmoType()) < basicWeaponAmmoCapacity)
        {
            GetComponent<WeaponChanger>().weapons[GetComponent<WeaponChanger>().SelectedGun].cachedAmmo = ammoSlot.GetCurrentAmmo(currentWeaponConfig.GetAmmoType());
            ammoSlot.ReduceAmmoAmount(currentWeaponConfig.GetAmmoType(), ammoSlot.GetCurrentAmmo(currentWeaponConfig.GetAmmoType()));

            DisplayInUIAllAmmo();
            isReloading = false;

            print("ssd");
            yield break;
        }

        GetComponent<WeaponChanger>().weapons[GetComponent<WeaponChanger>().SelectedGun].cachedAmmo = basicWeaponAmmoCapacity;
        ammoSlot.ReduceAmmoAmount(currentWeaponConfig.GetAmmoType(), basicWeaponAmmoCapacity);

        DisplayInUIAllAmmo();
        isReloading = false;
        print("normal");
        //reloaded        
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

    public void BasicAmmoMagazineCapacity()
    {
        int selectedWeaponAmmoCapacity = currentWeaponConfig.GetMagazineCapacity();
        basicWeaponAmmoCapacity = selectedWeaponAmmoCapacity;
        //GetComponent<WeaponChanger>().weapons[GetComponent<WeaponChanger>().SelectedGun].cachedAmmo = basicWeaponAmmoCapacity;
    }

    void HitBreakableEnemy(RaycastHit hit)
    {
        BodyPart enemy = hit.transform.GetComponentInChildren<BodyPart>();
        if (enemy == null) return;
        enemy.DamageBodyPart(currentWeaponConfig.GetDamage());
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

    void DisplayInUICurrentAmmo()
    {
        StatsDisplay.instance.currentAmmoMagazine.text = GetComponent<WeaponChanger>().weapons[GetComponent<WeaponChanger>().SelectedGun].cachedAmmo.ToString();
    }

    public void DisplayInUIAllAmmo()
    {
        StatsDisplay.instance.currentAmmoMagazine.text = GetComponent<WeaponChanger>().weapons[GetComponent<WeaponChanger>().SelectedGun].cachedAmmo.ToString();
        StatsDisplay.instance.allAmmo.text = ammoSlot.GetCurrentAmmo(currentWeaponConfig.GetAmmoType()).ToString();
    }
}
