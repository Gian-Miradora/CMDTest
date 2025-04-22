using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
public class GameManager : MonoBehaviour
{
    public static GameManager current;

    public Subject<Actor> ActorDamaged = new();
    public Subject<Actor> ActorDied = new();

    public Action RoundStarted;
    public Action<int, Vector3> EnemySpawnedAtPoint;
    public Action<float> TimeElapsedEvent;
    public Action<int> EnemiesKilledEvent;

    [Header("References")]
    [SerializeField] public Transform[] SpawnPoints;

    [Header("Round Statistics")]
    [SerializeField] private float[] LevelIncrements = new float[2];
    private bool OnRound;
    private int EnemiesKilled;
    private float TimeElapsed;

    public IObservable<Actor> ObserveActorDamaged()
    {
        ActorDamaged ??= new Subject<Actor>();
        return ActorDamaged.AsObservable();
    }

    public IObservable<Actor> ObserveActorDeath()
    {
        ActorDied ??= new Subject<Actor>();
        return ActorDied.AsObservable();
    }

    private void Awake()
    {
        if (current == null) current = this;
        else Destroy(this);

        ObserveActorDeath()
            .Where(x => x.gameObject.CompareTag("Enemy"))
            .Subscribe(_ =>
            {
                EnemiesKilled++;
                EnemiesKilledEvent?.Invoke(EnemiesKilled);
            });

        ObserveActorDeath()
            .Where(x => x.gameObject.CompareTag("Player"))
            .Subscribe(_ => OnPlayerDeath());
    }

    private void Start() => StartRound();

    public void StartRound()
    {
        RoundStarted?.Invoke();
        EnemiesKilled = 0;
        TimeElapsed = 0;
        OnRound = true;

        EnemiesKilledEvent?.Invoke(EnemiesKilled);
        TimeElapsedEvent?.Invoke(TimeElapsed);

        StartCoroutine(RoundUpdate());
    }

    private IEnumerator RoundUpdate()
    {
        int level = 0;
        float spawnInterval = UnityEngine.Random.Range(0.1f, 1f);
        float spawnIntervalElapsed = 0;

        do
        {
            TimeElapsed += Time.deltaTime;
            spawnIntervalElapsed += Time.deltaTime;

            TimeElapsedEvent?.Invoke(TimeElapsed);

            if (TimeElapsed > LevelIncrements[0] && TimeElapsed < LevelIncrements[1])
                level = 1;
            else if (TimeElapsed > LevelIncrements[1])
                level = 2;

            if (spawnIntervalElapsed > spawnInterval)
            {
                spawnIntervalElapsed = 0;
                EnemySpawnedAtPoint?.Invoke(level, SpawnPoints[UnityEngine.Random.Range(0, SpawnPoints.Length)].position);
                spawnInterval = UnityEngine.Random.Range(0.1f, 1f);
            }
            yield return new WaitForSeconds(Time.deltaTime);
        } while (OnRound);
    }

    public void OnPlayerDeath()
    {
        GameOver();
    }

    public void GameOver()
    {
        OnRound = false;
        Debug.Log("Game is over!");
    }

}
