using UnityEngine;

public class Enemy002 : EnemyBase, IPoolable
{
    private Vector3 m_move = Vector3.zero;

    public ObjectType BaseObjectType { get; set; } = ObjectType.Enemy002;

    /// <summary>
    /// 初期化
    /// </summary>
    public override void Init(float px, Vector3 target)
    {
        EnemyBaseInitialize();

        Transform.position = new Vector3(px, GameInfo.Instance.ScreenBound.y + BoundSize.y, 0.0f);

        m_move = new Vector3(0.0f, Speed, 0.0f);
    }

    /// <summary>
    /// 移動＆動作
    /// </summary>
    public override void Move()
    {
        Transform.position += m_move;
    }
}
