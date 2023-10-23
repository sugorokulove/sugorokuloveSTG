using UnityEngine;
using UnityEngine.Assertions;

public class Player : ObjectBase
{
    private const int ShootWaitTime = 40;               // 弾の間隔値
    private const float PowerUpSpeed = 0.25f;           // 自機の速度調整

    [SerializeField] SpriteFlash m_flash;               // 白点滅用
    [SerializeField] Sprite[] m_costumes;               // コスチューム画像

    private int m_state = 0;                            // 状態
    private bool m_isDamage = false;                    // 無敵判定
    private int m_shootWait = 0;                        // 弾の間隔用
    private float m_speedMin, m_speedMax;               // 自機の速度の最小値/最大値

    public bool IsDamage { get => m_isDamage; set => m_isDamage = value; }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Init()
    {
        Assert.IsTrue(m_costumes.Length == GameInfo.PowerType, $"costumeは{GameInfo.PowerType}個、値が設定されている必要があります。");

        Initialize();

        m_state = 1;
        m_isDamage = false;
        m_shootWait = 0;
        m_speedMin = Speed;
        m_speedMax = Speed + PowerUpSpeed * GameInfo.PowerMax;

        SetImageByPower();

        Transform.position = new Vector3(0.0f, -(GameInfo.Instance.ScreenBound.y + BoundSize.y), 0.0f);
    }

    /// <summary>
    /// 更新
    /// </summary>
    void Update()
    {
        DebugControl();

        switch (m_state)
        {
            case 1:
                PlayerEntry();
                break;
            case 2:
                PlayerControl();
                break;
        }
    }

    /// <summary>
    /// 自機と何かの当たり判定
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // アイテム
        if (collision.TryGetComponent<Item>(out var item))
        {
            PowerUp();
            item.Remove();
        }

        // 敵
        if (collision.TryGetComponent<EnemyBase>(out var enemy))
        {
            Damage(100);

            // ボスは除外
            if (!(enemy is Boss))
            {
                enemy.Damage(100);
            }
        }
    }

    /// <summary>
    /// 自機の登場演出
    /// </summary>
    void PlayerEntry()
    {
        var position = Transform.position;

        position.y++;
        if (position.y >= -120)
        {
            m_state = 2;
            m_isDamage = true;
        }
        m_flash.Flash();

        Transform.position = position;
    }

    /// <summary>
    /// 自機操作
    /// </summary>
    void PlayerControl()
    {
        var position = Transform.position;

        var direction = Vector3.zero;

        // キーボード操作
        if (Input.GetKey(KeyCode.D))
        {
            direction.x += Speed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction.x -= Speed;
        }
        if (Input.GetKey(KeyCode.W))
        {
            direction.y += Speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction.y -= Speed;
        }

        position += direction.normalized * Speed;

        // 画面内移動範囲
        if (position.x <= -(GameInfo.Instance.ScreenBound.x - BoundSize.x))
        {
            position.x = -(GameInfo.Instance.ScreenBound.x - BoundSize.x);
        }
        if (position.x >= (GameInfo.Instance.ScreenBound.x - BoundSize.x))
        {
            position.x = (GameInfo.Instance.ScreenBound.x - BoundSize.x);
        }
        if (position.y <= -(GameInfo.Instance.ScreenBound.y - BoundSize.y))
        {
            position.y = -(GameInfo.Instance.ScreenBound.y - BoundSize.y);
        }
        if (position.y >= (GameInfo.Instance.ScreenBound.y - BoundSize.y))
        {
            position.y = (GameInfo.Instance.ScreenBound.y - BoundSize.y);
        }

        // 弾発射
        m_shootWait--;
        if ((m_shootWait <= 0) && (GameInfo.Instance.BulletCount < GameInfo.BulletMax))
        {
            m_shootWait = 0;

            if (Input.GetKey(KeyCode.Space))
            {
                m_shootWait = ShootWaitTime;

                ResourceGenerator.GenerateBullet(
                    new Vector3(Transform.position.x, Transform.position.y + 10));
            }
        }

        Transform.position = position;
    }

    /// <summary>
    /// デバッグ操作
    /// </summary>
    void DebugControl()
    {
        // アイテム生成
        if (Input.GetKeyDown(KeyCode.I))
        {
            ResourceGenerator.GenerateItem(
                new Vector3(Transform.position.x + Random.Range(-10, 10), Transform.position.y + 50));
        }

        // パワーアップ
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            PowerUp();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            PowerDown(1);
        }
    }

    /// <summary>
    /// パワーアップ回数によるsprite設定
    /// </summary>
    void SetImageByPower()
    {
        SpriteRenderer.sprite = m_costumes[GameInfo.Instance.PowerUpCount];
        SetSize(0.5f);
    }
    
    /// <summary>
    /// パワーアップ処理
    /// </summary>
    void PowerUp()
    {
        GameInfo.Instance.PowerUpCount = Mathf.Min(GameInfo.PowerMax, GameInfo.Instance.PowerUpCount + 1);
        SetImageByPower();
        Speed = Mathf.Min(m_speedMax, Speed + PowerUpSpeed);
    }

    /// <summary>
    /// パワーダウン処理
    /// </summary>
    bool PowerDown(int power)
    {
        int result = GameInfo.Instance.PowerUpCount - power;
        GameInfo.Instance.PowerUpCount = Mathf.Max(0, result);
        SetImageByPower();
        Speed = Mathf.Max(m_speedMin, Speed - PowerUpSpeed);

        return result < 0;
    }

    /// <summary>
    /// ダメージ処理
    /// </summary>
    /// <param name="power">攻撃力(ダメージ値)</param>
    public void Damage(int power)
    {
        if (!m_isDamage) return;

        if (PowerDown(power))
        {
            m_state = 3;
            m_isDamage = false;
            ResourceGenerator.GeneratePlayerExplosion(Transform.position);
            GameInfo.Instance.MainGame.ReStart();
            Destroy(gameObject);
        }
    }
}
