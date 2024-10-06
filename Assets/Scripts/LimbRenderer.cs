using System.Collections.Generic;
using UnityEngine;

public class LimbRenderer : MonoBehaviour
{
    [Header("Joints")]
    public Transform startJoint;

    public Transform midJoint;

    public Transform endJoint;

    [Header("Segments")]
    public int segmentCount = 10;

    public GameObject segmentPrefab;

    public List<GameObject> segments = new List<GameObject>();

    private void Awake()
    {
        InitializeSegments();
    }

    private void InitializeSegments()
    {
        for (int i = 0; i < segmentCount; i++)
        {
            GameObject segment = Instantiate(segmentPrefab, transform);
            segment.SetActive(true);
            segments.Add(segment);
            if (i == segmentCount - 1)
            {
                segment.transform.localScale = Vector3.zero;
            }
        }
    }

    private void Update()
    {
        UpdateSegmentPositions();
        UpdateSegmentRotations();
    }

    private void UpdateSegmentPositions()
    {
        for (int i = 0; i < segments.Count; i++)
        {
            float t = (float)i / (float)(segments.Count - 1);
            segments[i].transform.position = BezierCurve.QuadraticBezier(startJoint.position, midJoint.position, endJoint.position, t);
        }
    }

    private void UpdateSegmentRotations()
    {
        for (int i = 0; i < segments.Count; i++)
        {
            Vector3 upwards = CalculateSegmentDirection(i);
            segments[i].transform.rotation = Quaternion.LookRotation(Vector3.forward, upwards);
        }
    }

    private Vector3 CalculateSegmentDirection(int index)
    {
        if (index == 0)
        {
            return segments[1].transform.position - segments[0].transform.position;
        }
        if (index == segments.Count - 1)
        {
            return segments[index].transform.position - segments[index - 1].transform.position;
        }
        Vector3 vector = segments[index].transform.position - segments[index - 1].transform.position;
        Vector3 vector2 = segments[index + 1].transform.position - segments[index].transform.position;
        return (vector + vector2) * 0.5f;
    }
}