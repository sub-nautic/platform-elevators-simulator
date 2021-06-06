using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Project.DestructableElements;
using RPG.Combat;

namespace Project.WeaponControl
{
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
        WeaponChanger weaponChanger;
        bool canShoot;

        PlayerWeaponConfig currentWeaponConfig;
        Weapon currentWeapon;
        int basicWeaponAmmoCapacity;
        int currentAmmo;
        public bool isReloading;

        public Transform GetCurrentWeaponTransform()
        {
            return currentWeapon.transform;
        }

        private void Awake()
        {
            cam = GetComponentInChildren<Camera>();
            weaponChanger = GetComponent<WeaponChanger>();
            currentWeaponConfig = defaultWeapon;
            currentWeapon = SetupDefaultWeapon();
        }

        void Start()
        {
            canShoot = false;
            int currentAmmo = weaponChanger.weapons[weaponChanger.SelectedGun].cachedAmmo;
            //BasicAmmoMagazineCapacity();
            DisplayInUICurrentAmmo();
            DisplayInUIAllAmmo();
        }

        void Update()
        {
            if (!canShoot) return;
            if (isReloading) return;
            if (weaponChanger.weapons[weaponChanger.SelectedGun].cachedAmmo == 0 &&
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

        Weapon SetupDefaultWeapon()
        {
            weaponChanger.AddAtStartToWeaponList(defaultWeapon);
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

        public void AmmoInMagazine(int amount)
        {
            weaponChanger.weapons[weaponChanger.SelectedGun].cachedAmmo = amount;
        }

        IEnumerator Shoot()
        {
            if (weaponChanger.weapons[weaponChanger.SelectedGun].cachedAmmo >= 1)
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
            weaponChanger.weapons[weaponChanger.SelectedGun].cachedAmmo -= 1;
            //instantiate ammo husk
        }

        /// <summary>
        /// Reload coroutine
        /// </summary>
        /// <returns></returns>
        public IEnumerator Reload()
        {
            isReloading = true;

            int currentWeaponNum = weaponChanger.SelectedGun;

            const float secToReload = 3f; //When to Reload (Every 3 second)
            float counter = 0;

            while (true)
            {
                //Check if we have reached counter
                if (counter > secToReload)
                {
                    counter = 0f; //Reset Counter

                    //if ammo is lower then maximum ammo capacity for current weapon
                    if (ammoSlot.GetCurrentAmmo(currentWeaponConfig.GetAmmoType()) < basicWeaponAmmoCapacity)
                    {
                        weaponChanger.weapons[weaponChanger.SelectedGun].cachedAmmo = ammoSlot.GetCurrentAmmo(currentWeaponConfig.GetAmmoType());
                        ammoSlot.ReduceAmmoAmount(currentWeaponConfig.GetAmmoType(), ammoSlot.GetCurrentAmmo(currentWeaponConfig.GetAmmoType()));

                        DisplayInUIAllAmmo();
                        isReloading = false;

                        print("last reload");
                        yield break;
                    }

                    weaponChanger.weapons[weaponChanger.SelectedGun].cachedAmmo = basicWeaponAmmoCapacity;
                    ammoSlot.ReduceAmmoAmount(currentWeaponConfig.GetAmmoType(), basicWeaponAmmoCapacity);

                    DisplayInUIAllAmmo();
                    isReloading = false;

                    print("normal reload");
                    yield break;
                }

                //Increment counter
                counter += Time.deltaTime;

                if (currentWeaponNum != weaponChanger.SelectedGun)
                {
                    Debug.Log("Action is break");
                    isReloading = false;
                    yield break;
                }

                yield return null;
            }
        }

        /// <summary>
        /// Send raycast towards enemies
        /// </summary>
        void ProcessRaycast()
        {
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            ray.origin = cam.transform.position;

            if (Physics.Raycast(ray, out RaycastHit hit, shootRange))
            {
                //Debug.Log("I hit this: " + hit.transform.name);

                //Temporary will affect only enemies with structure of skeletons
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
            float prepareTimeGunToShoot = 0.5f;

            yield return new WaitForSeconds(prepareTimeGunToShoot);
            canShoot = true;
        }

        void DisplayInUICurrentAmmo()
        {
            StatsDisplay.instance.currentAmmoMagazine.text = weaponChanger.weapons[weaponChanger.SelectedGun].cachedAmmo.ToString();
        }

        public void DisplayInUIAllAmmo()
        {
            StatsDisplay.instance.currentAmmoMagazine.text = weaponChanger.weapons[weaponChanger.SelectedGun].cachedAmmo.ToString();
            StatsDisplay.instance.allAmmo.text = ammoSlot.GetCurrentAmmo(currentWeaponConfig.GetAmmoType()).ToString();
        }
    }
}