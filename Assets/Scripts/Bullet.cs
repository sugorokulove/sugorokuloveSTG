using UnityEngine;
using UnityEngine.Assertions;

public class Bullet : ObjectBase
{
    [SerializeField] Sprite[] m_costumes;           // パワーアップ画像

    private Vector3 m_position = Vector3.zero;      // 弾の位置・座標

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="position">初期位置・座標</param>
    /// <param name="power">自機のパワーアップ回数による画像指定</param>
    public void Initialize(Vector3 position, int power)
    {
        Assert.IsTrue(m_costumes.Length == GameInfo.PowerType, $"costumeは{GameInfo.PowerType}個、値が設定されている必要があります。");

        Initialize(1.25f);

        m_position = position;

        SpriteRenderer.sprite = m_costumes[power];
        SetSize();
    }

    /// <summary>
    /// 更新
    /// </summary>
    void Update()
    {
        m_position.y += Speed;

        if (m_position.y >= (GameInfo.Instance.ScreenBound.y + BoundSize.y))
        {
            GameInfo.Instance.BulletCount--;
            GameInfo.Instance.BulletCount = Mathf.Max(GameInfo.Instance.BulletCount, 0);
            Destroy(gameObject);
        }

        Transform.position = m_position;
    }
}
