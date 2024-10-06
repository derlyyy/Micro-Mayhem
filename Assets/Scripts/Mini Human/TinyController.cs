using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sonity;
using UnityEngine;
using Random = UnityEngine.Random;

public class TinyController : MonoBehaviour
{
    public SoundEvent soundSummon;
    
    public Player player => G.main.player;
    
    public TinyCreature tinyPrefab;
    public int creaturesPerSummon;
    public float spawnRadius;
    
    public List<TinyCreature> activeCreatures = new List<TinyCreature>();

    public Transform hand;  // Рука игрока — цель для возврата

    public float summonCooldown;
    private float cooldownTimer;

    public float slowMotionFactor = 0.1f; // Коэффициент замедления времени
    public float normalTimeFactor = 1f;   // Обычное время

    public float bridgeLength;
    public float spacing;

    public float launchForce;
    public float launchAngle;

    private bool isSlowMotionActive = false;
    public bool isReturning = false; // Булевый флаг для возврата

    [Header("Action Selection")]
    public GameObject actionSelectUI;

    [Header("Return Animation Settings")]
    public AnimationCurve returnCurve; // Анимационная кривая для возврата
    public float returnDuration = 2f;  // Длительность возврата
    public float returnDelay = 0.3f;   // Задержка между возвратами существ

    private List<TinyCreature> creaturesToRemove = new List<TinyCreature>(); // Список для удаления существ

    private void Update()
    {
        if (G.main.player.data.playerActions.Summon.WasPressed && IsReady())
        {
            StartCoroutine(SummonTinyCreatureCoroutine());
        }

        cooldownTimer += Time.deltaTime;
    }

    public bool IsReady()
    {
        return cooldownTimer >= summonCooldown;
    }

    public IEnumerator SummonTinyCreatureCoroutine()
    {
        for (int i = 0; i < creaturesPerSummon; i++)
        {
            //Vector2 spawnPos = (Vector2)player.transform.position + Random.insideUnitCircle * 2;
            TinyCreature newTiny = Instantiate(tinyPrefab, player.data.hand.position, Quaternion.identity);
            
            player.data.hand.DOPunchScale(new Vector3(1, 1, 1), 0.1f);
            player.data.hand.DOPunchRotation(new Vector3(25, 25, 25), 0.1f);
            
            newTiny.transform.localScale = Vector3.zero;
            newTiny.transform.DOScale(new Vector3(1, 1, 1), 0.5f).SetEase(Ease.OutBack);
            
            float randomAngle = Random.Range(-launchAngle, launchAngle);
            float angleInRadians = randomAngle * Mathf.Deg2Rad;
            Vector2 launchDirection = new Vector2(Mathf.Sin(angleInRadians), Mathf.Cos(angleInRadians)).normalized;
            newTiny.rb.AddForce(launchDirection * launchForce, ForceMode2D.Impulse);
            
            activeCreatures.Add(newTiny);
            
            SoundManager.Instance.Play(soundSummon, player.transform);
            cooldownTimer = 0;

            yield return new WaitForSeconds(0.1f);
        }
    }
}