using UnityEngine;

public class Enemy001 : EnemyBase
{
    [SerializeField] private float m_angleSpeed;    // 角度の変化量
    [SerializeField] private float m_radius;        // 半径(移動幅)

    private Vector3 m_position = Vector3.zero;      // 位置
    private float m_angle = 0.0f;                   // 角度

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize(Vector3 position)
    {
        Initialize();

        position.y += BoundSize.y;
        m_position = position;
        m_angle = 0.0f;
    }

    /// <summary>
    /// 移動＆動作
    /// </summary>
    public override void Move()
    {
        m_position.x = Mathf.Sin(m_angle * Mathf.PI) * m_radius;
        m_position.y += Speed;

        m_angle += m_angleSpeed;

        if (m_position.y <= -(GameInfo.Instance.ScreenBound.y + BoundSize.y))
        {
            Destroy(gameObject);
        }

        Transform.position = m_position;
    }
}
