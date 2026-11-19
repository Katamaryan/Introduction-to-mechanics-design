using System.Collections;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private bool m_lapActive = false;
    private int m_checkpointsReached;
    public float m_currentTime;
    public float m_highScore;
    [SerializeField] private GameObject m_startLine;
    [SerializeField] private GameObject[] m_checkpoints;
    [SerializeField] private GameObject m_endLine;
    private Coroutine m_CLapTimer;

    void Start()
    {
        m_startLine.GetComponent<Checkpoint>().OnPass += StartLine;
        m_endLine.GetComponent<Checkpoint>().OnPass += FinishLine;
        for (int i = 0; i < m_checkpoints.Length; i++)
        {
            m_checkpoints[i].GetComponent<Checkpoint>().OnPass += Checkpoint;
        }
    }

    private void StartLine()
    {
        if (m_lapActive) { return; }

        m_currentTime = 0;
        m_lapActive = true;
        m_CLapTimer = StartCoroutine(C_LapTimer());
        m_checkpoints[0].SetActive(true);
    }

    private void FinishLine()
    {
        if (!m_lapActive || !(m_checkpointsReached == m_checkpoints.Length)) { return; }

        m_lapActive = false;
        if (m_highScore > m_currentTime || m_highScore == 0)
        {
            m_highScore = m_currentTime;
        }
        m_checkpointsReached = 0;

        for (int i = 0; i < m_checkpoints.Length; i++)
        {
            m_checkpoints[i].SetActive(false);
        }
    }

    private void Checkpoint()
    {
        if (m_lapActive)
        {
            m_checkpoints[m_checkpointsReached].SetActive(false);
            m_checkpointsReached = m_checkpointsReached + 1;
            if (m_checkpointsReached < m_checkpoints.Length)
                m_checkpoints[m_checkpointsReached].SetActive(true);
        }
    }

    IEnumerator C_LapTimer()
    {
        while (m_lapActive)
        {
            

            yield return new WaitForFixedUpdate();

            m_currentTime += Time.deltaTime;
        }
    }
}
