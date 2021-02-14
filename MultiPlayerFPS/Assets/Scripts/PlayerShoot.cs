
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : NetworkBehaviour
{
    private const string PLAYER_TAG = "Player";

    //[SerializeField]
    //private GameObject WeaponGraphics;
    
    //refer to camear
    [SerializeField]
    private Camera PlayerCamera;

    [SerializeField]
    private LayerMask Mask;

    private PlayerWeapon CurrentWeapon;
    //private 
    private WeaponManager PlayerWeaponManager;
    private void Start()
    {
        if (PlayerCamera == null)
        {
            Debug.LogError("Playershoot: player camera not found!");
            this.enabled = false;
        }
        PlayerWeaponManager = GetComponent<WeaponManager>();
        //WeaponGraphics.layer = LayerMask.NameToLayer(WeaponLayerName);

    }
    private void Update()
    {
        CurrentWeapon = PlayerWeaponManager.GetCurrentWeapon();
        if (CurrentWeapon.FireRate <= 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                InvokeRepeating("Shoot", 0f, 1f / CurrentWeapon.FireRate);
                //Shoot();
            }
            else if (Input.GetButtonUp("Fire1")){
                CancelInvoke("Shoot");
            }
        }
        
    }
    //when player shoots
    [Command]
    void CmdShoot()
    {
        RpcDoShootEffect();
    }
    //all clients call becuase effects are shown on all screens
    [ClientRpc]
    void RpcDoShootEffect()//display muzzle flash on all sever;
    {
        PlayerWeaponManager.GetCurrentGraphics().MuzzleFlash.Play();
    }
    [Command]
    void CmdOnHit(Vector3 _Pos, Vector3 _Normal)
    {
        RpcDoHitEffect(_Pos, _Normal);
    }
    [ClientRpc]
    void RpcDoHitEffect(Vector3 _Pos, Vector3 _Normal)//display muzzle flash on all sever;
    {
        GameObject _HitEffect = (GameObject)Instantiate(PlayerWeaponManager.GetCurrentGraphics().HitEffect, _Pos, Quaternion.LookRotation(_Normal));
        Destroy(_HitEffect, 1f);
        //PlayerWeaponManager.GetCurrentGraphics().HitEffect.Play();
    }
    [Client]
    void Shoot()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        //shooting, call On shoot Method on server
        CmdShoot();
        RaycastHit _Hit;
        Debug.Log("You shoot");
        if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward, out _Hit, CurrentWeapon.Range, Mask))//position, direction, hit information stored, range, layermask
        {
            if(_Hit.collider.tag == PLAYER_TAG)
            {
                CmdPlayerShot(_Hit.collider.name, CurrentWeapon.Damage);
            }
            CmdOnHit(_Hit.point, _Hit.normal);
            Debug.Log("WE hit " + _Hit.collider.name);
            //we hit something
        }
    }

    [Command]//method called only on server
    void CmdPlayerShot(string _PlayerID, int _DamageAmount)
    {
        Debug.Log(_PlayerID + "has been shot. ");

        Player _Player = GameManager.GetPlayer(_PlayerID);
        _Player.RpcTakeDamage(_DamageAmount);

        //Destroy(GameObject.Find(_ID));
    }
}
