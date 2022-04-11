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

    [HideInInspector]
    public float BonusInfluenceRange = 20;

    private void Awake()
    {
    }

    public float GetFireTimer()
    {
        return 1.0f / fireRate;
    }
}
