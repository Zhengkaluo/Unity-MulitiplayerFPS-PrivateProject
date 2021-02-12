
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class PlayerSetUp : NetworkBehaviour //No monobehaviour
{
    [SerializeField]
    Behaviour[] ComponentToDisable;

    [SerializeField]
    string RemoteLayerName = "RemotePlayer";

    [SerializeField]
    string DontDrawLayerName = "DontRraw";
    [SerializeField]
    GameObject PlayerGraphics;

    [SerializeField]
    GameObject PlayerUIPrefab;
    private GameObject PlayerUIInstance;

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
            //Disable Player Graphics For Local Player so not blocking camera
            SetLayerRecursively(PlayerGraphics, LayerMask.NameToLayer(DontDrawLayerName));

            //Create PlayerUI
            PlayerUIInstance = Instantiate(PlayerUIPrefab);
            PlayerUIInstance.name = PlayerUIPrefab.name;
        }


        //RegisterPlayer(); game manager takes care of it

        //player class set up
        GetComponent<Player>().Setup();
   
    }
    void SetLayerRecursively(GameObject Obj, int NewLayer)
    {
        Obj.layer = NewLayer;
        foreach(Transform child in Obj.transform)//recursively on child 
        {
            SetLayerRecursively(child.gameObject, NewLayer);
        }
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
        //Destroy PlayerUI
        Destroy(PlayerUIInstance);
        //when this object die, set back scene camear back
        if(SceneCamera != null)
        {
            SceneCamera.gameObject.SetActive(true);
        }

        //deregisger player when die are killed / or disconnect?
        GameManager.UnRegisterPlayer(transform.name);
    }
}
