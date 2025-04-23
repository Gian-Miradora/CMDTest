using System;
using System.Collections;
using System.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

[RequireComponent(typeof(Actor))]
public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField] protected float Damage;

    protected Actor actor;
    protected Animator animator;
    protected CircleCollider2D circleCollider;

    protected Actor targetActor;

    public ActorState enemyState { get; private set; }

    public void SetState(ActorState state)
    {
        enemyState = state;
        switch (enemyState)
        {
            case ActorState.Move:
                animator.SetTrigger("Move");
                break;
            case ActorState.Attack:
                animator.SetTrigger("Attack");
                break;
            case ActorState.Hit:
                animator.SetTrigger("Hit");
                break;
            case ActorState.Death:
                animator.SetTrigger("Death");
                break;
        }
    }

    private void Awake()
    {
        actor = GetComponent<Actor>();
        circleCollider = GetComponent<CircleCollider2D>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        targetActor = GameObject.FindGameObjectWithTag("Player").GetComponent<Actor>();
    }

    private void Start()
    {
        this.OnCollisionEnter2DAsObservable()
            .Where(x => x.transform.CompareTag("Player"))
            .Subscribe(collision => { if (enemyState != ActorState.Death) SetState(ActorState.Attack); });

        this.OnCollisionExit2DAsObservable()
            .Where(x => x.transform.CompareTag("Player"))
            .Subscribe(collision => { if (enemyState != ActorState.Death) SetState(ActorState.Move); });

        GameManager.current.ObserveActorDamaged()
            .Where(x => x == actor)
            .Subscribe(x => Hit());

        GameManager.current.ObserveActorDeath()
            .Where(x => x == actor)
            .Subscribe(x => Death());

        GameManager.current.ObserveActorDeath()
            .Where(x => x == targetActor)
            .Subscribe(x => actor.MoveActor(Vector2.zero));

        GameManager.current.RoundStarted += Initialize;
    }

    public void OnEnable()
    {
        Initialize();
    }

    protected virtual void Initialize()
    {
        SetState(ActorState.Move);
        circleCollider.enabled = true;
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("Hit");
        animator.ResetTrigger("Death");
    }

    public abstract void Attack();

    public virtual void Hit()
    {
        SetState(ActorState.Hit);
    }

    public virtual void Death()
    {
        SetState(ActorState.Death);
        actor.MoveActor(Vector2.zero);
        circleCollider.enabled = false;
        Debug.Log($"{gameObject.name} died!");
        StartCoroutine(DeathCooldown());
    }

    private IEnumerator DeathCooldown()
    {
        yield return new WaitForSeconds(5);
        gameObject.SetActive(false);
    }

    //public async void DeathCooldown()
    //{
    //    await Task.Delay(10000);
    //    try
    //    {
    //        gameObject.SetActive(false);
    //    }
    //    catch
    //    {
    //        Debug.LogWarning("Playmode was ended before this async function was finished. Moving on...");
    //    }
    //}
}