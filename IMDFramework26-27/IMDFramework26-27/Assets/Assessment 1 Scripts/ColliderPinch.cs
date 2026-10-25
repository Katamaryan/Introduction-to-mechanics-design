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
    [SerializeField] private float m_verticalColliderResetBuffer;
    [SerializeField] private float m_horizontalColliderResetBuffer;
    private bool m_isJumpActive;
    private bool m_isVerticalPinch = false;
    private Coroutine m_COverlapCheck;



    public void Pinch(int jumpState, float velocityX)
    {
        //checks the player is rising in a jump while not moving much horizontally to activate vertical collider pinch ( bumped head correction)
        if (jumpState == 0 && (velocityX < 0.5f && velocityX > -0.5f))
        {
            m_isVerticalPinch = true;
            if (m_isJumpActive) { return; }
            m_isJumpActive = true;
            m_playerCol.size = m_verticalSizePinch;
            m_COverlapCheck = StartCoroutine(C_OverlapCheck());
            // collider = size 1 (maybe veritcal)
        }


        //checks the player is rising in a jump while moving horizontally to activate horizontal collider pinch
        if (jumpState == 0 && (velocityX > 0.5f ^ velocityX < -0.5f))
        {
            m_isVerticalPinch = false;

            // checks there isn't a ceiling immediately above the player when starting horizontal collider pinch to prevent head clipping in the ceiling and pushing player down
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


        // vertical collider pinch reset and horizontal pinch reset when landing on a full platform to stop toes clipping in the floor
        if (jumpState == 2 && (m_isVerticalPinch || Physics2D.Raycast(transform.position, Vector2.down, 1.5f, m_groundMask)))
        {
            m_isJumpActive = false;
            m_playerCol.size = m_colliderSizeNormal;
            return;
        }

        if (jumpState == 2 && (velocityX > 0.5f ^ velocityX < -0.5f))
        {
            m_isVerticalPinch = false;

            if (m_isJumpActive) { return; }
            m_isJumpActive = true;
            m_playerCol.size = m_horizontalSizePinch;
            m_COverlapCheck = StartCoroutine(C_OverlapCheck());
        }


        // horizontal collider pinch reset on correcting toe clipping
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
            //attempt to prevent the player from clipping into wall if jumping into a regular wall and not a head bump corner
            //if (Physics2D.Raycast(transform.position, Vector2.left, .5f, m_groundMask) || Physics2D.Raycast(transform.position, Vector2.right, .5f, m_groundMask))
            //{
            //    m_playerCol.size = m_colliderSizeNormal;
            //}

            yield return new WaitForSeconds(m_overlapCheckBuffer);
            if (Physics2D.OverlapCapsule(transform.position, m_colliderSizeNormal, CapsuleDirection2D.Vertical, 0f, m_groundMask))
            {
                //corrects player position on vertical collider pinch overlap
                if (m_isVerticalPinch)
                {
                    //Debug.Log("i am overlapping");
                    yield return new WaitForSeconds(m_verticalColliderResetBuffer);
                    m_playerCol.size = m_colliderSizeNormal;
                }
                //corrects player position on horizontal collider pinch overlap
                else
                {
                    yield return new WaitForSeconds(m_horizontalColliderResetBuffer);
                    m_playerCol.size = m_colliderSizeNormal;
                }
            }
            yield return new WaitForFixedUpdate();

        }
    }  

}
