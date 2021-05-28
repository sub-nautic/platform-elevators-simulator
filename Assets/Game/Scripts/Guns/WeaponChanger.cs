using System.Collections.Generic;
using UnityEngine;

public class WeaponChanger : MonoBehaviour
{
    [SerializeField] public List<PlayerWeaponConfig> weapons = new List<PlayerWeaponConfig>();

    int selectedGun;

    void Update()
    {
        ChangeWeapon();
        DropWeapon();
        print(selectedGun);
    }

    public void AddAtStartToWeaponList(PlayerWeaponConfig weapon)
    {
        if (!CheckWeapon(weapon)) return;
        weapons.Add(weapon);
    }

    public void AddToWeaponList(PlayerWeaponConfig weapon)
    {
        weapons.Add(weapon);
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
            if (weapons[i].name == obj.name)
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
        GetComponent<WeaponSystem>().EquipWeapon(weapons[selectedGun]);
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
        Pickup dropedWeapon = Instantiate(weapons[selectedGun].GetEquippedPickupPrefab(), dropedTransform.position, dropedTransform.rotation);
        dropedWeapon.GetComponent<Rigidbody>().AddForce(transform.forward * 3f, ForceMode.Impulse);
    }
}