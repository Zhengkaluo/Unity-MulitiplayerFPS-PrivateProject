using UnityEngine;
[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]//will be shown in the inspector even though it is private
    private float speed = 5f;

    private PlayerMotor Motor;

    private void Start()
    {
        Motor = GetComponent<PlayerMotor>();//dont need to check because we require it before

    }
    void Update()
    {
        //Calculate movement velocity as a 3d Vector 
        float _xMove = Input.GetAxisRaw("Horizontal");//GetAxis will be differenet and 'slower'
        float _zMove = Input.GetAxisRaw("Vertical");

        Vector3 _MoveHorizontal = transform.right * _xMove; //1,0,0 
        Vector3 _MoveVertical = transform.forward * _zMove; //0,0,1

        //final Movement Vector
        Vector3 _Velocity = (_MoveHorizontal + _MoveVertical).normalized * speed;
        //apply movement
        Motor.Move(_Velocity);
    }
}
