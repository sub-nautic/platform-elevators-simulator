using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.WeaponControl
{
    public class WeaponChanger : MonoBehaviour
    {
        [SerializeField] public List<AmmoInWeapon> weapons = new List<AmmoInWeapon>();

        WeaponSystem weaponSystem;
        int selectedGun;
        public int SelectedGun { get { return selectedGun; } }

        private void Awake()
        {
            weaponSystem = GetComponent<WeaponSystem>();
        }

        void Update()
        {
            ChangeWeapon();
            DropWeapon();

            ManualReloading();
            //print(selectedGun);
        }

        public void AddAtStartToWeaponList(PlayerWeaponConfig weapon)
        {
            if (!CheckWeapon(weapon)) return;
            weapons.Add(new AmmoInWeapon(weapon, weapon.GetMagazineCapacity()));
        }

        public void AddToWeaponList(PlayerWeaponConfig weapon, int ammoInWeaponMagazine)
        {
            weapons.Add(new AmmoInWeapon(weapon, ammoInWeaponMagazine));
            IncreaseSelectedGunNumber();
        }

        public void IncreaseSelectedGunNumber()
        {
            selectedGun += 1;
        }

        public bool CheckWeapon(PlayerWeaponConfig obj)
        {
            for (int i = 0; i < weapons.Count; i++)
            {
                if (weapons[i].weapon.name == obj.name)
                {
                    return false;
                }
            }
            return true;
        }

        void ChangeWeapon()
        {
            int previousSelectedWeapon = selectedGun;

            if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
            {
                selectedGun += 1;
                if (selectedGun >= weapons.Count)
                {
                    selectedGun = 0;
                }
            }
            else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
            {
                selectedGun -= 1;
                if (selectedGun < 0)
                {
                    selectedGun = weapons.Count - 1;
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                selectedGun = 0;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (!CanSelectWeapon(2)) return;
                selectedGun = 1;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (!CanSelectWeapon(3)) return;
                selectedGun = 2;
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                if (!CanSelectWeapon(4)) return;
                selectedGun = 3;
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                if (!CanSelectWeapon(5)) return;
                selectedGun = 4;
            }

            if (previousSelectedWeapon != selectedGun)
            {
                print("switch");
                UpdateGun();
            }
        }

        bool CanSelectWeapon(int weaponNumber)
        {
            return weapons.Count >= weaponNumber;
        }

        void UpdateGun()
        {
            weaponSystem.EquipWeapon(weapons[selectedGun].weapon);
            weaponSystem.BasicAmmoMagazineCapacity();
            weaponSystem.DisplayInUIAllAmmo();
        }

        void DropWeapon()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                if (selectedGun == 0) return;
                CreateDropedItemPrefab();

                weapons.Remove(weapons[selectedGun]);
                selectedGun -= 1;
                UpdateGun();
            }
        }

        void ManualReloading()
        {
            if (Input.GetKeyDown(KeyCode.R) && !weaponSystem.isReloading)
            {
                StartCoroutine(weaponSystem.Reload());
            }
        }

        private void CreateDropedItemPrefab()
        {
            Transform dropedTransform = weaponSystem.GetCurrentWeaponTransform();
            Pickup dropedWeapon = Instantiate(weapons[selectedGun].weapon.GetEquippedPickupPrefab(), dropedTransform.position, dropedTransform.rotation);
            dropedWeapon.GetComponent<Rigidbody>().AddForce(transform.forward * 3f, ForceMode.Impulse);

            dropedWeapon.ammoLeftInMagazine = weapons[selectedGun].cachedAmmo;
        }
    }
}