using UnityEngine;

public class Aim : MonoBehaviour
{
    private CharacterData data;
    private GeneralInput input;

    private HoldingObject holdingObject;

    private Vector3 aimDirection;

    private void Awake()
    {
        data = GetComponent<CharacterData>();
        input = GetComponent<GeneralInput>();
        holdingObject = GetComponentInChildren<HoldingObject>();
    }

    private void Update()
    {
        if ((double)input.aimDirection.magnitude > 0.2f)
        {
            aimDirection = input.aimDirection;
        }

        if (input.direction.magnitude > 0.2f && input.aimDirection == Vector3.zero)
        {
            aimDirection = input.aimDirection;
        }

        if (holdingObject)
        {
            if (aimDirection != Vector3.zero)
            {
                holdingObject.transform.rotation = Quaternion.LookRotation(aimDirection);
            }

            data.aimDirection = aimDirection;
        }
    }
}