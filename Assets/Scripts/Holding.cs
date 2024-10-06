using UnityEngine;

public class Holding : MonoBehaviour
{
    public Holdable holdable;
    
    public float force;
    public float drag;
    
    public Transform handPos;

    public bool isHolding;
    private bool hasSpawnedWeapon;
    
    private GeneralInput input;
    private CharacterData data;
    private Player player;
    private Gun _gun;

    private void Awake()
    {
        holdable = Instantiate(holdable, transform.position, Quaternion.identity);
        
        data = GetComponent<CharacterData>();
        input = GetComponent<GeneralInput>();
        holdable.GetComponent<Holdable>().holder = data;
        player = GetComponent<Player>();

        if (holdable)
        {
            _gun = holdable.GetComponent<Gun>();
            GetComponentInChildren<WeaponHandler>().gun = holdable.GetComponent<Gun>();
            isHolding = true;
        }
    }

    private void FixedUpdate()
    {
        if (isHolding && holdable && holdable.rb)
        {
            holdable.rb.AddForce((handPos.transform.position + (Vector3)data.playerVel.rb.velocity * 0.04f - holdable.transform.position) * force * holdable.rb.mass, ForceMode2D.Force);
            holdable.rb.AddForce(holdable.rb.velocity * (0f - drag) * holdable.rb.mass, ForceMode2D.Force);
            holdable.rb.transform.rotation = Quaternion.LookRotation(Vector3.forward, handPos.transform.forward);
        }
    }
}