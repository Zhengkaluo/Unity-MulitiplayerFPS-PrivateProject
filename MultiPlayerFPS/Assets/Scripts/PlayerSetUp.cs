
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerController))]
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
    [HideInInspector]
    public GameObject PlayerUIInstance;

    //Camera SceneCamera;
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
            
            //Disable Player Graphics For Local Player so not blocking camera
            SetLayerRecursively(PlayerGraphics, LayerMask.NameToLayer(DontDrawLayerName));

            //Create PlayerUI
            PlayerUIInstance = Instantiate(PlayerUIPrefab);
            PlayerUIInstance.name = PlayerUIPrefab.name;

            //config PlayerUI
            PlayerUI UI = PlayerUIInstance.GetComponent<PlayerUI>();
            if(UI == null)
            {
                Debug.LogError("No player UI found");
            }
            UI.SetController(GetComponent<PlayerController>());
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
        //instance?
        GameManager.Instance.SetSceneCameraActive(true);

        //deregisger player when die are killed / or disconnect?
        GameManager.UnRegisterPlayer(transform.name);
    }
}
