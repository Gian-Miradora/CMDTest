using UnityEngine;

public class Knockback : StateMachineBehaviour
{
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.transform.parent.TryGetComponent<Actor>(out Actor actor))
        {
            actor.MoveActor(-animator.transform.up * 0.2f);
        }
    }
}
