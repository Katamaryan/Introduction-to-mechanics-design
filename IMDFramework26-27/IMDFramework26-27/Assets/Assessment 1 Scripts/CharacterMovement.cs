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
    private bool m_IsGrounded;
    private bool m_isJumpInput;
    [SerializeField]  private float m_jumpBufferLength;
    [SerializeField] private float m_coyoteTimeLength;

    private void Awake()
    {
		m_RB = GetComponent<Rigidbody2D>();
    }

    #region InputFunctions

    public void MovePerformed(float direction)
    {
        m_InMove = direction;
    }
    public void MoveCancelled()
    {
        m_InMove = 0f;
    }


    float m_jumpBufferWait;
    float m_coyoteTimer;


    public void JumpPerformed()
    {
        m_isJumpInput = true;
        //Debug.Log(m_isJumpInput);   
    }

    public void JumpCanceled()
    {
        m_isJumpInput = false;
        //Debug.Log(m_isJumpInput);
    }

    #endregion


	private void FixedUpdate()
	{
		m_RB.linearVelocityX = m_MoveSpeed * m_InMove;

        m_IsGrounded = Physics2D.Raycast(m_RaycastPosition.position, Vector2.down, 0.1f, m_GroundLayer);


        switch (jumpState)
        {
            case JumpState.rising:
                if (m_RB.linearVelocityY < 0.3f)
                {
                    jumpState = JumpState.apexing;
                    break;
                }
                if (!m_isJumpInput)
                {
                    m_RB.linearVelocityY = -1f;
                    jumpState = JumpState.falling;
                }
                break;
            case JumpState.apexing:
                m_isJumpInput = false;
                m_RB.gravityScale = 0.3f;
                if (m_RB.linearVelocityY < -0.2f)
                {
                    m_RB.gravityScale = 1f;
                    jumpState = JumpState.falling;
                }
                break;
            case JumpState.falling:
                if (m_isJumpInput && m_jumpBufferWait == 0f)
                {
                    m_jumpBufferWait = Time.time;
                    Debug.Log(m_jumpBufferWait);
                }
                if ((m_isJumpInput && (m_jumpBufferWait + m_jumpBufferLength < Time.time)) || !m_isJumpInput)
                { 
                    m_isJumpInput = false;
                    m_jumpBufferWait = 0f;
                }
                if (m_IsGrounded)
                    jumpState = JumpState.grounded;
                break;
            case JumpState.grounded:
                if (m_IsGrounded)
                    m_coyoteTimer = Time.time;
                if (!m_IsGrounded && m_coyoteTimer + m_coyoteTimeLength < Time.time)
                    jumpState = JumpState.falling;
                if (m_isJumpInput && !(m_jumpBufferWait + m_jumpBufferLength > Time.time))
                {
                    Debug.Log("Attempting jump");
                    m_RB.AddForce(Vector2.up * m_JumpStrength, ForceMode2D.Impulse);
                    m_jumpBufferWait = 0f;
                    jumpState = JumpState.rising;
                }

                if (m_jumpBufferWait + m_jumpBufferLength > Time.time)
                {
                    Debug.Log("Attempting jump");
                    m_RB.AddForce(Vector2.up * m_JumpStrength, ForceMode2D.Impulse);
                    m_jumpBufferWait = 0f;
                    jumpState = JumpState.rising;
                }

                break;
        }
        //Debug.Log(jumpState);
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
