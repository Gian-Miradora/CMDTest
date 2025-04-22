using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ZombieAttack : MonoBehaviour
{
    [SerializeField] private EnemyBase enemyBase;

    public void ProcessAttack() => enemyBase.Attack();
}
