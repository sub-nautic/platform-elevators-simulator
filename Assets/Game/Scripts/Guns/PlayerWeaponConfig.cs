using RPG.Attributes;
using RPG.Combat;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Weapon", menuName = "Weapons/Make New Player Weapon", order = 0)]
public class PlayerWeaponConfig : ScriptableObject
{
    [SerializeField] float weaponDamage = 5f;
    [SerializeField] float percentageBonus = 0f;
    [SerializeField] float timeBetweenShoots = 0.1f;
    [SerializeField] AmmoType ammoType;
    [SerializeField] Weapon equippedPrefab = null;
    [SerializeField] bool isAutomatic = true;
    [SerializeField] bool isRightHanded = true;
    [SerializeField] GameObject hitEffect;
    [SerializeField] Projectile projectile = null;

    const string weaponName = "Weapon";

    public Weapon Spawn(Transform rightHand, Transform leftHand)
    {
        DestroyOldWeapon(rightHand, leftHand);

        Weapon weapon = null;

        if (equippedPrefab != null)
        {
            Transform handTransform = GetTransform(rightHand, leftHand);

            weapon = Instantiate(equippedPrefab, handTransform);
            weapon.gameObject.name = weaponName;
        }

        return weapon;
    }

    void DestroyOldWeapon(Transform rightHand, Transform leftHand)
    {
        Transform oldWeapon = rightHand.Find(weaponName);
        if (oldWeapon == null)
        {
            oldWeapon = leftHand.Find(weaponName);
        }
        if (oldWeapon == null) return;

        //have to rename oldWeapon to avoid some issues
        oldWeapon.name = "DESTROYING";
        Destroy(oldWeapon.gameObject);
    }

    Transform GetTransform(Transform rightHand, Transform leftHand)
    {
        Transform handTransform;
        if (isRightHanded) handTransform = rightHand;
        else handTransform = leftHand;
        return handTransform;
    }

    public float GetDamage()
    {
        return weaponDamage;
    }

    public float GetPercentageBonus()
    {
        return percentageBonus;
    }

    public float GetTimeBetweenShoots()
    {
        return timeBetweenShoots;
    }

    public AmmoType GetAmmoType()
    {
        return ammoType;
    }

    public GameObject GetHitEffect()
    {
        return hitEffect;
    }

    public bool IsAutomatic()
    {
        return isAutomatic;
    }

    public bool HasProjectile()
    {
        return projectile != null;
    }

    public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float calculatedDamage)
    {
        Projectile projectileInstance = Instantiate(projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
        projectileInstance.SetTarget(target, instigator, calculatedDamage);
    }
}