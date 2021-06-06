using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.WeaponControl
{
    public class Ammo : MonoBehaviour
    {
        [SerializeField] AmmoSlot[] ammoSlots;

        [System.Serializable]
        class AmmoSlot
        {
            public AmmoType ammoType;
            public int ammoAmount;
        }

        public int GetCurrentAmmo(AmmoType ammoType)
        {
            return GetAmmoSlot(ammoType).ammoAmount;
        }

        public void IncreaseAmmoAmount(AmmoType ammoType, int amount)
        {
            GetAmmoSlot(ammoType).ammoAmount += amount;
        }

        public void ReduceAmmoAmount(AmmoType ammoType, int amount)
        {
            GetAmmoSlot(ammoType).ammoAmount -= amount;
        }

        AmmoSlot GetAmmoSlot(AmmoType ammoType)
        {
            foreach (AmmoSlot slot in ammoSlots)
            {
                if (slot.ammoType == ammoType)
                {
                    return slot;
                }
            }
            return null;
        }
    }
}