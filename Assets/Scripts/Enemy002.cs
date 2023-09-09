using UnityEngine;

public class Enemy002 : EnemyBase
{
    private Vector3 m_position = Vector3.zero;      // 位置
    public override Vector3 Position { get => m_position; set => m_position = value; }

    /// <summary>
    /// 初期化
    /// </summary>
    public override void Init()
    {
        Initialize();
    }

    /// <summary>
    /// 移動＆動作
    /// </summary>
    public override void Move()
    {
        m_position.y += Speed;

        if (m_position.y <= -(GameInfo.Instance.ScreenBound.y + BoundSize.y))
        {
            Destroy(gameObject);
        }

        Transform.position = m_position;
    }
}
