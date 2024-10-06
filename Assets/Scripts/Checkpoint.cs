using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int index;

    public bool isActivated;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            G.main.checkpoint_m.ActivateCheckpoint(this);
        }
    }
}
