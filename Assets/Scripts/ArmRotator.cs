using UnityEngine;

public class ArmRotator : MonoBehaviour
{
    public Vector3 rotation;

    private void Start()
    {
        transform.localEulerAngles = rotation;
        
        Invoke("later", 0.01f);
    }

    private void later()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }
}