using UnityEngine;

public class Enemy007 : EnemyBase
{
    [SerializeField] private int m_directionTime = 10;
    [SerializeField] private float m_angleSpeed = 3.0f;

    private int m_time = 0;
    private Vector3 m_move = Vector3.zero;          // 移動量

    /// <summary>
    /// 初期化
    /// </summary>
    public override void Init(float px, Vector3 target)
    {
        Initialize();

        Transform.position = new Vector3(px, GameInfo.Instance.ScreenBound.y + BoundSize.y, 0.0f);

        m_time = m_directionTime;
        m_move = (GameInfo.Instance.Player.Transform.position - Transform.position).normalized * Speed;
        Transform.rotation = Quaternion.FromToRotation(Vector3.up, m_move);
    }

    /// <summary>
    /// 移動＆動作
    /// </summary>
    public override void Move()
    {
        Transform.position += m_move;

        m_time--;
        if (m_time <= 0)
        {
            m_time = m_directionTime;
            HomingMove();
        }
    }

    /// <summary>
    /// 追尾計算
    /// </summary>
    private void HomingMove()
    {
        var direction = GameInfo.Instance.Player.Transform.position - Transform.position;   // 方向(ベクトル)
        var currentAngle = Mathf.Atan2(m_move.y, m_move.x) * Mathf.Rad2Deg;                 // 現在の角度
        var targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;            // ターゲットへの角度
        var deltaAngle = Mathf.DeltaAngle(currentAngle, targetAngle);                       // 角度の差

        // 旋回角度を調整
        var speed = m_angleSpeed;
        if (direction.magnitude <= ((GameInfo.Instance.Player.BoundSize.x + BoundSize.x) * 2.0f))
        {
            speed += 2;
        }

        // 次の角度を計算
        var nextAngle = currentAngle;
        if (Mathf.Abs(deltaAngle) < speed)
        {
        }
        else if (deltaAngle > 0)
        {
            nextAngle += speed;
        }
        else
        {
            nextAngle -= speed;
        }

        // 角度をベクトルに変換
        m_move = new Vector3(Mathf.Cos(nextAngle * Mathf.PI / 180.0f),
                             Mathf.Sin(nextAngle * Mathf.PI / 180.0f),
                             0.0f).normalized * Speed;

        // 画像の向きを変更
        Transform.rotation = Quaternion.FromToRotation(Vector3.up, m_move);
    }
}
