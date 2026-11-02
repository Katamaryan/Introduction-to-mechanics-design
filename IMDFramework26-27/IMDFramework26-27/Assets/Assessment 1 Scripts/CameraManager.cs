using Unity.VisualScripting;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public Transform m_target;
    [SerializeField] private Vector3 m_tempVector;
    [SerializeField] private float m_magnitude;

    //x = bottom, y = midpoint, z = top
   
    [SerializeField] Vector3 m_movingBorder;
    [SerializeField] private bool m_camLocked;
    [SerializeField] private float m_topBorderDist;
    [SerializeField] private float m_botBorderDist;

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        m_magnitude = Vector3.Distance(transform.position, m_target.position);
       

        m_tempVector = m_target.position;
        m_tempVector.y = m_movingBorder.y;
        MovingBorder();


        transform.position = Vector3.Lerp(transform.position, m_tempVector, m_magnitude);
    }

    private void MovingBorder()
    {
        if (m_target.position.y > m_movingBorder.z)
        {
            float distance = m_target.position.y - m_movingBorder.z;

            m_movingBorder.y = m_movingBorder.y + distance;
            
        }
        if (m_target.position.y < m_movingBorder.x)
        {
            float distance = m_movingBorder.x - m_target.position.y;

            m_movingBorder.y = m_movingBorder.y - distance;
        }
        m_tempVector.y = Mathf.Clamp(m_tempVector.y, -2, 10);
        m_movingBorder.x = m_movingBorder.y - m_botBorderDist;
        m_movingBorder.z = m_movingBorder.y + m_topBorderDist;
    }

}
