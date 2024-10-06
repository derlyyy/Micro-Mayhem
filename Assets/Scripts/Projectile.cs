using Sonity;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public SoundEvent soundHit;
    
    public int bounces;
    public int maxBounces;

    public GameObject hitEffect;
    
    public Rigidbody2D rb;
    
    private void OnCollisionEnter2D(Collision2D coll)
    {
        Vector2 collisionNormal = coll.contacts[0].normal;

        // Рассчитываем угол поворота для эффекта
        float angle = Mathf.Atan2(collisionNormal.y, collisionNormal.x) * Mathf.Rad2Deg;
        var effect = Instantiate(hitEffect, coll.contacts[0].point, hitEffect.transform.rotation);
        
        SoundManager.Instance.Play(soundHit, transform);
        G.main.camHandle.DoShake(5f, 0.1f);
        
        Destroy(gameObject);
    }
}
