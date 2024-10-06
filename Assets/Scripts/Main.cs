using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    public Camera mainCam;
    public CinemachineVirtualCamera vcam;
    
    public CameraHandle camHandle;
    public PlayerManager player_m;
    public TinyController tiny_m;
    public CheckpointManager checkpoint_m;

    public Player player;

    public int currentFruits;
    public int maxFruits;

    public TextMeshProUGUI fruitCountText;

    public GameObject menuEffect;
    
    private void Awake()
    {
        G.main = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            G.main.camHandle.DoShake(2, 0.5f);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        { 
            player_m.player.Die();
        }
    }

    public void AddFruit()
    {
        currentFruits++;
        tiny_m.creaturesPerSummon++;

        fruitCountText.text = currentFruits + "/" + maxFruits;
        
        if (currentFruits >= maxFruits)
        {
            Debug.Log("WIN!!!!!");
        }
    }
}
