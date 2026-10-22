using System;
using Unity.VisualScripting;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public event Action<bool, bool> onGrounded;
    //[SerializeField] private CharacterMovement m_CharacterMovement;
    [SerializeField] private Transform m_RaycastPosition;
    
    [SerializeField] private LayerMask m_GroundLayer;
    [SerializeField] private LayerMask m_PlatformLayer;
    public bool m_isGrounded = false;
    public bool m_isOnPlatform = false;
    [SerializeField] private Vector2 m_boxCastSize;

    private void Awake()
    {
        //m_CharacterMovement = gameObject.GetComponentInParent<CharacterMovement>();
        m_RaycastPosition = gameObject.transform;
    }

    private void FixedUpdate()
    {
        m_isGrounded = Physics2D.BoxCast(m_RaycastPosition.position, m_boxCastSize, 0f, Vector2.down, 0.1f, m_GroundLayer);
        m_isOnPlatform = Physics2D.BoxCast(m_RaycastPosition.position, m_boxCastSize, 0f, Vector2.down, 0.1f, m_PlatformLayer);

        onGrounded?.Invoke(m_isGrounded, m_isOnPlatform);
        //if (m_isGrounded || m_isOnPlatform)
        //{
        //}
        //m_CharacterMovement.m_IsGrounded = m_isGrounded;
        ////constantly sets coyote time while grounded
        //if (m_CharacterMovement.m_IsGrounded)
        //    m_CharacterMovement.m_coyoteTimer = Time.time;
    }


}
