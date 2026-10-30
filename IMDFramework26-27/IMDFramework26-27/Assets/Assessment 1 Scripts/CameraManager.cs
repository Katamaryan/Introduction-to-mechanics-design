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
    Vector3 border1;
    Vector3 border2;
    Vector3 border3;

    

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        m_tPos = m_target.position;
        m_tPos.y = 4;
        m_magnitude = Vector3.Distance(m_target.position, m_tPos);
        if (m_target.position.y > m_tPos.y)
        {
            m_tPos = Vector3.Lerp(m_tPos, m_target.position, m_magnitude);
        }
        if (m_target.position.y < m_tPos.y - m_camHeight)
        {

            m_tempVector = m_target.position;
            m_tempVector.y = m_tempVector.y + m_camHeight;

            m_tPos = Vector3.Lerp(m_tPos, m_tempVector, m_magnitude);
        }
        m_tPos.y = Mathf.Clamp(m_tPos.y, -2, 10);
        
        transform.position = Vector3.Lerp(transform.position, m_tPos, 10);
    }

}
