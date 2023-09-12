using UnityEngine;

public class Enemy003 : EnemyBase
{
    private Vector3 m_position = Vector3.zero;      // 位置
    private Vector3 m_move = Vector3.zero;          // 移動量

    /// <summary>
    /// 初期化
    /// </summary>
    public override void Init(float px, Vector3 target)
    {
        Initialize();

        m_position = new Vector3(px, GameInfo.Instance.ScreenBound.y + BoundSize.y, 0.0f);
        m_move = (target - m_position).normalized * Speed;
        Transform.rotation = Quaternion.FromToRotation(Vector3.up, m_move);
    }

    /// <summary>
    /// 移動＆動作
    /// </summary>
    public override void Move()
    {
        m_position += m_move;

        if (m_position.y <= -(GameInfo.Instance.ScreenBound.y + BoundSize.y))
        {
            Destroy(gameObject);
        }

        Transform.position = m_position;
    }
}
