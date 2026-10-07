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
    private float m_InMove;
    public bool m_IsGrounded;
    private bool m_isJumpInput;
    [SerializeField]  private float m_jumpBufferLength;
    [SerializeField] private float m_coyoteTimeLength;
    private Coroutine m_CMove;
    private bool m_isMoveActive;

    private void Awake()
    {
		m_RB = GetComponent<Rigidbody2D>();
    }

    #region InputFunctions

    public void MovePerformed(float direction)
    {
        m_InMove = direction;
        
        if (m_InMove == 0)
        {
            m_isMoveActive = false;
        }

        else
        {
            if (m_isMoveActive) { return; }
            m_isMoveActive = true;
            m_CMove = StartCoroutine(C_MoveUpdate());
        }
    }
    public void SetInMove(float direction)
    {
        m_InMove = direction;

        if (m_InMove == 0)
        {
            m_isMoveActive = false;
        }
        else
        {
            if (m_isMoveActive) { return; }
            m_isMoveActive = true;
            m_CMove = StartCoroutine(C_MoveUpdate());
            
        }
    }


    private float m_jumpBufferWait;
    private float m_coyoteTimer;


    public void JumpPerformed()
    {
        m_isJumpInput = true;
    }

    public void JumpCanceled()
    {
        m_isJumpInput = false;
    }

    #endregion

    

	private void FixedUpdate()
	{

        switch (jumpState)
        {
            case JumpState.rising:
                //Checks when the apex of the jump has been reached 
                if (m_RB.linearVelocityY < 0.3f)
                {
                    //prevents the player from holding jump to automatically jump upon hitting the ground
                    m_isJumpInput = false;
                    jumpState = JumpState.apexing;
                    break;
                }
                //checks if the jump has been ended early from letting go of jump button in style of metroid from gba and earlier (havent played dread yet)
                if (!m_isJumpInput)
                {
                    m_RB.linearVelocityY = -0.5f;
                    jumpState = JumpState.falling;
                }
                break;
            case JumpState.apexing:
                //slows player while in apex until falling fast enough to be considerd falling
                m_RB.gravityScale = 0.3f;
                if (m_RB.linearVelocityY < -0.2f)
                {
                    m_RB.gravityScale = 1f;
                    jumpState = JumpState.falling;
                }
                break;
            case JumpState.falling:
                //checks if the player inputs jump and there is no active jump buffer to make a time stamp for jump buffer (prevents holding jump too early and still being able to jump 
                if (m_isJumpInput && m_jumpBufferWait == 0f)
                {
                    m_jumpBufferWait = Time.time;
                    Debug.Log(m_jumpBufferWait);
                }
                // checks if the jump is held too early and resets it and the jump buffer time stamp also allows for player to reset jump buffer if accidentally pressed too early and let go
                if ((m_isJumpInput && (m_jumpBufferWait + m_jumpBufferLength < Time.time)) || !m_isJumpInput)
                { 
                    m_isJumpInput = false;
                    m_jumpBufferWait = 0f;
                }
                if (m_IsGrounded)
                    jumpState = JumpState.grounded;
                break;
            case JumpState.grounded:
                //constantly sets coyote time while grounded
                if (m_IsGrounded)
                    m_coyoteTimer = Time.time;
                //changes state if not on ground and coyote time ran out
                if (!m_IsGrounded && m_coyoteTimer + m_coyoteTimeLength < Time.time)
                    jumpState = JumpState.falling;
                // allows player to jump when on the ground (also has a check to prevent a perfectly timed super jump, could be fun idea for different project)
                if (m_isJumpInput && !(m_jumpBufferWait + m_jumpBufferLength > Time.time))
                {
                    //Debug.Log("Attempting jump");
                    m_RB.AddForce(Vector2.up * m_JumpStrength, ForceMode2D.Impulse);
                    m_jumpBufferWait = 0f;
                    jumpState = JumpState.rising;
                }
                //allows jump when ground is hit during jump buffer timing 
                if (m_jumpBufferWait + m_jumpBufferLength > Time.time)
                {
                    //Debug.Log("Attempting jump");
                    m_RB.AddForce(Vector2.up * m_JumpStrength, ForceMode2D.Impulse);
                    m_jumpBufferWait = 0f;
                    jumpState = JumpState.rising;
                }

                break;
        }
        //Debug.Log(jumpState);
	}

    IEnumerator C_MoveUpdate()
    {
        while (m_isMoveActive)
        {
            yield return new WaitForFixedUpdate();

            m_RB.linearVelocityX = m_MoveSpeed * m_InMove;
        }
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
