
using UnityEngine;
using UnityEngine.Networking;
public class Player : NetworkBehaviour
{
    [SerializeField]
    private int MaxHealth = 100;

    //need to sync to all client and server
    [SyncVar]
    private int CurrentHealth;

    private void Awake()
    {
        SetDefaults();

    }

    public void SetDefaults()
    {
        CurrentHealth = MaxHealth;
    }
    public void TakeDamage(int _DamageAmount)
    {
        CurrentHealth -= _DamageAmount;

        Debug.Log(transform.name + " now currentHealth " + CurrentHealth);
    }

}
