using UnityEngine;

public class RightLeftMirrorSpring : MonoBehaviour
{
    public Vector3 leftPosition;
    public Vector3 rightPosition;

    public float leftRotation;
    public float rightRotation;

    private Vector3 positionVelocity;
    private float rotationVelocity;

    public float drag;
    public float spring;

    private Holdable holdable;

    private float currentRotation;

    private void Start()
    {
        currentRotation = transform.localEulerAngles.z;
        holdable = transform.root.GetComponent<Holdable>();
        rightPosition = transform.localPosition;
    }

    private void Update()
    {
        if (holdable && holdable.holder)
        {
            bool isOnLeftSide = transform.root.position.x - 0.1f < holdable.holder.transform.position.x;
            
            Vector3 targetPosition = (isOnLeftSide ? leftPosition : rightPosition);
            float targetRotation = (isOnLeftSide ? leftRotation : rightRotation);
            
            positionVelocity = Vector3.Lerp(positionVelocity, (targetPosition - transform.localPosition) * spring, drag);
            rotationVelocity = Mathf.Lerp(rotationVelocity, (targetRotation - currentRotation) * spring, drag);
            currentRotation += rotationVelocity * Time.deltaTime;

            transform.localPosition += positionVelocity * Time.deltaTime;
            transform.localEulerAngles = new Vector3(0f, 0f, currentRotation);
        }
    }
}