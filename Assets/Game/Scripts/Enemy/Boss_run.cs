using UnityEngine;

public class Boss_Run : StateMachineBehaviour
{
	private EnemyAi enemyAi;
	private static readonly int Running = Animator.StringToHash("Running");
	private static readonly int Attack = Animator.StringToHash("Attack"); // New trigger for attack

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		enemyAi = animator.GetComponent<EnemyAi>();
		UpdateRunningState(animator);
	}

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		UpdateRunningState(animator);
	}

	private void UpdateRunningState(Animator animator)
	{
		if (enemyAi != null)
		{
			var currentState = enemyAi.GetCurrentState();
			animator.SetBool(Running, currentState == EnemyAi.State.Chasing);

			if (currentState == EnemyAi.State.Attacking)
			{
				animator.SetTrigger("Attack"); // Trigger the attack animation
			}
			else
			{
				animator.ResetTrigger("Attack"); // Reset the attack trigger
			}
		}
	}
}
