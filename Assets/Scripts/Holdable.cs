using UnityEngine;

public class Holdable : MonoBehaviour
{
    public Rigidbody2D rb;
    public CharacterData holder;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
}