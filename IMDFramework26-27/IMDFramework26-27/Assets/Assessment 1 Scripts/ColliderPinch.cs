using System.Collections;
using UnityEngine;

public class ColliderPinch : MonoBehaviour
{
    [SerializeField] private BoxCollider2D m_playerCol;
    [SerializeField] private Rigidbody2D m_RB;
    [SerializeField] private Vector2 m_colliderSizeNormal;
    [SerializeField] private Vector2 m_verticalSizePinch;
    [SerializeField] private Vector2 m_horizontalSizePinch;
    [SerializeField] private LayerMask m_groundMask;
    [SerializeField] private float m_overlapCheckBuffer;
    [SerializeField] private float m_colliderResetBuffer;
    private bool m_isJumpActive;
    private bool m_isVerticalPinch = false;
    private Coroutine m_COverlapCheck;



    public void Pinch(int jumpState, float velocityX)
    {
        if (jumpState == 0 && (velocityX < 0.5f && velocityX > -0.5f))
        {
            m_isVerticalPinch = true;
            if (m_isJumpActive) { return; }
            m_isJumpActive = true;
            m_playerCol.size = m_verticalSizePinch;
            m_COverlapCheck = StartCoroutine(C_OverlapCheck());
            // collider = size 1 (maybe veritcal)
        }
        if (jumpState == 0 && (velocityX > 0.5f ^ velocityX < -0.5f))
        {
            m_isVerticalPinch = false;

            if (Physics2D.Raycast(transform.position, Vector2.up, 1.5f, m_groundMask))
            {
                m_playerCol.size = m_colliderSizeNormal;
            }

            if (m_isJumpActive) { return; }
            m_isJumpActive = true;
            m_playerCol.size = m_horizontalSizePinch;
            m_COverlapCheck = StartCoroutine(C_OverlapCheck());
            // collider = size 1 (maybe veritcal)
        }
        if (jumpState == 2 && (m_isVerticalPinch || Physics2D.Raycast(transform.position, Vector2.down, 1.5f, m_groundMask)))
        {
            m_isJumpActive = false;
            m_playerCol.size = m_colliderSizeNormal;
        }
        if (jumpState == 3)
        {
            m_isJumpActive = false;
            m_playerCol.size = m_colliderSizeNormal;
        }


    }


    IEnumerator C_OverlapCheck()
    {
        while (m_isJumpActive)
        {
            //if (Physics2D.Raycast(transform.position, Vector2.left, .5f, m_groundMask) || Physics2D.Raycast(transform.position, Vector2.right, .5f, m_groundMask))
            //{
            //    Debug.Log("poo");
            //    m_playerCol.size = m_colliderSizeNormal;
            //}

            yield return new WaitForSeconds(m_overlapCheckBuffer);
            if (Physics2D.OverlapCapsule(transform.position, m_colliderSizeNormal, CapsuleDirection2D.Vertical, 0f, m_groundMask))
            {
                if (m_isVerticalPinch)
                {
                    Debug.Log("i am overlapping");
                    yield return new WaitForSeconds(m_colliderResetBuffer);
                    m_playerCol.size = m_colliderSizeNormal;
                }
                else
                {
                    m_playerCol.size = m_colliderSizeNormal;
                }
            }
            yield return new WaitForFixedUpdate();

        }
    }  

}
