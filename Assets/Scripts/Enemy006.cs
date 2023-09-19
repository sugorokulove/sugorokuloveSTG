using UnityEngine;

public class Enemy006 : EnemyBase
{
    [SerializeField] private float m_min, m_max;    // 移動距離用乱数の最小値と最大値

    private float m_moveCount = 0.0f;               // 移動距離
    private Vector3 m_move = Vector3.zero;          // 移動量

    /// <summary>
    /// 初期化
    /// </summary>
    public override void Init(float px, Vector3 target)
    {
        Initialize();

        Transform.position = new Vector3(px, GameInfo.Instance.ScreenBound.y + BoundSize.y, 0.0f);

        ChangeDirection(Vector3.down);
    }

    /// <summary>
    /// 移動＆動作
    /// </summary>
    public override void Move()
    {
        Transform.position += m_move;

        m_moveCount--;
        if (m_moveCount <= 0)
        {
            // 縦移動か？
            if (m_move.y != 0.0f)
            {
                // 画面端か否か？
                if (Transform.position.x >= (GameInfo.Instance.ScreenBound.x - BoundSize.x))
                {
                    ChangeDirection(Vector3.left);
                }
                else if (Transform.position.x <= -(GameInfo.Instance.ScreenBound.x - BoundSize.x))
                {
                    ChangeDirection(Vector3.right);
                }
                else
                {
                    if (Random.Range(0,2) == 0)
                    {
                        ChangeDirection(Vector3.left);
                    }
                    else
                    {
                        ChangeDirection(Vector3.right);
                    }
                }
            }
            else
            {
                ChangeDirection(Vector3.down);
            }
        }
    }

    /// <summary>
    /// 進行方向変更
    /// </summary>
    /// <param name="direction">方向</param>
    private void ChangeDirection(Vector3 direction)
    {
        m_moveCount = Random.Range(m_min, m_max);
        m_move = direction * Speed;
        Transform.rotation = Quaternion.FromToRotation(Vector3.up, m_move);
    }
}
