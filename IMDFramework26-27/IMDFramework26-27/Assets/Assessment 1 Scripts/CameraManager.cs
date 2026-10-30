using Unity.VisualScripting;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform m_target;
    [SerializeField] private Vector3 m_tPos;
    [SerializeField] private Vector3 m_tempVector;
    [SerializeField] private float m_camHeight;
    [SerializeField] private float m_magnitude;

    private void Start()
    {
        m_camHeight = 3 - m_target.position.y;
    }

    private void FixedUpdate()
    {
        m_tPos = m_target.position;
        m_tPos.y = 3;
        m_magnitude = Vector3.Distance(m_target.position, m_tPos);
        if (m_target.position.y > m_tPos.y)
        {
            m_tPos = Vector3.Lerp(m_tPos, m_target.position, m_magnitude);
        }
        if (m_target.position.y < m_camHeight)
        {
            m_tPos = Vector3.Lerp(m_tPos, m_target.position, m_magnitude);
        }
        transform.position = Vector3.Lerp(transform.position, m_tPos, 10);
    }

}
