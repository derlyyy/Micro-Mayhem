using System;
using Sonity;
using UnityEngine;

public class CharacterData : MonoBehaviour
{
	public Vector3 aimDirection;
	
	public PlayerActions playerActions;

	public ParticleSystem[] landParts;
	public SoundEvent soundLand;
	public SoundEvent soundStick;

	public int jumps = 1;
	public int currentJumps = 1;

	public bool isPlaying;
	public bool isDead;
	public bool canMove;
	public bool isStunned;

	public AnimationCurve slamCurve;

	public Vector3 wallPos;
	public Vector2 wallNormal;
	public Vector3 groundPos;

	public float sinceWallGrab = float.PositiveInfinity;
	public bool isWallGrab;
	public float wallDistance = 1f;
	private bool wasWallGrabLastFrame;

	public float sinceGrounded;
	public float sinceGroundedMultiplierWhenWallGrab = 0.2f;
	public bool isGrounded = true;
	private bool wasGroundedLastFrame = true;
	public float sinceJump = 1f;

	public Transform hand;

	[HideInInspector] public Player player;
	[HideInInspector] public PlayerVelocity playerVel;
	[HideInInspector] public GeneralInput input;
	[HideInInspector] public PlayerMovement movement;
	[HideInInspector] public PlayerJump jump;
	[HideInInspector] public CharacterStatModifier stats;
	[HideInInspector] public WeaponHandler weapon;
	[HideInInspector] public Collider2D mainCol;
	[HideInInspector] public Animator anim;

	//public HealthHandler health;

	private Transform wobblePos;
	public LayerMask groundMask;
	public Rigidbody2D standOnRig;

	public Action<float, Vector3, Vector3, Transform> TouchGroundAction;
	public Action<float, Vector3, Vector3> TouchWallAction;

	private void Awake()
	{
		player = GetComponent<Player>();
		playerActions = PlayerActions.CreateWithKeyboardBindings();
		mainCol = GetComponent<Collider2D>();
		stats = GetComponent<CharacterStatModifier>();
		input = GetComponent<GeneralInput>();
		movement = GetComponent<PlayerMovement>();
		jump = GetComponent<PlayerJump>();
		weapon = GetComponent<WeaponHandler>();
		playerVel = GetComponent<PlayerVelocity>();
		anim = GetComponentInChildren<Animator>();
	}

	private void Start()
	{
		
	}

	private void Update()
	{
		if (!playerVel.rb.simulated || !canMove)
		{
			sinceGrounded = 0f;
		}
		else
		{
			sinceJump += Time.deltaTime;
		}
		
		Wall();
	}

	private void FixedUpdate()
	{
		Ground();
	}

	private void Ground()
	{
		if (!isPlaying)
		{
			return;
		}
		if (!isGrounded)
		{
			sinceGrounded += Time.deltaTime * ((isWallGrab && wallDistance < 0.7f) ? sinceGroundedMultiplierWhenWallGrab : 1f);
			if (sinceGrounded < 0f)
			{
				sinceGrounded = Mathf.Lerp(sinceGrounded, 0f, Time.deltaTime * 15f);
			}
		}
		if (!wasGroundedLastFrame)
		{
			isGrounded = false;
		}
		wasGroundedLastFrame = false;
	}

	public void Wall()
	{
		if (!isWallGrab)
		{
			sinceWallGrab += Time.deltaTime;
		}

		if (!wasWallGrabLastFrame)
		{
			isWallGrab = false;
		}

		wasWallGrabLastFrame = false;
	}

	public void TouchGround(Vector3 pos, Vector3 groundNormal, Rigidbody2D groundRig, Transform groundTransform = null)
	{
		if (sinceJump > 0.2f)
		{
			currentJumps = jumps;
		}
		if (TouchGroundAction != null)
		{
			TouchGroundAction(sinceGrounded, pos, groundNormal, groundTransform);
		}
		standOnRig = groundRig;
		if (playerVel.rb.velocity.y < -10f && !isGrounded)
		{
			for (int i = 0; i < landParts.Length; i++)
			{
				landParts[i].transform.localScale = Vector3.one * Mathf.Clamp((0f - playerVel.rb.velocity.y) / 40f, 0.5f, 2f) * 0.5f;
				
				landParts[i].transform.position = new Vector3(base.transform.position.x + playerVel.rb.velocity.x * 0.03f, pos.y, 5f);
				
				landParts[i].transform.rotation = Quaternion.LookRotation(groundNormal);
				landParts[i].Play();
			}
			
			G.main.camHandle.DoShake(2f, 0.1f);
			SoundManager.Instance.Play(soundLand, transform);
		}
		groundPos = pos;
		wasGroundedLastFrame = true;
		isGrounded = true;
		sinceGrounded = 0f;
	}

	public void TouchWall(Vector2 normal, Vector3 pos)
	{
		if (isGrounded)
		{
			return;
		}

		wallNormal = normal;
		wallPos = pos;
		groundPos = pos;
		wallDistance = Vector2.Distance(transform.position, pos);
		if (!(sinceJump < 0.15f))
		{
			currentJumps = jumps;
			if (TouchWallAction != null)
			{
				TouchWallAction(sinceWallGrab, pos, normal);
			}

			_ = sinceWallGrab;
			_ = 0.15f;
			sinceWallGrab = 0f;
			wasWallGrabLastFrame = true;
			isWallGrab = true;
			
			SoundManager.Instance.Play(soundStick, transform);
		}
	}

	public bool ThereIsGroundBelow(Vector3 pos, float range = 5f)
	{
		RaycastHit2D raycastHit2D = Physics2D.Raycast(pos, Vector2.down, range, groundMask);
		if ((bool)raycastHit2D.transform && raycastHit2D.distance > 0.1f)
		{
			return true;
		}
		return false;
	}

	public void SetWobbleObjectChild(Transform obj)
	{
		obj.transform.SetParent(wobblePos, worldPositionStays: true);
	}
}