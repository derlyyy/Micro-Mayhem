using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public Holdable holdable;

    private void Start()
    {
        holdable = GetComponent<Holdable>();
    }

    public abstract bool Attack();
}