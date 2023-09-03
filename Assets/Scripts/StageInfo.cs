using System;
using UnityEngine;

[Serializable]
public class StageInfo
{
    [SerializeField] Sprite[] m_files;
    public Sprite[] Files => m_files;

    [SerializeField] EnemyGroup[] m_enemyGroup;
    public EnemyGroup[] EnemyGroup => m_enemyGroup;
}
