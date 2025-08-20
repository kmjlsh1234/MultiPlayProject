using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public int playerId;

    #region ::::PlayerController Property
    [Header("Player")]
    [Tooltip("Move speed of the character in m/s")]
    public float MoveSpeed = 2.0f;

    [Tooltip("Sprint speed of the character in m/s")]
    public float SprintSpeed = 5.335f;

    [Tooltip("How fast the character turns to face movement direction")]
    [Range(0.0f, 0.3f)]
    public float RotationSmoothTime = 0.12f;

    [Tooltip("Acceleration and deceleration")]
    public float SpeedChangeRate = 10.0f;

    public AudioClip LandingAudioClip;
    public AudioClip[] FootstepAudioClips;
    [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

    [Space(10)]
    [Tooltip("The height the player can jump")]
    public float JumpHeight = 1.2f;

    [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
    public float Gravity = -15.0f;

    [Space(10)]
    [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
    public float JumpTimeout = 0.50f;

    [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
    public float FallTimeout = 0.15f;

    [Header("Player Grounded")]
    [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
    public bool Grounded = true;

    [Tooltip("Useful for rough ground")]
    public float GroundedOffset = -0.14f;

    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    public float GroundedRadius = 0.28f;

    [Tooltip("What layers the character uses as ground")]
    public LayerMask GroundLayers;

    /*
    [Header("Cinemachine")]
    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    public GameObject CinemachineCameraTarget;
    */
    [Tooltip("How far in degrees can you move the camera up")]
    public float TopClamp = 70.0f;

    [Tooltip("How far in degrees can you move the camera down")]
    public float BottomClamp = -30.0f;

    [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    public float CameraAngleOverride = 0.0f;

    [Tooltip("For locking the camera position on all axis")]
    public bool LockCameraPosition = false;

    // player
    protected float _speed;
    protected float _animationBlend;
    protected float _targetRotation = 0.0f;
    protected float _rotationVelocity;
    protected float _verticalVelocity;
    protected float _terminalVelocity = 53.0f;

    // animation IDs
    protected int _animIDSpeed;
    protected int _animIDGrounded;
    protected int _animIDJump;
    protected int _animIDFreeFall;
    protected int _animIDMotionSpeed;

#if ENABLE_INPUT_SYSTEM
    protected PlayerInput _playerInput;
#endif
    protected Animator _animator;
    protected CharacterController _controller;
    protected StarterAssetsInputs _input;
    

    protected const float _threshold = 0.01f;

    protected bool _hasAnimator;

    #endregion

    protected virtual void Start()
    {
        gameObject.tag = "Untagged";
        _hasAnimator = TryGetComponent(out _animator);
        _controller = GetComponent<CharacterController>();
        _input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM
        _playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

        AssignAnimationIDs();
    }

    private void AssignAnimationIDs()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDGrounded = Animator.StringToHash("Grounded");
        _animIDJump = Animator.StringToHash("Jump");
        _animIDFreeFall = Animator.StringToHash("FreeFall");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
    }

    private void OnDrawGizmosSelected()
    {
        
    }

    private void OnFootstep(AnimationEvent animationEvent)
    {
        
    }

    private void OnLand(AnimationEvent animationEvent)
    {
        
    }

    private Vector3 targetPos;
    private Quaternion targetRot;
    private float moveSpeed = 10f;  // 위치 보간 속도
    private float rotSpeed = 10f;


    public void RecvPacket(S_BroadCast_MovePacket packet)
    {
        targetPos = new Vector3(packet.posX, packet.posY, packet.posZ);
        targetRot = Quaternion.Euler(0f, packet.rotY, 0f);

        Debug.Log($"Client {packet.playerId} TargetPos : [{targetPos.x},{targetPos.y},{targetPos.z}]");
    }

    // 보간 속도
    private float moveLerpSpeed = 3f;
    private float rotLerpSpeed = 3f;

    private void FixedUpdate()
    {
        //transform.position = Vector3.Lerp(transform.position, targetPos, Time.fixedDeltaTime * moveSpeed);
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.fixedDeltaTime * rotSpeed);
        // 위치 보간
        Vector3 newPos = Vector3.Lerp(transform.position, targetPos, Time.fixedDeltaTime * moveLerpSpeed);
        Vector3 moveDir = newPos - transform.position;

        // CharacterController로 이동
        _controller.Move(moveDir);

        // 회전 보간
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.fixedDeltaTime * rotLerpSpeed);

        // 애니메이션 반영
        if (_hasAnimator)
        {
            // 속도 계산
            _speed = moveDir.magnitude / Time.fixedDeltaTime;
            _animationBlend = Mathf.Lerp(_animationBlend, _speed, Time.fixedDeltaTime * SpeedChangeRate);

            _animator.SetFloat(_animIDSpeed, _animationBlend);
            _animator.SetFloat(_animIDMotionSpeed, 1f);
        }
    }
}
