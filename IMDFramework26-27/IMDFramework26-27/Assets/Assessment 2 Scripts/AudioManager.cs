using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] m_playerSounds = new AudioClip[4];
    [SerializeField] private AudioClip[] m_collectibleSounds = new AudioClip[1];
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

    public void PlaySound(int clipID)
    {
        m_audioSource.loop = false;
        m_audioSource.clip = m_collectibleSounds[clipID];
        m_audioSource.Play();
    }

    public void StopSound()
    {
        m_audioSource.Stop();
    }

}
