using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    private Vector2 lastCheckpointPosition;

    public List<Checkpoint> activatedCheckpoints;

    public void ActivateCheckpoint(Checkpoint checkpoint)
    {
        if (!activatedCheckpoints.Contains(checkpoint))
        {
            activatedCheckpoints.Add(checkpoint);
            Debug.Log("added new checkpoint" + checkpoint);
        }
    }

    public Checkpoint GetLastCheckpoint()
    {
        if (activatedCheckpoints.Count > 0)
        {
            return activatedCheckpoints[activatedCheckpoints.Count - 1];
        }
        else
        {
            return null;
        }
    }

    public bool HasActivatedCheckpoints()
    {
        return activatedCheckpoints.Count > 0;
    }
}
