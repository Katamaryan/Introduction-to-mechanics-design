using Unity.Properties;
using UnityEngine;

public class TimerWrapper : MonoBehaviour
{
    [CreateProperty]
    public float currentTime { get { return GetComponent<TimeManager>().m_currentTime; } }
    [CreateProperty]
    public float highScore { get { return GetComponent<TimeManager>().m_highScore; } }
}
