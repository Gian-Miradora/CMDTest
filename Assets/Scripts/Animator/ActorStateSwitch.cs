using UnityEngine;

public class ActorStateSwitch : StateMachineBehaviour
{
    [SerializeField] private bool TriggerOnEnter;
    [SerializeField] private bool TriggerOnExit;
    [SerializeField] private ActorState enterState;
    [SerializeField] private ActorState exitState;

    private EnemyAbstractStateSwitcher actorBase;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (actorBase == null)
            actorBase = animator.transform.GetComponent<EnemyAbstractStateSwitcher>();
        
        actorBase.SetState(enterState);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (actorBase == null)
            actorBase = animator.transform.GetComponent<EnemyAbstractStateSwitcher>();
        
        actorBase.SetState(exitState);
    }
}
