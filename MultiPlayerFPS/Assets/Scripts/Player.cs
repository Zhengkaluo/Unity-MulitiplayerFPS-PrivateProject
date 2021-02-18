
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
[RequireComponent(typeof(PlayerSetUp))]
public class Player : NetworkBehaviour
{
    [SyncVar, SerializeField]
    private bool _IsDead = false;
    public bool isDead
    {
        get { return _IsDead; }
        protected set { _IsDead = value; }
    }
    [SerializeField]
    private int MaxHealth = 100;

    
    //need to sync to all client and server
    [SyncVar]
    private int CurrentHealth;

    [SerializeField]
    private Behaviour[] DisableOnDeath;
    [SerializeField]
    private bool[] WasEnabled;

    [SerializeField]
    private GameObject DeadEffect;
    [SerializeField]
    private GameObject SpawnEffect;
    [SerializeField]
    private GameObject[] DisableGameObjectOnDeath;
    public void Setup()
    {
        WasEnabled = new bool[DisableOnDeath.Length];
        for (int i = 0; i < WasEnabled.Length; i++)
        {//loop through components and store if they are enabled
            WasEnabled[i] = DisableOnDeath[i].enabled;
        }
        SetDefaults();
    }
    IEnumerator Respawn() {
        yield return new WaitForSeconds(GameManager.Instance.ThisMatchSettings.RespawnWaitTime);

        Transform _SpawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = _SpawnPoint.position;
        transform.rotation = _SpawnPoint.rotation;
        Debug.Log(transform.name + " Player respawn");
        SetDefaults();
    }

    public void SetDefaults()
    {
        isDead = false;
        CurrentHealth = MaxHealth;
        for (int i = 0; i < DisableOnDeath.Length; i++)
        {//collider is not included
            DisableOnDeath[i].enabled = WasEnabled[i];
        }
        //enable game object, make them visible
        for (int i = 0; i < DisableGameObjectOnDeath.Length; i++)
        {
            DisableGameObjectOnDeath[i].SetActive(true);
        }
        Collider _Col = GetComponent<Collider>();
        if (_Col != null)
        {
            _Col.enabled = true;
        }
        if (isLocalPlayer)
        {
            GameManager.Instance.SetSceneCameraActive(false);
            GetComponent<PlayerSetUp>().PlayerUIInstance.SetActive(true);
        }
        //create spawn Effect GameObject 
        GameObject _GraphicsIns = Instantiate(SpawnEffect, transform.position, Quaternion.identity);
        Destroy(_GraphicsIns, 3f);
    }
    private void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.K))//k for kill, for debuging
        {
           RpcTakeDamage(999);
       }
    }

    [ClientRpc]
    public void RpcTakeDamage(int _DamageAmount)
    {
        if (isDead)
        {
            return;
        }
        CurrentHealth -= _DamageAmount;

        Debug.Log(transform.name + " now currentHealth " + CurrentHealth);
        if(CurrentHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {   //set isdead
        isDead = true;
        Debug.Log(transform.name + " is dead");
        //disable components
        for (int i = 0; i < DisableOnDeath.Length; i++)
        {
            DisableOnDeath[i].enabled = false;
        }
        //game object disable, meaning not displaying
        for (int i = 0; i < DisableGameObjectOnDeath.Length; i++)
        {
            DisableGameObjectOnDeath[i].SetActive(false);
        }
        //enable collisder
        Collider _Col = GetComponent<Collider>();
        if (_Col != null)
        {
            _Col.enabled = false;
        }

        //display dead effect
        GameObject _GraphicsIns = Instantiate(DeadEffect, transform.position, Quaternion.identity);
        Destroy(_GraphicsIns, 3f);

        //switch Camera after local player die
        if (isLocalPlayer)
        {
            GameManager.Instance.SetSceneCameraActive(true);
            GetComponent<PlayerSetUp>().PlayerUIInstance.SetActive(false);
        }

        //call respawn method
        StartCoroutine(Respawn());
    }

}
