using UnityEngine;

public class LegRaycasters : MonoBehaviour
{
	[SerializeField] private LayerMask mask;

	[SerializeField] private float force = 10f;

	[SerializeField] private float drag = 5f;

	[SerializeField] private Transform[] legCastPositions;

	[SerializeField] private AnimationCurve animationCurve;

	private PlayerVelocity rig;

	private CharacterData data;

	public AnimationCurve wobbleCurve;

	public AnimationCurve forceCurve;

	private IKLeg[] legs;

	private float totalStepTime;

	private void Awake()
	{
		legs = base.transform.root.GetComponentsInChildren<IKLeg>();
	}

	private void Start()
	{
		rig = GetComponentInParent<PlayerVelocity>();
		data = GetComponentInParent<CharacterData>();
	}

	private void FixedUpdate()
	{
		totalStepTime = 0f;
		for (int i = 0; i < legs.Length; i++)
		{
			if (!legs[i].footDown)
			{
				totalStepTime += legs[i].stepTime;
			}
		}
		
		CastRays();
	}

	private void CastRays()
	{
		Transform[] array = legCastPositions;
		foreach (Transform transform in array)
		{
			RaycastHit2D[] array2 = Physics2D.RaycastAll(transform.position + Vector3.up * 0.5f, Vector2.down, 1f * transform.root.localScale.x, mask);
			for (int j = 0; j < array2.Length; j++)
			{
				RaycastHit2D hit = array2[j];
				if (hit.collider != null && hit.transform.root != base.transform.root)
				{
					HitGround(transform, hit);
					break;
				}
			}
		}
	}

	private void HitGround(Transform leg, RaycastHit2D hit)
	{
		if (data.sinceJump >= 0.2f && Vector3.Angle(Vector3.up, hit.normal) <= 70f)
		{
			data.TouchGround(hit.point, hit.normal, hit.rigidbody);
			Vector3 vector = ((Vector3)hit.point - leg.position) / base.transform.root.localScale.x;
			if (data.input.direction.x != 0f)
			{
				vector.y += wobbleCurve.Evaluate(totalStepTime) * base.transform.root.localScale.x;
				rig.rb.AddForce(Vector3.up * forceCurve.Evaluate(totalStepTime) * rig.rb.mass);
			}

			rig.rb.AddForce(animationCurve.Evaluate(Mathf.Abs(vector.y)) * Vector3.up * rig.rb.mass * force);
			rig.rb.AddForce(animationCurve.Evaluate(Mathf.Abs(vector.y)) * (0f - rig.rb.velocity.y) * Vector2.up * rig.rb.mass * drag);
		}
	}
}