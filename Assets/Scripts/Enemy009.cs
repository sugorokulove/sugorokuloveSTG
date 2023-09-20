using UnityEngine;

public class Enemy009 : EnemyBase
{
    private int m_state = 0;                        // 動作管理
    private int m_time = 0;                         // 待機時間
    private Vector3 m_move = Vector3.zero;          // 移動量
    private Vector3 m_relay = Vector3.zero;         // 中継地点の保存用

    /// <summary>
    /// 初期化
    /// </summary>
    public override void Init(float px, Vector3 target)
    {
        Initialize();

        Transform.position = new Vector3(px, GameInfo.Instance.ScreenBound.y + BoundSize.y, 0.0f);

        m_state = 0;
        m_time = 0;
        m_move = (target - Transform.position).normalized * Speed;
        m_relay = target;

        Transform.rotation = Quaternion.FromToRotation(Vector3.up, m_move);
    }

    /// <summary>
    /// 移動＆動作
    /// </summary>
    public override void Move()
    {
        switch(m_state)
        {
            case 0:
                Transform.position += m_move;
                if (Transform.position.y <= m_relay.y)
                {
                    m_state++;
                    m_time = Random.Range(120, 180);
                    m_move = Vector3.down * Speed;
                    Transform.position = m_relay;
                    Transform.rotation = Quaternion.FromToRotation(Vector3.up, m_move);
                }
                break;
            case 1:
                m_time--;
                if (m_time <= 0)
                {
                    m_state++;
                    m_time = Random.Range(180, 240);
                    GenerateMissile();
                }
                break;
            case 2:
                m_time--;
                if (m_time <= 0)
                {
                    m_state++;
                }
                break;
            case 3:
                Transform.position += m_move;
                break;
        }
    }
}
