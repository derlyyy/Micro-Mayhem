using UnityEngine;

public class RemoveAfterSeconds : MonoBehaviour
{
    public float seconds;
    
    private void Start()
    {
        Destroy(gameObject, seconds);
    }
}