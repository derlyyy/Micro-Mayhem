using UnityEngine;

public class FollowInactiveHand : MonoBehaviour
{
    public Vector3 offset;
    
    public GameObject leftHand;
    public GameObject rightHand;
    
    private CharacterData data;

    private void Start()
    {
        data = transform.root.GetComponent<CharacterData>();
    }

    private void Update()
    {
        if (data.aimDirection.x < 0f)
        {
            transform.position = rightHand.transform.TransformPoint(offset);
        }
        else
        {
            transform.position = leftHand.transform.TransformPoint(offset);
        }
    }
}