using Unity.VisualScripting;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    struct S_CamBorder
    {
        //x = bottom, y = mid point, z = top
        Vector3 border1;
        Vector3 border2;
        Vector3 border3;
    }

    public Transform m_target;
    [SerializeField] private Vector3 m_tPos;
    [SerializeField] private Vector3 m_tempVector;
    [SerializeField] private float m_camHeight;
    [SerializeField] private float m_magnitude;

    //x = bottom, y = midpoint, z = top
    [SerializeField] Vector3 border1;
    [SerializeField] Vector3 border2;
    [SerializeField] Vector3 border3;

    [SerializeField] Vector3 m_movingBorder;

    [SerializeField] private int m_currentBorder;
    [SerializeField] private bool m_camLocked;
    

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        m_tPos = m_target.position;
        //m_tPos.y = 4;
        m_magnitude = Vector3.Distance(transform.position, m_target.position);
        //if (m_target.position.y > m_tPos.y)
        //{
        //    m_tPos = Vector3.Lerp(m_tPos, m_target.position, m_magnitude);
        //}
        //if (m_target.position.y < m_tPos.y - m_camHeight)
        //{

        //    m_tempVector = m_target.position;
        //    m_tempVector.y = m_tempVector.y + m_camHeight;

        //    m_tPos = Vector3.Lerp(m_tPos, m_tempVector, m_magnitude);
        //}
        //m_tPos.y = Mathf.Clamp(m_tPos.y, -2, 10);

        //transform.position = Vector3.Lerp(transform.position, m_tPos, 10);



        m_tempVector = m_target.position;
        m_tempVector.y = m_movingBorder.y;
        MovingBorder();

        //BorderCheck();
        

        transform.position = Vector3.Lerp(transform.position, m_tempVector, m_magnitude);
    }

    private void MovingBorder()
    {
        if (m_target.position.y > m_movingBorder.z)
        {
            float distance = m_target.position.y - m_movingBorder.z;

            m_movingBorder.x = m_movingBorder.x + distance;
            m_movingBorder.y = m_movingBorder.y + distance;
            m_movingBorder.z = m_movingBorder.z + distance;
        }
        if (m_target.position.y < m_movingBorder.x)
        {
            float distance = m_movingBorder.x - m_target.position.y;

            m_movingBorder.x = m_movingBorder.x - distance;
            m_movingBorder.y = m_movingBorder.y - distance;
            m_movingBorder.z = m_movingBorder.z - distance;
        }
    }

    private void BorderCheck()
    {
        if (m_target.position.y > border1.x && m_target.position.y < border1.z)
        {
            if (m_currentBorder != 1 && m_camLocked)
            {
                m_camLocked = false;
            }
            m_currentBorder = 1;
            m_tempVector = m_target.position;

            //if (m_target.position.y < m_tempVector.y + .25f && m_target.position.y > m_tempVector.y - .25f)
            //{
            //    m_camLocked = false;
            //    m_tempVector = m_target.position;
            //    return;
            //}

            if (m_camLocked)
            {
                m_tempVector.y = m_target.position.y + m_camHeight;
                m_tempVector.y = Mathf.Clamp(m_tempVector.y, border1.y, border1.y);
                return;
            }

            if (transform.position.y < border1.y + .5f && transform.position.y > border1.y - .5f)
            {
                m_camLocked = true;

            }
        }

        if (m_target.position.y > border2.x && m_target.position.y < border2.z)
        {
            if (m_currentBorder != 2 && m_camLocked)
            {
                m_camLocked = false;
            }
            m_currentBorder = 2;
            m_tempVector = m_target.position;

            //if (m_target.position.y < m_tempVector.y + .5f && m_target.position.y > m_tempVector.y - .5f)
            //{
            //    Debug.Log("im hitting threshold");
            //    m_camLocked = false;
            //    m_tempVector.y = m_target.position.y;
            //    return;
            //}

            if (m_camLocked)
            {
                m_tempVector.y = m_target.position.y + m_camHeight;
                m_tempVector.y = Mathf.Clamp(m_tempVector.y, border2.y, border2.y);
                return;
            }

            if (m_tempVector.y < border2.y + .5f && m_tempVector.y > border2.y - .5f)
            {
                m_camLocked = true;

            }
        }

        if (m_target.position.y > border3.x && m_target.position.y < border3.z)
        {
            if (m_currentBorder != 3 && m_camLocked)
            {
                m_camLocked = false;
            }
            m_currentBorder = 3;
            m_tempVector = m_target.position;

            //if (m_target.position.y < m_tempVector.y + .25f && m_target.position.y > m_tempVector.y - .25f)
            //{
            //    m_camLocked = false;
            //    m_tempVector.y = m_target.position.y;
            //    return;
            //}

            if (m_camLocked)
            {
                m_tempVector.y = m_target.position.y + m_camHeight;
                m_tempVector.y = Mathf.Clamp(m_tempVector.y, border3.y, border3.y);
                return;
            }

            if (transform.position.y < border3.y + .5f && transform.position.y > border3.y - .5f)
            {
                m_camLocked = true;

            }
        }
    }
}
