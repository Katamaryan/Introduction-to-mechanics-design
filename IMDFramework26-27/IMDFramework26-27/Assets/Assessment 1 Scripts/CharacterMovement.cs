using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CharacterMovement : MonoBehaviour
{
	private Rigidbody2D m_RB;

	[SerializeField] private float m_MoveSpeed;
	[SerializeField] private float m_JumpStrength;
	[SerializeField] private Transform m_RaycastPosition;
	[SerializeField] private LayerMask m_GroundLayer;
	private float m_InMove;
    private PlayerControls m_ActionMap;
    private bool m_IsGrounded;
    private bool m_isJumpInput;
    [SerializeField]  private float m_jumpBufferLength;
    [SerializeField] private float m_coyoteTimeLength;

    private void Awake()
    {
        m_ActionMap = new PlayerControls();

		m_RB = GetComponent<Rigidbody2D>();
    }

    #region Bindings
    private void OnEnable()
    {
        m_ActionMap.Enable();

        m_ActionMap.Default.MoveHoriz.performed += Handle_MovePerformed;
        m_ActionMap.Default.MoveHoriz.canceled += Handle_MoveCancelled;
        m_ActionMap.Default.Jump.performed += Handle_JumpPerformed;
    }

    private void OnDisable()
    {
        m_ActionMap.Disable();

        m_ActionMap.Default.MoveHoriz.performed -= Handle_MovePerformed;
        m_ActionMap.Default.MoveHoriz.canceled -= Handle_MoveCancelled;
        m_ActionMap.Default.Jump.performed -= Handle_JumpPerformed;
    }
    #endregion

    #region InputFunctions

    private void Handle_MovePerformed(InputAction.CallbackContext context)
    {
        m_InMove = context.ReadValue<float>();
    }
    private void Handle_MoveCancelled(InputAction.CallbackContext context)
    {
        m_InMove = 0f;
    }


    float m_JumpBufferWait;
    float m_coyoteTimer;


    private void Handle_JumpPerformed(InputAction.CallbackContext context)
    {
        m_isJumpInput = true;
    }
    #endregion


	private void FixedUpdate()
	{
		m_RB.linearVelocityX = m_MoveSpeed * m_InMove;

        m_IsGrounded = Physics2D.Raycast(m_RaycastPosition.position, Vector2.down, 0.1f, m_GroundLayer);

        if (m_IsGrounded)
        {
            m_coyoteTimer = Time.time;
        }
        
        switch(jumpState)
        {
            case JumpState.rising:
                if (m_RB.linearVelocityY < 0.3f)
                    jumpState = JumpState.apexing;
                break;
            case JumpState.apexing:
                m_RB.gravityScale = 0.3f;
                if (m_RB.linearVelocityY < -0.2f)
                {
                    m_RB.gravityScale = 1f;
                    jumpState = JumpState.falling;
                }
                break;
            case JumpState.falling:
                if (m_isJumpInput)
                {
                    m_JumpBufferWait = Time.time;
                    m_isJumpInput = false;
                }
                if (m_IsGrounded)
                    jumpState = JumpState.grounded;
                break;
            case JumpState.grounded:
                if (!m_IsGrounded && m_coyoteTimer + m_coyoteTimeLength < Time.time)
                {
                    jumpState = JumpState.falling;
                }
                if (m_isJumpInput)
                {
                    m_isJumpInput = false;
                    m_RB.AddForce(Vector2.up * m_JumpStrength, ForceMode2D.Impulse);
                    jumpState = JumpState.rising;
                }
                if (m_JumpBufferWait + m_jumpBufferLength > Time.time)
                {
                    m_isJumpInput = false;
                    m_RB.AddForce(Vector2.up * m_JumpStrength, ForceMode2D.Impulse);
                    jumpState = JumpState.rising;
                }
                break;
        }
                Debug.Log(jumpState);

	}

    enum JumpState
    {
        rising,
        apexing,
        falling,
        grounded
    }
    JumpState jumpState = JumpState.grounded;
    
}
