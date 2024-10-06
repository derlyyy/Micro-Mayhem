using UnityEngine;

public class HoldingObject : MonoBehaviour
{
    public Holding holder;

    private void Update()
    {
        if (!(holder == null) && Vector3.Distance(holder.transform.position, transform.position) > 100f)
        {
            transform.position = holder.transform.position;
        }
    }
}