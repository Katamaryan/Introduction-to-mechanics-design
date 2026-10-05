using Unity.VisualScripting;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private CharacterMovement m_CharacterMovement;
    [SerializeField] private Transform m_RaycastPosition;
    [SerializeField] private LayerMask m_GroundLayer;
    private bool m_isGrounded = false;
    private Vector2 m_boxCastSize = new Vector2(.5f, .1f);

    private void Awake()
    {
        m_CharacterMovement = gameObject.GetComponentInParent<CharacterMovement>();
        m_RaycastPosition = gameObject.transform;
    }

    private void FixedUpdate()
    {
        m_isGrounded = Physics2D.BoxCast(m_RaycastPosition.position, m_boxCastSize, 0f, Vector2.down, 0.1f, m_GroundLayer);
        m_CharacterMovement.m_IsGrounded = m_isGrounded;
    }


}
