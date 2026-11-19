using UnityEngine;

public class PostProcessingManager : MonoBehaviour
{
    [SerializeField] private GameObject[] PPEffect;

    public static PostProcessingManager instance { get; set; }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }
    public void ActivateEffect(int ID)
    {
        PPEffect[ID].SetActive(true);
    }
    
    public void DeactivateEffect(int ID)
    {
        PPEffect[ID].SetActive(false);
    }
}
