using UnityEngine;

public class Enemy001 : EnemyBase
{
    private Vector3 m_position = Vector3.zero;

    public void Initialize()
    {
        Initialize(0.0f);

        Hp = 1;
        m_position = new Vector3(Random.Range(-100.0f, 100.0f), Random.Range(-100.0f, 100.0f));
    }

    public override void Move()
    {
        Transform.position = m_position;
    }
}
