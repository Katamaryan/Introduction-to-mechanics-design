using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private CharacterMovement m_characterMovement;
    private GroundCheck m_groundCheck;
    private InputHandler m_inputHandler;
    private ColliderPinch m_colliderPinch;

    void Awake()
    {
        m_characterMovement = GetComponent<CharacterMovement>();
        m_colliderPinch = GetComponent<ColliderPinch>();
        m_groundCheck = GetComponent<GroundCheck>();
        m_inputHandler = GetComponent<InputHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
