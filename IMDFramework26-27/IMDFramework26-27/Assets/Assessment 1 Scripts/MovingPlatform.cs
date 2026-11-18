using Unity.VisualScripting;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private GameObject[] pathingPoints;
    private Rigidbody2D m_RB;
    private Vector3 m_distance; 
    private Vector3 m_lastPos;
    private int m_currentPoint;

    void Awake()
    {
        m_RB = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (m_currentPoint == pathingPoints.Length)
            m_currentPoint = 0;
        
        transform.position = Vector3.MoveTowards(transform.position, pathingPoints[m_currentPoint].transform.position, 1f * Time.deltaTime);
        
        m_distance = transform.position - m_lastPos;    
        m_lastPos = transform.position;
        
        
        if (transform.position == pathingPoints[m_currentPoint].transform.position)
            m_currentPoint += 1;
    }

    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    if (collision.transform.GetComponent<CharacterMovement>() != null )
    //    {
    //        collision.transform.position += m_distance;
    //    }
    //}
}
