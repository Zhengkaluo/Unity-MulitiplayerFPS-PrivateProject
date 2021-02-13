
using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour
{
    private const string PLAYER_TAG = "Player";
    
    [SerializeField]
    private PlayerWeapon Weapon;
    [SerializeField]
    private GameObject WeaponGraphics;
    [SerializeField]
    private const string WeaponLayerName = "Weapon";
    //refer to camear
    [SerializeField]
    private Camera PlayerCamera;

    [SerializeField]
    private LayerMask Mask;
    private void Start()
    {
        if (PlayerCamera == null)
        {
            Debug.LogError("Playershoot: player camear not found!");
            this.enabled = false;
        }

        WeaponGraphics.layer = LayerMask.NameToLayer(WeaponLayerName);

    }
    private void Update()
    {
        
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    [Client]
    void Shoot()
    {
        RaycastHit _Hit;
        Debug.Log("You shoot");
        if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward, out _Hit, Weapon.Range, Mask))//position, direction, hit information stored, range, layermask
        {
            if(_Hit.collider.tag == PLAYER_TAG)
            {
                CmdPlayerShot(_Hit.collider.name, Weapon.Damage);
            }
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
