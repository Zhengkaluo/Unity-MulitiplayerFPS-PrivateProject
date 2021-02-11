using UnityEngine;

[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]//will be shown in the inspector even though it is private
    private float speed = 5f;

    [SerializeField]
    private float LookSensitivity = 3f;

    [SerializeField]
    private float ThrusterForce = 1000f;

    [SerializeField]
    private Animator ThrusterAnimator;

    [Header("Spring Settings: ")]
    //[SerializeField]
    //private JointDriveMode JointMode;
    [SerializeField]
    private float JointSpring = 20f;
    [SerializeField]
    private float JointMaxForce = 40f;

    private PlayerMotor Motor;
    private ConfigurableJoint Joint;

    

   
    private void Start()
    {
        Motor = GetComponent<PlayerMotor>();//dont need to check because we require it before
        Joint = GetComponent<ConfigurableJoint>();
        SetJointSettings(JointSpring);
    }
    void Update()
    {
        //Calculate movement velocity as a 3d Vector 
        float _xMove = Input.GetAxis("Horizontal");//GetAxis will be differenet and 'slower'
        float _zMove = Input.GetAxis("Vertical");

        Vector3 _MoveHorizontal = transform.right * _xMove; //1,0,0 
        Vector3 _MoveVertical = transform.forward * _zMove; //0,0,1

        //final Movement Vector
        Vector3 _Velocity = (_MoveHorizontal + _MoveVertical) * speed;

        //Animate Movement
        if(ThrusterAnimator == null)
        {
            Debug.Log("No Thruster Animator found!");
        }
        else
        {
            ThrusterAnimator.SetFloat("ForwardVelocity", _zMove);
        }
        //apply movement
        Motor.Move(_Velocity);

        //calculate rotation from mouse. turning around character
        float _yRotation = Input.GetAxisRaw("Mouse X");//right and left effected by mouse to character, up and right effected camera

        //character rotation
        Vector3 _Rotation = new Vector3(0f, _yRotation, 0f) * LookSensitivity;
        Motor.Rotate(_Rotation);

        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _CameratRotaion = _xRotation * LookSensitivity;
        Motor.RotateCamera(_CameratRotaion);

        Vector3 _ThrusterForce = Vector3.zero;
        if (Input.GetButton("Jump"))
        {
            _ThrusterForce = Vector3.up * ThrusterForce;
            SetJointSettings(0f);
        }
        else
        {
            SetJointSettings(JointSpring);
        }
        //apply the thruster force
        Motor.ApplyThruster(_ThrusterForce);
    }
    private void SetJointSettings(float _jointSpring)
    {
        Joint.yDrive = new JointDrive
        {
            //mode = jointMode, //remove this here
            positionSpring = _jointSpring,
            maximumForce = JointMaxForce
        };
    }

}
