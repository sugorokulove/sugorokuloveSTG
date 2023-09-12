using UnityEngine;

public class Enemy002 : EnemyBase
{
    private Vector3 m_position = Vector3.zero;      // 位置

    /// <summary>
    /// 初期化
    /// </summary>
    public override void Init(float px, Vector3 target)
    {
        Initialize();

        m_position = new Vector3(px, GameInfo.Instance.ScreenBound.y + BoundSize.y, 0.0f);
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
