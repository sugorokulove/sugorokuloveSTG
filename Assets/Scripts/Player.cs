﻿using UnityEngine;
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

    private Vector3 m_position = Vector3.zero;          // 自機の座標位置

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize(Vector3 position)
    {
        Assert.IsTrue(m_costumes.Length == GameInfo.PowerType, $"costumeは{GameInfo.PowerType}個、値が設定されている必要があります。");

        Initialize();

        m_state = 1;
        m_isDamage = false;
        m_shootWait = 0;
        m_speedMin = Speed;
        m_speedMax = Speed + PowerUpSpeed * GameInfo.PowerMax;

        position.y -= BoundSize.y;
        m_position = position;

        SetImageByPower();
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
                m_position.y++;
                if (m_position.y >= -120)
                {
                    m_state = 2;
                    m_isDamage = true;
                }
                m_flash.Flash();
                break;
            case 2:
                PlayerControl();
                break;
        }

        Transform.position = m_position;
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
            enemy.Damage(100);
        }
    }

    /// <summary>
    /// 自機操作
    /// </summary>
    void PlayerControl()
    {
        // キーボード操作
        if (Input.GetKey(KeyCode.D))
        {
            m_position.x += Speed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            m_position.x -= Speed;
        }
        if (Input.GetKey(KeyCode.W))
        {
            m_position.y += Speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            m_position.y -= Speed;
        }

        // 画面内移動範囲
        if (m_position.x <= -(GameInfo.Instance.ScreenBound.x - BoundSize.x))
        {
            m_position.x = -(GameInfo.Instance.ScreenBound.x - BoundSize.x);
        }
        if (m_position.x >= (GameInfo.Instance.ScreenBound.x - BoundSize.x))
        {
            m_position.x = (GameInfo.Instance.ScreenBound.x - BoundSize.x);
        }
        if (m_position.y <= -(GameInfo.Instance.ScreenBound.y - BoundSize.y))
        {
            m_position.y = -(GameInfo.Instance.ScreenBound.y - BoundSize.y);
        }
        if (m_position.y >= (GameInfo.Instance.ScreenBound.y - BoundSize.y))
        {
            m_position.y = (GameInfo.Instance.ScreenBound.y - BoundSize.y);
        }

        // 弾発射
        m_shootWait--;
        if ((m_shootWait <= 0) && (GameInfo.Instance.BulletCount < GameInfo.BulletMax))
        {
            m_shootWait = 0;

            if (Input.GetKey(KeyCode.Space))
            {
                m_shootWait = ShootWaitTime;

                GenerateBullet();
            }
        }
    }
    
    /// <summary>
    /// デバッグ操作
    /// </summary>
    void DebugControl()
    {
        // アイテム生成
        if (Input.GetKeyDown(KeyCode.I))
        {
            GenerateItem();
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
    /// 弾生成
    /// </summary>
    void GenerateBullet()
    {
        var prefab = Resources.Load<GameObject>("Prefabs/Bullet/Bullet");
        var bullet = Instantiate(prefab);
        bullet.GetComponent<Bullet>().Initialize(
            new Vector3(m_position.x, m_position.y + 10),
            GameInfo.Instance.PowerUpCount);

        GameInfo.Instance.BulletCount++;
        GameInfo.Instance.BulletCount = Mathf.Min(GameInfo.Instance.BulletCount, GameInfo.BulletMax);
    }
    
    /// <summary>
    /// アイテム生成
    /// </summary>
    void GenerateItem()
    {
        var prefab = Resources.Load<GameObject>("Prefabs/Item/Item");
        var item = Instantiate(prefab);
        item.GetComponent<Item>().Initialize(new Vector3(m_position.x + Random.Range(-10, 10), m_position.y + 50));
    }

    /// <summary>
    /// 爆発の生成
    /// </summary>
    void GenerateExplosion()
    {
        var prefab = Resources.Load<GameObject>("Prefabs/Explosion/PlayerExplosion");
        var explosion = Instantiate(prefab);
        explosion.GetComponent<Explosion>().Initialize(m_position);
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
            GenerateExplosion();
            GameInfo.Instance.MainGame.ReStart();
            Destroy(gameObject);
        }
    }
}
