using System.Collections;
using System.Collections.Generic;
using Project.Control;
using UnityEngine;

public class Pickup : MonoBehaviour, IRaycastable
{
    [SerializeField] PlayerWeaponConfig weaponPickup = null;
    [SerializeField] bool canRespawn;
    [SerializeField] float respawnTime = 2f;

    [Header("optional")]
    [SerializeField] public int ammoLeftInMagazine;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //ThisPickup(other.gameObject);
        }
    }

    void ThisPickup(GameObject subject)
    {
        //if have component with weapon then pick weapon
        if (weaponPickup != null)
        {
            WeaponChanger weaponChanger = subject.GetComponent<WeaponChanger>();
            bool isNotInPossessionObject = weaponChanger.CheckWeapon(weaponPickup);
            WeaponSystem playerWeaponSystem = subject.GetComponent<WeaponSystem>();

            if (isNotInPossessionObject)
            {
                playerWeaponSystem.EquipWeapon(weaponPickup);
                weaponChanger.AddToWeaponList(weaponPickup, ammoLeftInMagazine);
                playerWeaponSystem.BasicAmmoMagazineCapacity();
                playerWeaponSystem.AmmoInMagazine(ammoLeftInMagazine);


                playerWeaponSystem.DisplayInUIAllAmmo();
                if (canRespawn)
                {
                    StartCoroutine(HideForSeconds(respawnTime));
                }
                else
                {
                    Destroy(gameObject);
                }

                return;
            }
            Debug.Log("This weapon is current in your possession!");
        }
        // if (healthToRestore > 0)
        // {
        //     subject.GetComponent<Health>().Heal(healthToRestore);
        // }

    }

    IEnumerator HideForSeconds(float seconds)
    {
        ShowPickup(false);
        yield return new WaitForSeconds(seconds);
        ShowPickup(true);
    }

    void ShowPickup(bool shouldShow)
    {
        GetComponent<Collider>().enabled = shouldShow;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(shouldShow);
        }
    }

    public bool HandleRaycast(Project.Control.PlayerCamera callingController)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            print("Pick");
            ThisPickup(callingController.gameObject);
        }
        return true;
    }

    public int GetAmmoLeftInMagazine()
    {
        return ammoLeftInMagazine;
    }
}