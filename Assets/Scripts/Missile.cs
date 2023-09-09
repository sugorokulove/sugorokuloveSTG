using UnityEngine;

public class Missile : ObjectBase
{
    private Vector3 m_position = Vector3.zero;      // 弾の位置・座標

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="position">初期位置・座標</param>
    public void Init(Vector3 position)
    {
        Initialize();

        m_position = position;
    }

    /// <summary>
    /// 更新
    /// </summary>
    void Update()
    {
        m_position.y += Speed;

        if (m_position.y <= -(GameInfo.Instance.ScreenBound.y + BoundSize.y))
        {
            Destroy(gameObject);
        }

        Transform.position = m_position;
    }

    /// <summary>
    /// 弾と何かの当たり判定
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 敵
        if (collision.TryGetComponent<Player>(out var player))
        {
            player.Damage(1);
            Destroy(gameObject);
        }
    }
}
