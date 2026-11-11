using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] m_playerSounds = new AudioClip[3];
    private AudioSource m_audioSource;

    public static AudioManager instance { get; private set; }    

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        m_audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// See top of movement class to see what sound each ID represents
    /// </summary>
    /// <param name="clipID"></param>
    public void PlayPlayerSound(int clipID)
    {
        if (clipID == 2)
            m_audioSource.loop = true;
        else
            m_audioSource.loop = false;
        m_audioSource.clip = m_playerSounds[clipID];
        m_audioSource.Play();
    }

    public void StopSound()
    {
        m_audioSource.Stop();
    }

}
