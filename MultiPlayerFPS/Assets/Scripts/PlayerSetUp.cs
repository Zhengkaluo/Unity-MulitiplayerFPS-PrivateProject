
using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetUp : NetworkBehaviour //No monobehaviour
{
    [SerializeField]
    Behaviour[] ComponentToDisable;
    Camera SceneCamera;
    void Start()
    {
        //if this object are the local player
        if (!isLocalPlayer) { 
            for (int i = 0; i < ComponentToDisable.Length; i++)
            {
                ComponentToDisable[i].enabled = false;
            }
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
    }
    private void OnDisable()
    {
        //when this object die, set back scene camear back
        if(SceneCamera != null)
        {
            SceneCamera.gameObject.SetActive(true);
        }
    }
}
