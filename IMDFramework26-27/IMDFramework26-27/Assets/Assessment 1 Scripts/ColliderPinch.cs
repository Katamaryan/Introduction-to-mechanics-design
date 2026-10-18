using System.Collections;
using UnityEngine;

public class ColliderPinch : MonoBehaviour
{
    [SerializeField] private CapsuleCollider2D m_playerCol;
    [SerializeField] private Vector2 m_verticalSizeNormal;
    [SerializeField] private Vector2 m_verticalSizePinch;

    private bool m_isJumpActive;
    private bool m_isHeadBumped = false;
    [SerializeField] private float m_headBumpLength;
    private float m_headBumpTimer;
    private Coroutine m_CPinch;
    private Coroutine m_COverlapCheck;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        m_isHeadBumped = true;
        m_headBumpTimer = Time.time;
        m_CPinch = StartCoroutine(C_ColliderPinchUpdate());
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        m_isHeadBumped = false;
        m_headBumpTimer = 0f;
        m_playerCol.size = m_verticalSizeNormal;
    }

    public void ColliderSizeChange(int JumpState)
    {
        //0 = rising, 1 = apexing, 2 = falling, 3 = grounded

        if (JumpState == 0)
        {
            m_playerCol.size = m_verticalSizePinch;

            if (m_isJumpActive) { return; }
                m_isJumpActive = true;
                m_COverlapCheck = StartCoroutine(C_OverlapCheck());
            
        }

        if (JumpState == 1 || JumpState == 3)
        { 
            m_playerCol.size = m_verticalSizeNormal;
            m_isJumpActive = false;
            m_isHeadBumped = false;
        }
    }

    IEnumerator C_OverlapCheck()
    {
        while (m_isJumpActive)
        {
            if (Physics2D.OverlapCapsule(transform.position, m_verticalSizeNormal, CapsuleDirection2D.Vertical, 0f))
            {
                Debug.Log("i am overlapping");
                m_headBumpTimer = Time.time;
                m_isHeadBumped = true;
                m_CPinch = StartCoroutine(C_ColliderPinchUpdate());
                m_isJumpActive = false; 
            }
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator C_ColliderPinchUpdate()
    {
        while (m_isHeadBumped)
        {

            if (m_headBumpTimer + m_headBumpLength < Time.time)
            {
                m_playerCol.size = m_verticalSizeNormal;
                m_isHeadBumped = false;
            }

            yield return new WaitForFixedUpdate();

        }
    }
        
}
