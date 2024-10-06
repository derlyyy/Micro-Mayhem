using System;
using Sonity;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
	private CharacterData data;

	public SoundEvent soundJump;

	public float upForce;

	private CharacterStatModifier stats;

	public ParticleSystem[] jumpPart;

	public float sideForce = 1f;

	public Action JumpAction;

	private void Start()
	{
		stats = GetComponent<CharacterStatModifier>();
		data = GetComponent<CharacterData>();
	}

	private void Update()
	{
		if (data.input.jumpWasPressed && data.canMove)
		{
			Jump();
		}
		if (data.input.jumpIsPressed && data.sinceJump < 0.2f)
		{
			data.playerVel.rb.AddForce(Vector2.up * Time.deltaTime * 2f * data.stats.jump * data.playerVel.rb.mass * (1f - stats.slow) * upForce, ForceMode2D.Force);
		}
	}

	public void Jump(bool forceJump = false, float multiplier = 1f)
	{
		if (!forceJump && (data.sinceJump < 0.1f || (data.currentJumps <= 0 && data.sinceWallGrab > 0.1f)))
		{
			return;
		}
		Vector3 vector = Vector3.up;
		Vector3 vector2 = data.groundPos;
		if (JumpAction != null)
		{
			JumpAction();
		}
		bool flag = false;
		if (data.sinceWallGrab < 0.1f && !data.isGrounded)
		{
			vector = Vector2.up * 0.8f + data.wallNormal * 0.4f;
			vector2 = data.wallPos;
			data.currentJumps = data.jumps;
			flag = true;
		}
		else
		{
			if (data.sinceGrounded > 0.05f)
			{
				vector2 = transform.position;
			}
			data.currentJumps = data.jumps;
		}
		if (data.playerVel.rb.velocity.y < 0f)
		{
			data.playerVel.rb.velocity = new Vector2(data.playerVel.rb.velocity.x, 0f);
		}
		data.sinceGrounded = 0f;
		data.sinceJump = 0f;
		data.isGrounded = false;
		data.isWallGrab = false;
		data.currentJumps--;
		SoundManager.Instance.Play(soundJump, transform);
		data.playerVel.rb.AddForce(vector * 0.01f * data.stats.jump * data.playerVel.rb.mass * (1f - stats.slow) * upForce, ForceMode2D.Impulse);
		if (!flag)
		{
			data.playerVel.rb.AddForce(Vector2.right * sideForce * 0.01f * data.stats.jump * data.playerVel.rb.mass * (1f - stats.slow) * data.playerVel.rb.velocity.x, ForceMode2D.Impulse);
		}
		for (int i = 0; i < jumpPart.Length; i++)
		{
			jumpPart[i].transform.position = new Vector3(vector2.x, vector2.y, 5f) - vector * 0f;
			jumpPart[i].transform.rotation = Quaternion.LookRotation(data.playerVel.rb.velocity);
			jumpPart[i].Play();
		}
	}
}