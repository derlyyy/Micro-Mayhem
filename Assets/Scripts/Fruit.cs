using System;
using Sonity;
using UnityEngine;
using DG.Tweening;

public class Fruit : MonoBehaviour
{
    public SoundEvent soundCollect; 
    public GameObject destroyEffect; 
    
    [Header("Animation Settings")]
    public float swayAngle = 15f; 
    public float swayDuration = 1f; 
    public float pulseScale = 1.2f; 
    public float pulseDuration = 0.5f; 
    public float attractDuration = 0.3f; 
    public float disappearDuration = 0.2f; 

    private Vector3 originalScale;
    private bool isCollected = false;

    private void Start()
    {
        originalScale = transform.localScale;

        transform.DORotate(new Vector3(0f, 0f, swayAngle), swayDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo)
            .From(new Vector3(0f, 0f, -swayAngle));

        transform.DOScale(originalScale * pulseScale, pulseDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            isCollected = true; 

            transform.DOMove(other.transform.position, attractDuration)
                .SetEase(Ease.InOutQuad)
                .OnComplete(() => Collect(other));
        }
    }

    private void Collect(Collider2D player)
    {
        SoundManager.Instance.Play(soundCollect, transform);
        G.main.AddFruit();
        Instantiate(destroyEffect, transform.position, Quaternion.identity);

        transform.DOScale(originalScale * 1.5f, disappearDuration)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() => Destroy(gameObject));
    }
}
