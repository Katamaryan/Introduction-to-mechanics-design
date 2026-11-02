using Unity.VisualScripting;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public Transform m_target;
    [SerializeField] private Vector3 m_tempVector;
    [SerializeField] private float m_magnitude;
    [SerializeField] private float m_OffsetX;

    //x = bottom, y = midpoint, z = top
   
    [SerializeField] Vector3 m_movingBorder;
    [SerializeField] private bool m_camLocked;
    [SerializeField] private float m_topBorderDist;
    [SerializeField] private float m_botBorderDist;


    private void Start()
    {
        
    }

    private void LateUpdate()
    {
        m_magnitude = Vector3.Distance(transform.position, m_target.position);
       

        m_tempVector = new Vector3(m_target.position.x + m_OffsetX, m_target.position.y, m_target.position.z);
        MovingVerticalBorder();
        m_tempVector.y = m_movingBorder.y;


        transform.position = Vector3.Lerp(transform.position, m_tempVector, m_magnitude * Time.deltaTime);
    }

    private void MovingVerticalBorder()
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
        m_movingBorder.y = Mathf.Clamp(m_movingBorder.y, -2, 10);
        m_movingBorder.x = m_movingBorder.y - m_botBorderDist;
        m_movingBorder.z = m_movingBorder.y + m_topBorderDist;
    }

}
