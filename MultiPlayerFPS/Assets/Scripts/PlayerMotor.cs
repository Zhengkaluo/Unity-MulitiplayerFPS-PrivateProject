
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    [SerializeField]
    private Camera PlayerCamera;
    private Vector3 Velocity = Vector3.zero;
    private Vector3 Rotation = Vector3.zero;
    private float CamearRotationX = 0f;
    private float CurrentCameraRotationX = 0f;
    private Rigidbody PlayerRigidBody;
    private Vector3 ThursterForce = Vector3.zero;

    [SerializeField]
    private float CameraRotationLimit = 85f;

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
    public void RotateCamera(float _CameraRotationX)
    {
        CamearRotationX = _CameraRotationX;
    }
    public void ApplyThruster(Vector3 _ThrusterForce)
    {
        ThursterForce = _ThrusterForce;
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
        if(ThursterForce != Vector3.zero)
        {
            PlayerRigidBody.AddForce(ThursterForce * Time.fixedDeltaTime, ForceMode.Acceleration);
        }
    }
    void PerformRotation()
    {
        PlayerRigidBody.MoveRotation(PlayerRigidBody.rotation * Quaternion.Euler(Rotation));//use quaternion
        if(PlayerCamera != null)
        {
            //PlayerCamera.transform.Rotate(-CamearRotation);
            //set rotation and clamp
            CurrentCameraRotationX -= CamearRotationX;
            CurrentCameraRotationX = Mathf.Clamp(CurrentCameraRotationX, -CameraRotationLimit, CameraRotationLimit); //-85 -> 85
            //apply rotation to camera
            PlayerCamera.transform.localEulerAngles = new Vector3(CurrentCameraRotationX, 0f, 0f); 
        }
    }


}
