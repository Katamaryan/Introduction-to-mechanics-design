using UnityEngine;
using Unity.Properties;
public class TransformWrapper : MonoBehaviour
{
    [CreateProperty]
    public float XPos { get { return transform.position.x; } }
    [CreateProperty]
    public float YPos { get { return transform.position.y; } }
    
    [CreateProperty]
    public float XVol { get { return gameObject.GetComponent<Rigidbody2D>().linearVelocity.x; } }
    
    [CreateProperty]
    public float YVol { get { return gameObject.GetComponent<Rigidbody2D>().linearVelocity.y; } }
    
}
