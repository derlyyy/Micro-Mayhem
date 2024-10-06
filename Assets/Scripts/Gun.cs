using System;
using Sonity;
using UnityEngine;

public class Gun : Weapon
{
    public SoundEvent soundJump;
    
    public Transform shootPosition;
    
    public float shootForce;

    public float explosiveRadius;
    
    public float cooldown;
    public float recoil;
    public float recoilMultiplier;
    public float sinceAttack;

    public Projectile projectilePrefab;
    public Projectile currentProjectile;

    public int bounces;
    public int maxBounces;
    
    private Vector2 direction;

    [HideInInspector] public Player player;

    private Rigidbody2D rb;

    private void Start()
    {
        holdable = GetComponent<Holdable>();
        rb = GetComponent<Rigidbody2D>();

        
    }

    private void Update()
    {
        if (holdable && holdable.holder && holdable.holder.player)
        {
            player = holdable.holder.player;
        }

        sinceAttack += Time.deltaTime;
    }

    public bool IsReady()
    {
        return sinceAttack >= cooldown;
    }
    
    public override bool Attack()
    {
        DoAttack();

        sinceAttack = 0;
        
        return true;
    }

    public void DoAttack()
    {
        if (rb)
        {
            rb.AddForce(rb.mass * recoil * 5 * -transform.up, ForceMode2D.Impulse);
        }

        player.data.playerVel.rb.AddForce(-transform.up * recoil * recoilMultiplier, ForceMode2D.Impulse);

        Projectile bullet = Instantiate(projectilePrefab, shootPosition.position, shootPosition.rotation);
        bullet.rb.AddForce(transform.up * shootForce);
        
        SoundManager.Instance.Play(soundJump, transform);
        G.main.camHandle.DoShake(3f, 0.1f);
    }
}