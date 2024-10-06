using UnityEngine;

public class IKArm : MonoBehaviour
{
    private CharacterData data;

    public Transform target;
    public Vector3 startPos = new Vector3(-1, 0, 0); // Начальная позиция таргета

    public bool isActive;
    
    // Параметры для вертикального и горизонтального качания
    public float verticalSwingAmplitude = 0.05f; // Небольшая амплитуда для вертикального качания
    public float horizontalSwingAmplitude = 0.02f; // Ещё меньшая амплитуда для горизонтального
    public float swingFrequency = 2f;   // Пониженная частота для медленного движения
    public float speedThreshold = 0.1f; // Порог скорости для начала качания

    private Vector3 velocity;
    private float sinceRaise = 10f;

    private Holding holding;

    private void Start()
    {
        data = GetComponentInParent<CharacterData>();
        holding = GetComponentInParent<Holding>();

        // Установка начальной позиции
        startPos = target.localPosition;
    }

    private void Update()
    {
        isActive = false;
        sinceRaise += Time.deltaTime;

        if (holding.holdable && holding.isHolding && holding.holdable.rb && 
            holding.holdable.rb.transform.position.x > data.playerVel.rb.transform.position.x == transform.position.x > data.playerVel.rb.position.x)
        {
            target.position = holding.holdable.rb.transform.position;
            velocity = Vector3.zero;
            isActive = true;
            return;
        }

        Vector3 vector = transform.parent.TransformPoint(startPos) + ((sinceRaise < 0.3f) ? Vector3.up : Vector3.zero);
        Vector3 vector2 = data.playerVel.rb.velocity;
        vector2.x *= 0.3f;
        velocity = Vector3.Lerp(velocity, (vector - target.position) * 15f, 15f);

        float speed = data.playerVel.rb.velocity.magnitude;

        if (speed > speedThreshold)
        {
            float verticalSwingOffset = Mathf.Sin(Time.time * swingFrequency) * verticalSwingAmplitude;
            float horizontalSwingOffset = Mathf.Sin(Time.time * swingFrequency * 0.5f) * horizontalSwingAmplitude;

            Vector3 swingDirection = transform.right * horizontalSwingOffset + transform.up * verticalSwingOffset;

            target.position += swingDirection;
        }

        target.position += velocity * Time.deltaTime;
        target.position += vector2 * -0.3f * Time.deltaTime;
    }

    public void RaiseHands()
    {
        if (!isActive)
        {
            sinceRaise = 0f;
            velocity += Vector3.up * 20f;
        }
    }
}
