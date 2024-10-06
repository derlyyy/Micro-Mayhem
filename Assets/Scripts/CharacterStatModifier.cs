// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// CharacterStatModifier
using System;
using UnityEngine;

public class CharacterStatModifier : MonoBehaviour
{
	[Header("Sounds")]
	private float soundSlowTime;

	private float soundSlowSpeedSec = 0.3f;

	[Header("Settings")]
	public GameObject AddObjectToPlayer;

	[Header("Multiply")]
	public float sizeMultiplier = 1f;

	public float health = 1f;

	public float movementSpeed = 1f;

	public float jump = 1f;

	public float gravity = 1f;

	public float slow;

	public float slowSlow;

	public float fastSlow;

	[Header("Add")]
	public float secondsToTakeDamageOver;

	public int numberOfJumps;

	public float regen;

	public float lifeSteal;

	public bool refreshOnDamage;

	[HideInInspector]
	public float tasteOfBloodSpeed = 1f;

	[HideInInspector]
	public float rageSpeed = 1f;

	public float attackSpeedMultiplier = 1f;

	private CharacterData data;

	public ParticleSystem slowPart;

	private float soundBigThreshold = 1.5f;

	public Action<Vector2, bool> DealtDamageAction;

	public Action<Vector2, bool> WasDealtDamageAction;

	public bool SoundTransformScaleThresholdReached()
	{
		if (base.transform.localScale.x > soundBigThreshold)
		{
			return true;
		}
		return false;
	}

	private void Start()
	{
		data = GetComponent<CharacterData>();
		//ConfigureMassAndSize();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.H))
		{
			sizeMultiplier *= 1.1f;
			ConfigureMassAndSize();
		}
		
		attackSpeedMultiplier = tasteOfBloodSpeed * rageSpeed;
		slow = slowSlow;
		if (fastSlow > slowSlow)
		{
			slow = fastSlow;
		}
		if (slowSlow > 0f)
		{
			slowSlow = Mathf.Clamp(slowSlow - Time.deltaTime * 0.3f, 0f, 1f);
		}
		if (fastSlow > 0f)
		{
			fastSlow = Mathf.Clamp(fastSlow - Time.deltaTime * 2f, 0f, 1f);
		}
	}

	public void WasDealtDamage(Vector2 damage, bool selfDamage)
	{
		if (WasDealtDamageAction != null)
		{
			WasDealtDamageAction(damage, selfDamage);
		}
	}

	private void DoSlowDown(float newSlow)
	{
		if (soundSlowTime + soundSlowSpeedSec < Time.time)
		{
			soundSlowTime = Time.time;
		}
		float num = Mathf.Clamp(newSlow - slow, 0f, 1f);
		slowPart.Emit((int)Mathf.Clamp((newSlow * 0.1f + num * 0.7f) * 50f, 1f, 50f));
		data.playerVel.rb.velocity *= 1f - num * 1f;
		data.sinceGrounded *= 1f - num * 1f;
	}

	internal void ConfigureMassAndSize()
	{
		transform.localScale = Vector3.one * 1.2f * Mathf.Pow(1.2f, 0.2f) * sizeMultiplier;
		data.playerVel.rb.mass = 100f * Mathf.Pow(1.2f, 0.8f) * sizeMultiplier;
	}
}
