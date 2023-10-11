﻿using UnityEngine;

public class Missile : ObjectBase
{
    private Vector3 m_move = Vector3.zero;

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="position">初期位置・座標</param>
    public void Init(Vector3 position, Vector3 direction)
    {
        Initialize();

        m_move = direction * Speed;

        Transform.position = position;
        Transform.rotation = Quaternion.FromToRotation(Vector3.up, m_move);
    }

    /// <summary>
    /// 更新
    /// </summary>
    void Update()
    {
        Transform.position += m_move;

        if (JudgeOutOfScreenLeft() ||
            JudgeOutOfScreenRight() ||
            JudgeOutOfScreenTop() ||
            JudgeOutOfScreenBottom())
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 弾と何かの当たり判定
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 自機
        if (collision.TryGetComponent<Player>(out var player))
        {
            player.Damage(1);
            Destroy(gameObject);
        }
    }
}
