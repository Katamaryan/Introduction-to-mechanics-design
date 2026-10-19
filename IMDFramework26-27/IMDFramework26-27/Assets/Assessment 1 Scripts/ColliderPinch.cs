using System.Collections;
using UnityEngine;

public class ColliderPinch : MonoBehaviour
{
    [SerializeField] private CapsuleCollider2D m_playerCol;
    [SerializeField] private Rigidbody2D m_RB;
    [SerializeField] private Vector2 m_verticalSizeNormal;
    [SerializeField] private Vector2 m_verticalSizePinch;
    [SerializeField] private LayerMask m_groundMask;
    [SerializeField] private float m_overlapCheckBuffer;
    [SerializeField] private float m_colliderResetBuffer;
    private bool m_isJumpActive;
    private Coroutine m_COverlapCheck;



    public void Pinch(int jumpState, float velocityX)
    {
        if (jumpState == 0 && velocityX < 0.5f)
        {
            if (m_isJumpActive) { return; }
            m_isJumpActive = true;
            m_playerCol.size = m_verticalSizePinch;
            m_COverlapCheck = StartCoroutine(C_OverlapCheck());
            // collider = size 1 (maybe veritcal)
        }
        if (jumpState == 2)
        {
            m_isJumpActive = false;
            m_playerCol.size = m_verticalSizeNormal;
        }

    }


    IEnumerator C_OverlapCheck()
    {
        while (m_isJumpActive)
        {
            yield return new WaitForSeconds(m_overlapCheckBuffer);
            if (Physics2D.OverlapCapsule(transform.position, m_verticalSizeNormal, CapsuleDirection2D.Vertical, 0f, m_groundMask))
            {
                Debug.Log("i am overlapping");
                yield return new WaitForSeconds(m_colliderResetBuffer);
                m_playerCol.size = m_verticalSizeNormal;
            }
            yield return new WaitForFixedUpdate();

        }
    }  

}
