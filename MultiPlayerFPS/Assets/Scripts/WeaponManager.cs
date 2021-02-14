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

    private WeaponGraphics CurrentGraphics;

    private void Start()
    {
        EquipWeapon(PrimaryWeapon);
    }

    public WeaponGraphics GetCurrentGraphics()
    {
        return CurrentGraphics;
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

        CurrentGraphics = _WeaponIns.GetComponent<WeaponGraphics>();
        if(CurrentGraphics == null)
        {
            Debug.LogError("No Weapon Graphics Component on the weapon object: " + _WeaponIns.name);
        }
        if (isLocalPlayer)
        {
            Util.SetLayerRecursively(_WeaponIns, LayerMask.NameToLayer(WeaponLayerName));
            //_WeaponIns.layer = LayerMask.NameToLayer(WeaponLayerName);
        }
    }
}
