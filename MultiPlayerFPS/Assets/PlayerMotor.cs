
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    private Vector3 Velocity = Vector3.zero;

    private Rigidbody PlayerRigidBody;

    private void Start()
    {
        PlayerRigidBody = GetComponent<Rigidbody>();        
    }


    //getting vector3 from PlayerController script
    public void Move(Vector3 _Velocity)
    {
        Velocity = _Velocity;
    }
    //
    private void FixedUpdate()
    {
        PerformMovement();  
    }
    //PerformMovement based on Velocity variable
    void PerformMovement()
    {
        if(Velocity != Vector3.zero)//check for meanless move
        {//calculate the destination based on the current position plus the new vector3
            PlayerRigidBody.MovePosition(PlayerRigidBody.position + Velocity * Time.fixedDeltaTime);
        }
    }

}
