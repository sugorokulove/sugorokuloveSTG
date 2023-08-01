using UnityEngine;

public class Enemy001 : EnemyBase
{
    [SerializeField] private Countdown m_countdown;

    private Vector3 m_position = Vector3.zero;

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize()
    {
        Initialize(0.0f);

        Hp = 1;
        m_position = new Vector3(Random.Range(-100.0f, 100.0f), Random.Range(-100.0f, 100.0f));
        m_countdown.Initialize(() => GenerateMissile());
    }

    /// <summary>
    /// 移動＆動作
    /// </summary>
    public override void Move()
    {
        Transform.position = m_position;
    }
}
