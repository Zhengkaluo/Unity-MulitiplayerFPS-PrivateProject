
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    RectTransform ThrusterFuelFillAmount;

    private PlayerController Controller;
    public void SetController(PlayerController _Controller)
    {
        Controller = _Controller;
    }
    void SetFuelAmount(float _Amount)
    {
        ThrusterFuelFillAmount.localScale = new Vector3(1f, _Amount, 1f);
    }
    private void Update()
    {
        SetFuelAmount(Controller.GetThrusterFuelAmount());
    }
}
