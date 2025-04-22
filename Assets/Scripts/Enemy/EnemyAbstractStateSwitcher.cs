using UnityEngine;

public class EnemyAbstractStateSwitcher : MonoBehaviour
{
    [SerializeField] private EnemyBase enemyBase;

    public void SetState(ActorState state)
    {
        if (enemyBase.enemyState != ActorState.Death)
            enemyBase.SetState(state);
    }
}