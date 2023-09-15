using System;
using UnityEngine;

public class Enemy005 : EnemyBase
{
    [SerializeField] private float m_x;     // 横幅
    [SerializeField] private float m_a;     // カーブ調整
    [SerializeField] private float m_p;     // X方向のズレ
    [SerializeField] private float m_q;     // Y方向のズレ

    private bool m_turn = false;            // 方向転換フラグ

    /// <summary>
    /// 初期化
    /// </summary>
    public override void Init(float px, Vector3 target)
    {
        Initialize();

        m_turn = false;

        // 自機の位置により出現画面を自機と逆画面に設定する
        // 速度も併せて調整する必要あり
        if (GameInfo.Instance.Player.Transform.position.x >= 0.0f)
        {
            m_x = -Math.Abs(m_x);
            Speed = Mathf.Abs(Speed);
        }
        else
        {
            m_x = Math.Abs(m_x);
            Speed = -Mathf.Abs(Speed);
        }

        Transform.position = new Vector3(m_x, GameInfo.Instance.ScreenBound.y + BoundSize.y, 0.0f);
    }

    /// <summary>
    /// 移動＆動作
    /// </summary>
    public override void Move()
    {
        var position = Transform.position;

        position.x += Speed;

        float px = position.x - m_p;
        position.y = m_a * (px * px) + m_q;

        Transform.position = position;

        if (m_turn)
        {
            // 反転後、画面上部の画面外判定を行う
            if (Transform.position.y >= (GameInfo.Instance.ScreenBound.y + BoundSize.y))
            {
                Destroy(gameObject);
            }
        }
        else
        {
            // 反転の判定
            if (((Speed >= 0) && (position.x >= 0)) || ((Speed < 0) && (position.x <= 0)))
            {
                m_turn = true;
                Transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
        }
    }
}
