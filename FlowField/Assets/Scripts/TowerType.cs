using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTowerType", menuName = "Tower Type")]
public class TowerType : ScriptableObject
{
    [SerializeField]
    public float minRange, maxRange, damage;
    [SerializeField]
    private float fireRate;

    private float fireTimer;

    [HideInInspector]
    public float BonusInfluenceRange = 20;

    private void Awake()
    {
        fireTimer = 1.0f / fireTimer;
    }

    public float GetFireTimer()
    {
        return fireTimer;
    }
}
