
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class PlayerSetUp : NetworkBehaviour //No monobehaviour
{
    [SerializeField]
    Behaviour[] ComponentToDisable;

    [SerializeField]
    string RemoteLayerName = "RemotePlayer";

    Camera SceneCamera;
    void Start()
    {
        //if this object are the local player
        if (!isLocalPlayer) {
            DisableComponents();
            AssignRemoteLayer();
        }
        //disable scene camera
        else
        {
            SceneCamera = Camera.main;
            if(SceneCamera != null)
            {
                SceneCamera.gameObject.SetActive(false);
            }
        }
        //RegisterPlayer(); game manager takes care of it
        
        //player class set up
        GetComponent<Player>().Setup();

        
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        string _NetID = GetComponent<NetworkIdentity>().netId.ToString();
        Player _Player = GetComponent<Player>();
        GameManager.RegisterPlayer(_NetID, _Player);
    }
    void AssignRemoteLayer()//assgin layer to players who are not local
    {
        gameObject.layer = LayerMask.NameToLayer(RemoteLayerName);
    }
    void DisableComponents()
    {
        for (int i = 0; i < ComponentToDisable.Length; i++)
        {
            ComponentToDisable[i].enabled = false;
        }
    }
    private void OnDisable()
    {
        //when this object die, set back scene camear back
        if(SceneCamera != null)
        {
            SceneCamera.gameObject.SetActive(true);
        }

        //deregisger player when die are killed / or disconnect?
        GameManager.UnRegisterPlayer(transform.name);
    }
}
