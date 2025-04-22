using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateSwitch : StateMachineBehaviour
{
    [SerializeField] public string State;
    [SerializeField] public string[] ClosedState;

    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(State, true);
        foreach (var state in ClosedState)
        {
            animator.SetBool(state, false);
        }
    }
}
