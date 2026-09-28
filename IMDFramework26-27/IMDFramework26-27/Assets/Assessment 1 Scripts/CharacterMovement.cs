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
    [SerializeField]  private float m_jumpBufferLength = 1;

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


    private void Handle_JumpPerformed(InputAction.CallbackContext context)
    {
        m_isJumpInput = true;
        if (m_IsGrounded)
        {
            m_RB.AddForce(Vector2.up * m_JumpStrength, ForceMode2D.Impulse);
            m_isJumpInput = false;
        }
        else// we are in the air
        {
            //save the current time
            //timeInAir = Time.time;
            m_JumpBufferWait = Time.time;
            Debug.Log(m_JumpBufferWait);
        }



        //else if (m_isJumpInput == 1)
        //{
        //    m_isJumpInput = ;

        //    while (m_JumpBufferWait > Time.deltaTime)
        //    {
        //        if (m_IsGrounded)
        //        {
        //            m_RB.AddForce(Vector2.up * m_JumpStrength, ForceMode2D.Impulse);
        //            break;
        //        }

        //    }

        //}
    }
    #endregion


	private void FixedUpdate()
	{
		m_RB.linearVelocityX = m_MoveSpeed * m_InMove;

        m_IsGrounded = Physics2D.Raycast(m_RaycastPosition.position, Vector2.down, 0.1f, m_GroundLayer);
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {

        //get the current time and see if its less than a threshold
        if (m_isJumpInput && m_IsGrounded && (m_JumpBufferWait + m_jumpBufferLength > Time.time))
        {
            m_RB.AddForce(Vector2.up * m_JumpStrength, ForceMode2D.Impulse);
            m_isJumpInput = false;
            Debug.Log(m_JumpBufferWait + m_jumpBufferLength);
            Debug.Log(Time.time);
        }
    }

    IEnumerator JumpBuffer()
    {

        while (true)
        {
            yield return null;
        }
    }

}
