using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerManager : MonoBehaviour
{
    public Player player => G.main.player;
    
    public AnimationCurve playerMoveCurve;

    public Transform startPosition;

    public void RespawnPlayer()
    {
        if (G.main.checkpoint_m.HasActivatedCheckpoints())
        {
            StartCoroutine(Move(player.data.playerVel, G.main.checkpoint_m.GetLastCheckpoint().transform.position));
        }
        else
        {
            StartCoroutine(Move(player.data.playerVel, startPosition.transform.position));
        }
    }
    
    public IEnumerator Move(PlayerVelocity player, Vector3 targetPos)
    {
        player.GetComponent<Player>().data.isPlaying = false;
        player.GetComponent<Player>().data.canMove = false;
        player.rb.simulated = false;
        player.rb.isKinematic = true;
        Vector3 distance = targetPos - player.transform.position;
        Vector3 targetStartPos = player.transform.position;
        PlayerCollision col = player.GetComponent<PlayerCollision>();
        float t = playerMoveCurve.keys[playerMoveCurve.keys.Length - 1].time;
        float c = 0f;
        while (c < t)
        {
            col.IgnoreWallForFrames(2);
            c += Mathf.Clamp(Time.unscaledDeltaTime, 0f, 0.02f);
            player.transform.position = targetStartPos + distance * playerMoveCurve.Evaluate(c);
            yield return null;
        }
        int frames = 0;
        while (frames < 10)
        {
            player.transform.position = targetStartPos + distance;
            frames++;
            yield return null;
        }
        player.rb.simulated = true;
        player.rb.isKinematic = false;
        player.GetComponent<Player>().data.isPlaying = true;
        player.GetComponent<Player>().data.canMove = true;
    }

}
