using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] public int MaxPoolAmountPerType = 50;
    [SerializeField] public GameObject[] EnemyTypes;

    private List<List<GameObject>> pooledEnemies = new();

    private void Awake()
    {
        if (EnemyTypes == null || EnemyTypes.Length == 0) return;
        foreach (var enemy in EnemyTypes)
        {
            var enemyPool = new List<GameObject>();
            for (int i = 0; i < MaxPoolAmountPerType; i++)
            {
                var loadedEnemy = Instantiate<GameObject>(enemy, transform);
                enemyPool.Add(loadedEnemy);
                loadedEnemy.SetActive(false);
            }
            pooledEnemies.Add(enemyPool);
        }
    }

    private void Start()
    {
        GameManager.current.RoundStarted += OnRoundStarted;
        GameManager.current.EnemySpawnedAtPoint += SpawnInPosition;
    }

    public void OnRoundStarted()
    {
        foreach (var pool in pooledEnemies)
        {
            foreach (var enemy in pool)
            {
                enemy.SetActive(false);
            }
        }
    }

    public void SpawnInPosition(int level, Vector3 spawnPosition)
    {
        int randEnemy = Random.Range(0, level + 1);

        foreach (var enemy in pooledEnemies[randEnemy])
        {
            if (enemy.activeSelf) continue;
            enemy.SetActive(true);
            enemy.transform.position = spawnPosition;
            Debug.Log($"Spawning {enemy.name}");
            break;
        }
    }
}
