using UnityEngine;
using UnityEngine.Networking;

public class WeaponManager : NetworkBehaviour
{
    [SerializeField]
    private const string WeaponLayerName = "Weapon";

    [SerializeField]
    private Transform WeaponHolder;

    [SerializeField]
    private PlayerWeapon PrimaryWeapon;

    private PlayerWeapon CurrentWeapon;

    private void Start()
    {
        EquipWeapon(PrimaryWeapon);
    }
    public PlayerWeapon GetCurrentWeapon()
    {
        return CurrentWeapon;
    }
    void EquipWeapon(PlayerWeapon _Weapon)
    {
        CurrentWeapon = _Weapon;
        GameObject _WeaponIns = (GameObject)Instantiate(_Weapon.Graphics, WeaponHolder.position, WeaponHolder.rotation);
        _WeaponIns.transform.SetParent(WeaponHolder);
        if (isLocalPlayer)
        {
            _WeaponIns.layer = LayerMask.NameToLayer(WeaponLayerName);
        }
    }
}
