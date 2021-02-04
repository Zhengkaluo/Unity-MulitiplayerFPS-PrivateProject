
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    [SerializeField]
    private Camera PlayerCamera;
    private Vector3 Velocity = Vector3.zero;
    private Vector3 Rotation = Vector3.zero;
    private Vector3 CamearRotation = Vector3.zero;
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
    public void Rotate(Vector3 _Rotate)
    {
        Rotation = _Rotate;
    }
    public void RotateCamera(Vector3 _CameraRotation)
    {
        CamearRotation = _CameraRotation;
    }
    private void FixedUpdate()
    {
        PerformRotation();
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
    void PerformRotation()
    {
        PlayerRigidBody.MoveRotation(PlayerRigidBody.rotation * Quaternion.Euler(Rotation));//use quaternion
        if(PlayerCamera != null)
        {
            PlayerCamera.transform.Rotate(-CamearRotation);
        }
    }


   
}
