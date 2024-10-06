// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// LegRotator
using UnityEngine;

public class LegRotator : MonoBehaviour
{
    private PlayerVelocity rig;

    private void Start()
    {
        rig = GetComponentInParent<PlayerVelocity>();
    }

    private void Update()
    {
        if ((bool)rig)
        {
            if (rig.rb.velocity.x < 0f)
            {
                base.transform.localEulerAngles = Vector3.Lerp(base.transform.localEulerAngles, new Vector3(0f, 0f, 0f), Time.deltaTime * 15f * Mathf.Clamp(Mathf.Abs(rig.rb.velocity.x), 0f, 1f));
            }
            else
            {
                base.transform.localEulerAngles = Vector3.Lerp(base.transform.localEulerAngles, new Vector3(0f, 180f, 0f), Time.deltaTime * 15f * Mathf.Clamp(Mathf.Abs(rig.rb.velocity.x), 0f, 1f));
            }
        }
    }
}