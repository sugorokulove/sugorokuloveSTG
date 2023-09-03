using System;
using UnityEngine;

public enum EnemyType
{
    Enemy001 = 0,
    Enemy002,
    Enemy003,
    Enemy004,
    Enemy005,
    Enemy006,
    Enemy007,
    Enemy008,
    Enemy009
}

[Serializable]
public class EnemyGroup
{
    [SerializeField] EnemyType m_type;      // 種類
    [SerializeField] int m_count;           // 個数
    [SerializeField] float m_interval;      // 間隔
    [SerializeField] float m_distance;      // 距離

    public EnemyType Type => m_type;
    public int Count => m_count;
    public float Interval => m_interval;
    public float Distance => m_distance;
}
