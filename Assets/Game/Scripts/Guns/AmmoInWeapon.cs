[System.Serializable]
public class AmmoInWeapon
{
    public PlayerWeaponConfig weapon;
    public int cachedAmmo;

    public AmmoInWeapon(PlayerWeaponConfig newWeapon, int newCachedAmmo)
    {
        weapon = newWeapon;
        cachedAmmo = newCachedAmmo;
    }
}