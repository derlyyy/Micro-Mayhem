using Sonity;
using UnityEngine;
using UnityEngine.Serialization;

public class WeaponHandler : MonoBehaviour
{
    [FormerlySerializedAs("spear")] public Gun gun;
    public SpriteRenderer handSr;

    public ParticleSystem changeArrowPart;

    public Holding holding;
    private GeneralInput input;
    private CharacterData data;

    private void Awake()
    {
        holding = GetComponent<Holding>();
        input = GetComponent<GeneralInput>();
        data = GetComponent<CharacterData>();
    }

    private void Update()
    {
        if (!gun.holdable.holder && data)
        {
            gun.holdable.holder = data;
        }

        if (data.playerVel.rb.simulated)
        {
            Attack();
        }
    }

    public void Attack()
    {
        if (!gun || !gun.IsReady())
        {
            return;
        }

        if (input.shootIsPressed)
        {
            //spear.ChargeAttack();
        }

        if (input.shootWasReleased)
        {
            gun.Attack();
        }
    }
}