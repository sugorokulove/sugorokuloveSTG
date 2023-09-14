using UnityEngine;

public class Enemy003 : EnemyBase
{
    private Vector3 m_move = Vector3.zero;          // 移動量

    /// <summary>
    /// 初期化
    /// </summary>
    public override void Init(float px, Vector3 target)
    {
        Initialize();

        Transform.position = new Vector3(px, GameInfo.Instance.ScreenBound.y + BoundSize.y, 0.0f);
        m_move = (target - Transform.position).normalized * Speed;
        Transform.rotation = Quaternion.FromToRotation(Vector3.up, m_move);
    }

    /// <summary>
    /// 移動＆動作
    /// </summary>
    public override void Move()
    {
        Transform.position += m_move;
    }
}
