using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChanger : MonoBehaviour
{
    [SerializeField] public List<AmmoInWeapon> weapons = new List<AmmoInWeapon>();

    int selectedGun;
    public int SelectedGun { get { return selectedGun; } }

    void Update()
    {
        ChangeWeapon();
        DropWeapon();
        print(selectedGun);
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
        if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
        {
            selectedGun += 1;
            if (selectedGun >= weapons.Count)
            {
                selectedGun = 0;
            }
            UpdateGun();
        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
        {
            selectedGun -= 1;
            if (selectedGun < 0)
            {
                selectedGun = weapons.Count - 1;
            }
            UpdateGun();
        }
    }

    void UpdateGun()
    {
        //stop current reloading
        if (GetComponent<WeaponSystem>().reload != null)
        {

            //StopCoroutine(GetComponent<WeaponSystem>().reload);
            GetComponent<WeaponSystem>().isReloading = false;
        }

        GetComponent<WeaponSystem>().EquipWeapon(weapons[selectedGun].weapon);
        GetComponent<WeaponSystem>().BasicAmmoMagazineCapacity();
        GetComponent<WeaponSystem>().DisplayInUIAllAmmo();
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

    private void CreateDropedItemPrefab()
    {
        Transform dropedTransform = GetComponent<WeaponSystem>().GetCurrentWeaponTransform();
        Pickup dropedWeapon = Instantiate(weapons[selectedGun].weapon.GetEquippedPickupPrefab(), dropedTransform.position, dropedTransform.rotation);
        dropedWeapon.GetComponent<Rigidbody>().AddForce(transform.forward * 3f, ForceMode.Impulse);

        dropedWeapon.ammoLeftInMagazine = weapons[selectedGun].cachedAmmo;
    }
}