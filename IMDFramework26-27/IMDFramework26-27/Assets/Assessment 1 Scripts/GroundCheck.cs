using System;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public event Action<bool, bool> onGrounded;
    [SerializeField] private Transform m_playerTransform;
    [SerializeField] private LayerMask m_GroundLayer;
    [SerializeField] private LayerMask m_PlatformLayer;
    public bool m_isGrounded = false;
    public bool m_lastGroundedCheck = false;
    public bool m_isOnPlatform = false;
    public bool m_lastOnPlatformCheck = false;
    public bool m_leftFloorCheck = false;
    public bool m_rightFloorCheck = false;
    private float m_lastSafeCheck;
    public Vector3 m_safePosition;
    [SerializeField] private Vector2 m_boxCastSize;


    private void FixedUpdate()
    {
        m_isGrounded = Physics2D.OverlapBox(transform.position, m_boxCastSize, 0.0f, m_GroundLayer);
        m_isOnPlatform = Physics2D.OverlapBox(transform.position, m_boxCastSize, 0.0f, m_PlatformLayer);

        m_rightFloorCheck = Physics2D.Raycast(new Vector3(transform.position.x + 1, transform.position.y + 1, transform.position.z), Vector3.down, 1.1f, m_GroundLayer);
        m_leftFloorCheck = Physics2D.Raycast(new Vector3(transform.position.x - 1, transform.position.y + 1, transform.position.z), Vector3.down, 1.1f, m_GroundLayer);
        
        if (m_rightFloorCheck && m_leftFloorCheck && m_isGrounded && m_lastSafeCheck < Time.time)
        {
            m_lastSafeCheck = Time.time + 3;
            m_safePosition = m_playerTransform.position;
        }

        if (m_isGrounded != m_lastGroundedCheck || m_isOnPlatform != m_lastOnPlatformCheck)
            onGrounded?.Invoke(m_isGrounded, m_isOnPlatform);
        m_lastGroundedCheck = m_isGrounded;
        m_lastOnPlatformCheck = m_isOnPlatform;
    }


}
