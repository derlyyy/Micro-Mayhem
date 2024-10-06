using Sonity;
using UnityEngine;

public class Player : MonoBehaviour
{
    public CharacterData data;

    public SoundEvent soundDie;
    public GameObject dieEffectPrefab;

    private void Awake()
    {
        data = GetComponent<CharacterData>();
    }

    public void Die()
    {
        Instantiate(dieEffectPrefab, transform.position, Quaternion.identity);
        SoundManager.Instance.Play(soundDie, transform);
        G.main.player_m.RespawnPlayer();
    }
}