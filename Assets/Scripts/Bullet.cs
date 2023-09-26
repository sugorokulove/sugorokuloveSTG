using UnityEngine;
using UnityEngine.Assertions;

public class Bullet : ObjectBase
{
    [SerializeField] Sprite[] m_costumes;           // パワーアップ画像

    private int m_power = 0;                        // 弾の威力(自機のパワーアップ回数)
    private Vector3 m_move = Vector3.zero;          // 弾の移動量

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="position">初期位置・座標</param>
    /// <param name="power">自機のパワーアップ回数による画像指定</param>
    public void Init(Vector3 position, int power)
    {
        Assert.IsTrue(m_costumes.Length == GameInfo.PowerType, $"costumeは{GameInfo.PowerType}個、値が設定されている必要があります。");

        Initialize();

        Transform.position = position;

        m_power = power + 1;
        m_move = new Vector3(0.0f, Speed, 0.0f);

        SpriteRenderer.sprite = m_costumes[power];
        SetSize();
    }

    /// <summary>
    /// 更新
    /// </summary>
    void Update()
    {
        Transform.position += m_move;

        if (JudgeOutOfScreenTop())
        {
            Remove();
        }
    }

    /// <summary>
    /// 弾と何かの当たり判定
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 敵
        if (collision.TryGetComponent<EnemyBase>(out var enemy))
        {
            enemy.Damage(m_power);
            Remove();
        }
    }

    /// <summary>
    /// 弾の削除
    /// </summary>
    private void Remove()
    {
        GameInfo.Instance.BulletCount--;
        GameInfo.Instance.BulletCount = Mathf.Max(GameInfo.Instance.BulletCount, 0);
        Destroy(gameObject);
    }
}
