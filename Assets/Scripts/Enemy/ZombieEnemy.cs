using UnityEngine;

public class ZombieEnemy : EnemyBase
{
    [SerializeField] private float DamageAngle = 30f;

    private void Update()
    {
        if (enemyState == ActorState.Move && targetActor.currentHealth > 0)
        {
            actor.MoveActor((targetActor.transform.position - transform.position).normalized);
            actor.RotateActor(targetActor.transform.position, transform.up);
        }
        else
        {
            actor.MoveActor(Vector2.zero);
        }
    }

    public override void Attack()
    {
        var playerVector = targetActor.transform.position - transform.position;
        var character = transform.GetChild(0).transform;
        var insideAngle = Mathf.Abs(Vector2.SignedAngle(character.up, playerVector)) < (DamageAngle / 2);

        if (Vector2.Distance(transform.position, targetActor.transform.position) < 1f && insideAngle)
        {
            targetActor.DamageActor(Damage);
        }
    }
}
