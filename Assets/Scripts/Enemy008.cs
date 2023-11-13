using UnityEngine;

public class Enemy008 : EnemyBase, IPoolable
{
    private bool m_turn = false;
    private Vector3 m_move = Vector3.zero;

    public ObjectType BaseObjectType { get; set; } = ObjectType.Enemy008;

    /// <summary>
    /// 初期化
    /// </summary>
    public override void Init(float px, Vector3 target)
    {
        EnemyBaseInitialize();

        Transform.position = new Vector3(px, GameInfo.Instance.ScreenBound.y + BoundSize.y, 0.0f);

        m_turn = false;
        m_move = Vector3.down * Speed;
    }

    /// <summary>
    /// 移動＆動作
    /// </summary>
    public override void Move()
    {
        Transform.position += m_move;

        if (!m_turn)
        {
            if (GameInfo.Instance.Player != null)
            {
                if (Transform.position.y <= GameInfo.Instance.Player.Transform.position.y)
                {
                    if (Transform.position.x <= 0)
                    {
                        m_move = Vector3.right * Speed;
                    }
                    else
                    {
                        m_move = Vector3.left * Speed;
                    }
                    Transform.rotation = Quaternion.FromToRotation(Vector3.up, m_move);

                    m_turn = true;
                }
            }
        }
    }
}
