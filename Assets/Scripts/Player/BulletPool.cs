using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    [SerializeField] private GameObject BulletPrefab;
    [SerializeField] private int BulletCount = 50;
    
    private List<BulletBase> pooledBullets = new();

    private void Awake()
    {
        if (BulletPrefab == null) return;
        for (int i = 0; i < BulletCount; i++)
        {
            pooledBullets.Add(Instantiate<GameObject>(BulletPrefab, transform).GetComponent<BulletBase>());
        }
    }
}
