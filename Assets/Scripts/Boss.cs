using UnityEngine;

public class Boss : EnemyBase
{
    // 移動方向設定
    private static readonly Vector3[] NextMove = new Vector3[8]
    {
            new (0.0f,  0.0f, 0.0f),        // 00 : なし
            new (0.0f, -1.0f, 0.0f),        // 01 : 登場
            new (0.0f,  1.0f, 0.0f),        // 02 : 上
            new (0.0f, -1.0f, 0.0f),        // 03 : 下
            new (-1.0f, 0.0f, 0.0f),        // 04 : 左
            new ( 1.0f, 0.0f, 0.0f),        // 05 : 右
            new ( 1.0f, 0.0f, 0.0f),        // 06 : 右
            new (-1.0f, 0.0f, 0.0f)         // 07 : 左
    };

    [SerializeField] private Cannon[] m_cannons;

    private int m_state = 0;                        // 動作管理
    private Vector3 m_move = Vector3.zero;          // 移動量

    /// <summary>
    /// 初期化
    /// </summary>
    public override void Init(float px, Vector3 target)
    {
        Initialize();
        SetSize(0.5f);

        Transform.position = new Vector3(px, GameInfo.Instance.ScreenBound.y + BoundSize.y, 0.0f);

        m_state = 1;
        m_move = Vector3.down * Speed;
    }

    /// <summary>
    /// 移動＆動作
    /// </summary>
    public override void Move()
    {
        switch (m_state)
        {
            case 1:     // 登場
                if (Transform.position.y <= 40.0f)
                {
                    foreach (var cannon in m_cannons)
                    {
                        cannon.State = Cannon.StateType.SelectAction;
                    }

                    Speed = 0.5f;
                    CreateDirection(0.0f, 40.0f);
                }
                break;
            case 2:     // 上下の上昇
                if (Transform.position.y >= 100.0f)
                {
                    CreateDirection(0.0f, 100.0f);
                }
                break;
            case 3:     // 上下の下降
                if (Transform.position.y <= 40.0f)
                {
                    CreateDirection(0.0f, 40.0f);
                }
                break;
            case 4:     // 左側の往路
                if (Transform.position.x <= -200.0f)
                {
                    CreateDirection(-200.0f, 40.0f);
                }
                break;
            case 5:     // 左側の復路
                if (Transform.position.x >= 0.0f)
                {
                    CreateDirection(0.0f, 40.0f);
                }
                break;
            case 6:     // 右側の往路
                if (Transform.position.x >= 200.0f)
                {
                    CreateDirection(200.0f, 40.0f);
                }
                break;
            case 7:     // 右側の復路
                if (Transform.position.x <= 0.0f)
                {
                    CreateDirection(0.0f, 40.0f);
                }
                break;
        }

        Transform.position += m_move;
    }

    /// <summary>
    /// 移動方向作成
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    private void CreateDirection(float x, float y)
    {
        if ((m_state % 2) == 0)
        {
            m_state++;
        }
        else
        {
            m_state = Random.Range(1, 4) * 2;
        }

        m_move = NextMove[m_state] * Speed;
        Transform.position = new Vector3(x, y, 0.0f);
    }
}
