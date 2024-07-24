using System.Collections;
using UnityEngine;

public class SlimeBoss : MonoBehaviour
{
	private enum SkillType
	{
		SummonCreep,
		BodySlam,
		Charge
	}

	[Header("Skill Settings")]
	[SerializeField] private float skillDelay = 2f;
	[SerializeField] private GameObject creepPrefab;
	[SerializeField] private float chargeSpeed = 5f;
	[SerializeField] private float bodySlamRadius = 2f;

	private Animator animator;
	private Rigidbody2D rb;
	private SlimeMovement slimeMovement;
	private bool isUsingSkill = false;

	private void Awake()
	{
		animator = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
		slimeMovement = GetComponent<SlimeMovement>();
	}

	private void Start()
	{
		StartCoroutine(SkillRoutine());
	}

	private IEnumerator SkillRoutine()
	{
		while (true)
		{
			yield return new WaitForSeconds(skillDelay);
			if (!isUsingSkill)
			{
				UseRandomSkill();
			}
		}
	}

	private void UseRandomSkill()
	{
		int skillIndex = Random.Range(0, 3);
		SkillType selectedSkill = (SkillType)skillIndex;

		switch (selectedSkill)
		{
			case SkillType.SummonCreep:
				StartCoroutine(SummonCreep());
				break;
			case SkillType.BodySlam:
				StartCoroutine(BodySlamSequence());
				break;
			case SkillType.Charge:
				StartCoroutine(Charge());
				break;
		}
	}

	private IEnumerator SummonCreep()
	{
		isUsingSkill = true;
		slimeMovement.StopMovement();
		animator.SetBool("isWalking", false);
		animator.SetBool("isJumping", false);
		yield return new WaitForSeconds(skillDelay);
		Vector2 spawnPosition = new Vector2(
			Random.Range(-10f, 10f),
			Random.Range(-10f, 10f)
		);
		Instantiate(creepPrefab, spawnPosition, Quaternion.identity);
		yield return new WaitForSeconds(skillDelay); // Add delay before next skill
		isUsingSkill = false;
	}

	private IEnumerator BodySlam()
	{
		isUsingSkill = true;
		animator.SetBool("isWalking", false);
		animator.SetBool("isJumping", true);
		yield return new WaitForSeconds(skillDelay);
		animator.SetBool("isJumping", false);
		Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, bodySlamRadius);
		foreach (var hit in hits)
		{
			if (hit.CompareTag("Player"))
			{
				// Apply damage to player
				Debug.Log("Player hit by Body Slam");
			}
		}
	}

	private IEnumerator BodySlamSequence()
	{
		for (int i = 0; i < 3; i++)
		{
			yield return BodySlam();
			yield return new WaitForSeconds(1f); // Delay between jumps
		}
		yield return new WaitForSeconds(skillDelay); // Add delay before next skill
		isUsingSkill = false;
	}

	private IEnumerator Charge()
	{
		isUsingSkill = true;
		slimeMovement.StopMovement();
		animator.SetBool("isWalking", false);
		animator.SetBool("isJumping", false);
		yield return new WaitForSeconds(skillDelay);
		Vector2 direction = (slimeMovement.GetPlayerPosition() - (Vector2)transform.position).normalized;
		rb.velocity = direction * chargeSpeed;
		yield return new WaitForSeconds(1f);
		rb.velocity = Vector2.zero;
		Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.5f);
		foreach (var hit in hits)
		{
			if (hit.CompareTag("Player"))
			{
				// Apply damage to player
				Debug.Log("Player hit by Charge");
			}
		}
		isUsingSkill = false;
	}
}
