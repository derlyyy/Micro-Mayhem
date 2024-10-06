using UnityEngine;

public class Gravity : MonoBehaviour
{
    [SerializeField]
    private float gravityForce = 9.81f;

    [SerializeField]
    private float exponent = 1f;

    private PlayerVelocity rig;

    private CharacterData data;

    private void Start()
    {
        data = GetComponent<CharacterData>();
        rig = GetComponent<PlayerVelocity>();
    }

    private void FixedUpdate()
    {
        float num = Mathf.Min(data.sinceGrounded, data.sinceWallGrab);
        float num2 = ((num > 0f) ? Mathf.Pow(num, exponent) : num);
        rig.rb.AddForce(Vector3.down * num2 * gravityForce * rig.rb.mass, ForceMode2D.Force);
    }
}