using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private Transform m_CameraTransform;
    private Transform m_Transform;
    private CapsuleCollider m_CapsuleCollider;
    private Rigidbody m_Rigidbody;
    private Animator m_Animator;
    private AudioSource m_AudioSource;

    [Header("Input")]
    [SerializeField]
    private string m_ForwardInput = "Vertical";
    [SerializeField]
    private string m_HorizontalInput = "Horizontal";
    [SerializeField]
    private string m_AimInput = "Fire2";
    [SerializeField]
    public float SpeedMultiplier
    {
        get
        {
            return Input.GetButton("Change Speed") ? 2f: 1f;
        }
    }

    [Header("Movement")]
    [SerializeField]
    private float m_RotationSpeed = 10f;
    [SerializeField]
    private float m_GroundDampening = 0f;
    [SerializeField]
    private float m_StepOffset = 0.2f;
    [SerializeField]
    private float m_SlopeLimit = 45f;

    [SerializeField]
    private LayerMask m_GroundLayer = 1 << 0;

    private bool m_IsAiming;
    private bool m_IsGrounded;
    public bool m_IsMoving;
    private bool m_IsJumping;
    private bool m_IsSwimming;

    [HideInInspector]
    public Vector3 RawInput;
    public Vector3 RelativeInput
    {
        get
        {
            Vector3 input = Vector3.zero;
            input.x = this.RawInput.x * this.SpeedMultiplier;
            input.z = this.RawInput.z * this.SpeedMultiplier;
            return input;
        }
    }
    private Vector3 m_Velocity;
    public Vector3 Velocity
    {
        get
        {
            return this.m_Velocity;
        }
        set
        {
            this.m_Velocity = value;
        }
    }
    
    private Quaternion m_LookRotation;

    [SerializeField]
    private List<MotionState> m_Motions;
    public List<MotionState> Motions
    {
        get
        {
            return this.m_Motions;
        }
        set
        {
            this.m_Motions = value;
        }
    }
    public AnimatorStateInfo[] m_LayerStateMap;
    public Dictionary<int, MotionState[]> m_MotionStateMap;

    //public delegate void EventFunction(bool isEvent);
    //public static event EventFunction OnControllerGrounded;

    public bool IsAiming
    {
        get
        {
            return this.m_IsAiming;
        }
        set
        {
            if (this.m_IsAiming != value)
            {
                this.m_IsAiming = value;
                this.m_Animator.SetFloat("Yaw Input", 0f);
            }
        }
    }
    public bool IsGrounded
    {
        get
        {
            return this.m_IsGrounded;
        }
        set
        {
            if (this.m_IsGrounded != value)
            {
                this.m_IsGrounded = value;
                this.m_Animator.SetBool("IsGrounded", m_IsGrounded);
                //OnControllerGrounded(m_IsGrounded);
            }
        }
    }
    public bool IsMoving
    {
        get
        {
            IsMoving = RelativeInput.sqrMagnitude > 0f;
            return this.m_IsMoving;
        }
        set
        {
            if (this.m_IsMoving != value)
            {
                this.m_IsMoving = value;
                this.m_Animator.SetBool("Moving", m_IsMoving);
            }
        }
    }
    public bool IsJumping
    {
        get
        {
            return this.m_IsJumping;
        }
        set
        {
            if (this.m_IsJumping != value)
            {
                this.m_IsJumping = value;
                this.m_Animator.SetBool("Jumping", m_IsJumping);
            }
        }
    }
    public bool IsSwimming
    {
        get
        {
            return this.m_IsSwimming;
        }
        set
        {
            if (this.m_IsSwimming != value)
            {
                this.m_IsSwimming = value;
                this.m_Animator.SetBool("Swimming", m_IsSwimming);
            }
        }
    }
    public bool IsFalling
    {
        get
        {
            return !IsGrounded && !IsSwimming;
        }
    }
    public float Slope;
    public bool IsStepping;


    private void Awake()
    {
        this.m_CameraTransform = Camera.main.transform;
        this.m_Transform = transform;
        this.m_CapsuleCollider = GetComponent<CapsuleCollider>();
        this.m_Rigidbody = GetComponent<Rigidbody>();
        this.m_Animator = GetComponent<Animator>();
        
        for (int i = 0; i < this.m_Motions.Count; i++)
        {
            this.m_Motions[i].Index = i;
        }
        
        this.m_LayerStateMap = new AnimatorStateInfo[this.m_Animator.layerCount];
        this.m_MotionStateMap = new Dictionary<int, MotionState[]>();
        for (int j = 0; j < this.m_Animator.layerCount; j++)
        {
            AnimatorStateInfo stateInfo = this.m_Animator.GetCurrentAnimatorStateInfo(j);
            List<MotionState> states = new List<MotionState>();
            for (int k = 0; k < this.m_Motions.Count; k++)
            {
                if (m_Animator.HasState(j, Animator.StringToHash(this.m_Motions[k].State)))
                {
                    this.m_Motions[k].Layer = j;
                    states.Add(this.m_Motions[k]);
                }
            }
            this.m_MotionStateMap.Add(j, states.ToArray());
            this.m_LayerStateMap[j] = stateInfo;
        }
    }

    void Update()
    {
        this.RawInput = new Vector3(Input.GetAxis(this.m_HorizontalInput), 0, Input.GetAxis(this.m_ForwardInput));

        for (int j = 0; j < this.m_Motions.Count; j++)
        {
            MotionState motion = this.m_Motions[j];
            if (!motion.isActiveAndEnabled) continue;
            if (motion.m_StartType != MotionState.StartType.Down && motion.m_StopType != MotionState.StopType.Toggle || !Input.GetButtonDown(motion.m_InputName))
            {
                if (motion.m_StopType == MotionState.StopType.Up)
                {
                    this.TryStopMotion(motion);
                }
            }
            else if (!motion.IsActive && motion.m_StartType == MotionState.StartType.Down)
            {
                this.TryStartMotion(motion);
            }
            else if (motion.m_StopType == MotionState.StopType.Toggle)
            {
                this.TryStopMotion(motion);
                break;
            }
            if (motion.m_StartType == MotionState.StartType.Press && Input.GetButton(motion.m_InputName))
            {
                this.TryStartMotion(motion);
            }
        }
    }

    private void FixedUpdate() {
        for (int i = 0; i < this.m_Motions.Count; i++)
        {
            MotionState motion = this.m_Motions[i];
            if (!motion.isActiveAndEnabled) continue;
            if (!motion.IsActive && motion.m_StartType == MotionState.StartType.Automatic && motion.CanStart())
            {
                this.TryStartMotion(motion);
            }
            if (motion.IsActive && motion.m_StopType == MotionState.StopType.Automatic && motion.CanStop())
            {
                this.TryStopMotion(motion);
            }
        }

        this.m_LookRotation = Quaternion.Euler(this.m_Transform.eulerAngles.x, this.m_CameraTransform.eulerAngles.y, this.m_Transform.eulerAngles.z);
        this.m_Velocity = this.m_Rigidbody.velocity;
        this.CheckGround();
        this.UpdateVelocity();
        this.UpdateRotation();
        this.UpdateAnimator();
        this.m_Rigidbody.velocity = this.m_Velocity;
    }
    
    public void UpdateVelocity()
    {
        for (int i = 0; i < this.m_Motions.Count; i++)
        {
            MotionState motion = this.m_Motions[i];
            if (motion.IsActive && !motion.UpdateVelocity(ref this.m_Velocity)) return;
        }
    }

    private void UpdateRotation()
    {
        for (int i = 0; i < this.m_Motions.Count; i++)
        {
            MotionState motion = this.m_Motions[i];
            if (motion.IsActive && !motion.UpdateRotation()) return;
        }

        Quaternion rotation = this.m_Transform.rotation;
        rotation = Quaternion.LookRotation(this.m_LookRotation * this.RawInput);
        this.m_Transform.rotation = Quaternion.Slerp(this.m_Transform.rotation, rotation, this.m_RotationSpeed * Time.fixedDeltaTime);
    }

    private void UpdateAnimator()
    {
        for (int i = 0; i < this.m_Motions.Count; i++)
        {
            MotionState motion = this.m_Motions[i];
            if (motion.IsActive && !motion.UpdateAnimator()) return;
        }

        this.m_Animator.SetFloat("Forward Input", RelativeInput.z);
        this.m_Animator.SetFloat("Horizontal Input", RelativeInput.x);
    }

    public void CheckGround()
    {
        for (int i = 0; i < this.m_Motions.Count; i++)
        {
            MotionState motion = this.m_Motions[i];
            if (motion.IsActive && !motion.CheckGround()) return;
        }

        float distanceToPoints = this.m_CapsuleCollider.height * 0.5f - this.m_CapsuleCollider.radius;
        Vector3 point1 = this.m_Transform.position + this.m_CapsuleCollider.center + Vector3.up * distanceToPoints;
        Vector3 point2 = this.m_Transform.position + this.m_CapsuleCollider.center - Vector3.up * distanceToPoints;
        this.IsGrounded = (Physics.CapsuleCast(point1, point2, this.m_CapsuleCollider.radius, -this.m_Transform.up, this.m_CapsuleCollider.radius * 2f)) ? true : false;
    }

    public void CheckStep()
    {
        Vector3 velocity = this.m_Velocity;
        velocity.y = 0f;
        if (this.RelativeInput.sqrMagnitude > velocity.sqrMagnitude)
        {
            velocity = this.m_Transform.TransformDirection(RelativeInput);
        }

        RaycastHit hitInfo;
        bool prevSlope = this.Slope != -1f;
        this.Slope = -1f;
        this.IsStepping = false;

        if (velocity.sqrMagnitude > 0.001f && Physics.Raycast(this.m_Transform.position + this.m_Transform.up * 0.1f, velocity.normalized, out hitInfo, this.m_CapsuleCollider.radius + 0.2f, this.m_GroundLayer, QueryTriggerInteraction.Ignore))
        {

            float slope = Mathf.Acos(Mathf.Clamp(hitInfo.normal.y, -1f, 1f)) * Mathf.Rad2Deg;
            if (slope > this.m_SlopeLimit)
            {
                Vector3 direction = hitInfo.point - this.m_Transform.position;
                direction.y = 0f;
                Physics.Raycast((hitInfo.point + (Vector3.up * this.m_StepOffset)) + (direction.normalized * 0.1f), Vector3.down, out hitInfo, this.m_StepOffset + 0.1f, m_GroundLayer, QueryTriggerInteraction.Ignore);
                if (Mathf.Acos(Mathf.Clamp(hitInfo.normal.y, -1f, 1f)) * Mathf.Rad2Deg > this.m_SlopeLimit)
                {
                    this.m_Velocity.x *= this.m_GroundDampening;
                    this.m_Velocity.z *= this.m_GroundDampening;
                }
                else
                {
                    Vector3 position = this.m_Transform.position;
                    float y = position.y;
                    position.y = Mathf.MoveTowards(y, position.y + this.m_StepOffset, Time.deltaTime);
                    this.m_Transform.position = position;
                    this.m_Velocity.y = 0f;
                    this.IsStepping = true;
                }
            }
            else
            {
                this.Slope = slope;
                this.m_Velocity.y = 0f;
            }
        }
        if (prevSlope && this.Slope == -1f)
        {
            this.m_Velocity.y = 0f;
        }


    }

    private void TryStartMotion(MotionState motion)
    {
        if (!motion.IsActive && motion.CanStart())
        {
            motion.StartMotion();
        }
    }

    private void TryStopMotion(MotionState motion)
    {
        if (motion.IsActive)
        {
            motion.StopMotion(false);
        }
    }


    public float _volume;
    public void ChangeVolume(UnityEngine.UI.Slider slider)
    {
        _volume = slider.value;
    }
    private void PlayFootstepSound(AnimationEvent evt)
    {
        PlaySound(evt.objectReferenceParameter as AudioClip, evt.floatParameter);
    }
    private void PlaySound(AnimationEvent evt)
    {
        PlaySound(evt.objectReferenceParameter as AudioClip, evt.floatParameter);
    }
    private void PlaySound(AudioClip clip, float volume)
    {
        if (clip == null) { return; }

        if (this.m_AudioSource == null)
        {

            this.m_AudioSource = GetComponent<AudioSource>();
            if (this.m_AudioSource == null)
            {
                this.m_AudioSource = gameObject.AddComponent<AudioSource>();
            }

        }
        if (this.m_AudioSource != null)
        {
            this.m_AudioSource.PlayOneShot(clip, _volume);
        }
    }
}
