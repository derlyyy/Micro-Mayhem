using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Sonity;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class TinyCreature : MonoBehaviour
{
    public enum CreatureType
    {
        Builder,
        Interactor
    }

    public SoundEvent soundExplosion;

    [Header("Settings")]
    public CreatureType creatureType;

    public Transform targetPoint; // Целевая точка для переноса
    public Vector2 targetDirection;

    public GameObject explosionEffectPrefab;

    public LayerMask explosionLayerMask;

    public float moveSpeed = 2.0f;
    public float followDistance = 1.0f;
    public float jumpForce = 5.0f;
    
    public float explosionForce;
    public float objectForceMultiplier;
    public float explosionRadius = 3.0f;
    public float explosionDelay;
    
    [Header("Random Direction")]
    private Vector2 randomDirection;
    public float changeDirectionTime = 2f; // Интервал для смены направления
    private float timeSinceDirectionChange = 0f;

    [Header("Wobble")]
    public float wobbleForce;
    public float stability;

    [Header("Effects")]
    public bool applyForceToObjects; // применять ли силу к обьектам
    public bool scaleForceWithDistance; // скалировать ли силу в зависим

    public bool isMoving;
    public bool hasExploded;

    public Player player => G.main.player; // Убедитесь, что G.main.player корректно инициализирован

    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
        PickRandomDirection();

        rb.centerOfMass = new Vector3(0, -0.5f, 0);
    }

    private void Update()
    {
        switch (creatureType)
        {
            case CreatureType.Builder:
                break;
            case CreatureType.Interactor:
                Move(randomDirection);
                break;
        }

        timeSinceDirectionChange += Time.deltaTime;
        if (timeSinceDirectionChange > changeDirectionTime)
        {
            PickRandomDirection();
            timeSinceDirectionChange = 0f;
        }
    }

    private void FixedUpdate()
    {
        Vector2 upDirection = transform.up;
        Vector2 targetUp = Vector2.up;

        float angleDiff = Vector2.SignedAngle(upDirection, targetUp);
        float stabilizationTorque = angleDiff * stability;
        
        rb.AddTorque(stabilizationTorque * Time.fixedDeltaTime);

        float randomWobble = Random.Range(-1f, 1f) * wobbleForce;
        rb.AddTorque(randomWobble * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            Explode();
        }
    }

    public void Move(Vector2 direction)
    {
        rb.velocity = direction * moveSpeed;

        anim.SetBool("IsRun", true);
    }

    public void PickRandomDirection()
    {
        float randomAngle = Random.Range(-1f, 1f);
        randomDirection = new Vector2(randomAngle, 0).normalized;
    }
    
    public void Explode()
    {
        if (hasExploded) return;

        hasExploded = true;
        
        player.data.sinceGrounded = 0f;
        
        G.main.camHandle.DoShake(35, 0.25f);
        SoundManager.Instance.Play(soundExplosion, transform);
        Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);

        StartCoroutine(HandleExplosionWithDelay());
    }
    
    private IEnumerator HandleExplosionWithDelay()
    {
        yield return new WaitForSeconds(explosionDelay);

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius, explosionLayerMask);

        foreach (var hit in hits)
        {
            Rigidbody2D hitRb = hit.GetComponent<Rigidbody2D>();
            if (hitRb)
            {
                Vector2 explosionDirection = (hitRb.position - rb.position).normalized;

                // Рассчитываем силу взрыва в зависимости от расстояния
                float distance = Vector2.Distance(hitRb.position, rb.position);
                float forceMultiplier = Mathf.Clamp01((explosionRadius - distance) / explosionRadius); // Сила уменьшается с расстоянием
                float force = explosionForce * forceMultiplier; // Итоговая сила взрыва

                // Применяем силу к объекту
                hitRb.AddForce(explosionDirection * force, ForceMode2D.Impulse);
            }
            
            if (hit.CompareTag("TinyCreature") && hit.gameObject != this.gameObject)
            {
                TinyCreature otherCreature = hit.GetComponent<TinyCreature>();
                if (otherCreature != null && !otherCreature.hasExploded)
                {
                    otherCreature.Explode();
                }
            }
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
