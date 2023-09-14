using System;
using UnityEngine;

public enum EnemyType
{
    None = 0,
    Enemy001,
    Enemy002,
    Enemy003,
    Enemy004,
    Enemy005,
    Enemy006,
    Enemy007,
    Enemy008,
    Enemy009
}

public enum LocationType
{
    None = 0,
    RandomPoint,
    RandomHorizontal
}

public enum TargetType
{
    None = 0,
    First,
    Every
}

[Serializable]
public class EnemyGroup
{
    [SerializeField] EnemyType m_enemyType;         // 敵種類
    [SerializeField] LocationType m_locationType;   // 出現種類
    [SerializeField] TargetType m_targetType;       // ターゲット種類
    [SerializeField] int m_count;                   // 個数
    [SerializeField] float m_interval;              // 間隔
    [SerializeField] float m_distance;              // 距離

    public EnemyType EnemyType => m_enemyType;
    public LocationType LocationType => m_locationType;
    public TargetType TargetType => m_targetType;
    public int Count => m_count;
    public float Interval => m_interval;
    public float Distance => m_distance;
}
