using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CharacterMovement : MonoBehaviour
{
	private Rigidbody2D m_RB;

    public event Action<int, float> onJumpStateChange;

    [SerializeField] private float m_MoveSpeed;
	[SerializeField] private float m_JumpStrength;
    private float m_InMove;
    public bool m_IsGrounded;
    public bool m_IsOnPlatform;
    private bool m_InJump;
    [SerializeField]  private float m_jumpBufferLength;
    [SerializeField] private float m_coyoteTimeLength;
    [SerializeField] private float m_apexGravScale;
    [SerializeField] private float m_variableJumpStrength;
    private Coroutine m_CMove;
    private bool m_isMoveActive;
    private Coroutine m_CJump;
    private bool m_isJumpActive;


    private void Awake()
    {
		m_RB = GetComponent<Rigidbody2D>();
    }

    public void OnGrounded(bool isGrounded, bool isOnPlatform)
    {
        m_IsGrounded = isGrounded;
        m_IsOnPlatform = isOnPlatform;
        if (isOnPlatform && !isGrounded)
        {
            m_IsGrounded = isOnPlatform;
        }
        if(m_IsGrounded)
        {
            m_coyoteTimer = Time.time;
            return;
        }
        //if (m_isJumpActive) { return; }
        //{
        //    m_isJumpActive = true;
        //    onJumpStateChange?.Invoke(4, m_RB.linearVelocityX);
        //}
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
    public float m_coyoteTimer;


    public void JumpPerformed()
    {
        m_InJump = true;
        
        if (m_isJumpActive) { return; }
        m_isJumpActive = true;
        m_CJump = StartCoroutine(C_JumpUpdate());
    }

    public void JumpCanceled()
    {
        m_InJump = false;
    }

    //not in use atm, will implement later for relaxed semi solids
    public void DropPerformed()
    {
        //Physics2D.IgnoreCollision()
    }

    #endregion

    #region Coroutines

    IEnumerator C_MoveUpdate()
    {
        while (m_isMoveActive)
        {
            yield return new WaitForFixedUpdate();

            m_RB.linearVelocityX = m_MoveSpeed * m_InMove;
            //Debug.Log("the move coroutine is alive");
        }
    }

    IEnumerator C_JumpUpdate()
    {

        while (m_isJumpActive)
        {
            yield return new WaitForFixedUpdate();
            switch (jumpState)
            {
                case JumpState.rising:
                    
                    //tells other scripts what jump state the player is in
                    onJumpStateChange?.Invoke(0, m_RB.linearVelocityX);
                
                    //Checks when the apex of the jump has been reached 
                    if (m_RB.linearVelocityY < 0.3f)
                    {
                        //prevents the player from holding jump to automatically jump upon hitting the ground
                        m_InJump = false;
                        jumpState = JumpState.apexing;

                        
                        break;
                    }
                    //checks if the jump has been ended early from letting go of jump button in style of metroid from gba and earlier (havent played dread yet)
                    if (!m_InJump)
                    {
                        m_RB.linearVelocityY = m_variableJumpStrength;
                        jumpState = JumpState.falling;
                    }
                    break;
                case JumpState.apexing:

                    onJumpStateChange?.Invoke(1, m_RB.linearVelocityX);

                    //slows player while in apex until falling fast enough to be considerd falling
                    m_RB.gravityScale = m_apexGravScale;
                    if (m_RB.linearVelocityY < -0.2f)
                    {
                        m_RB.gravityScale = 1f;
                        jumpState = JumpState.falling;
                    }
                    break;
                case JumpState.falling:

                    onJumpStateChange?.Invoke(2, m_RB.linearVelocityX);

                    //if(m_RB.linearVelocityY <= -1f)
                    //{
                    //    m_RB.linearVelocityY = -1f;
                    //}
                        //m_RB.linearVelocityY = Mathf.Clamp(m_RB.linearVelocityY, -1f, 0f);

                    //checks if the player inputs jump and there is no active jump buffer to make a time stamp for jump buffer (prevents holding jump too early and still being able to jump 
                    if (m_InJump && m_jumpBufferWait == 0f)
                    {
                        m_jumpBufferWait = Time.time;
                        //Debug.Log(m_jumpBufferWait);
                    }
                    // checks if the jump is held too early and resets it and the jump buffer time stamp also allows for player to reset jump buffer if accidentally pressed too early and let go
                    if ((m_InJump && (m_jumpBufferWait + m_jumpBufferLength < Time.time)) || !m_InJump)
                    { 
                        m_InJump = false;
                        m_jumpBufferWait = 0f;
                    }
                    if (m_IsGrounded)
                    {
                        jumpState = JumpState.grounded;
                    }
                    Debug.Log(m_RB.linearVelocityY);
                    break;
                case JumpState.grounded:

                    onJumpStateChange?.Invoke(3, m_RB.linearVelocityX);

                    //changes state if not on ground and coyote time ran out
                    if (!m_IsGrounded && m_coyoteTimer + m_coyoteTimeLength < Time.time)
                    {
                        jumpState = JumpState.falling;
                        break;
                    }
                    // allows player to jump when on the ground (also has a check to prevent a perfectly timed super jump, could be fun idea for different project)
                    if (m_InJump && !(m_jumpBufferWait + m_jumpBufferLength > Time.time))
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
                    if(!m_InJump) 
                    {
                        m_isJumpActive = false;
                    }
                    break;
            }
            //Debug.Log("the jump coroutine is alive");
            //Debug.Log(jumpState);
        }
    }
    #endregion

    

    enum JumpState
    {
        //0 = rising, 1 = apexing, 2 = falling, 3 = grounded
        rising,
        apexing,
        falling,
        grounded
    
    }
    JumpState jumpState = JumpState.grounded;
    
}
