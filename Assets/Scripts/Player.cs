using UnityEngine;
using UnityEngine.Assertions;

public class Player : ObjectBase, IPoolable
{
    public enum StateType
    {
        None = 0,
        Entry,
        Control,
        Exit,
        GameEnd
    }

    private const int ShootWaitTime = 40;               // 弾の間隔値
    private const float PowerUpSpeed = 0.25f;           // 自機の速度調整

    [SerializeField] SpriteFlash m_flash;               // 白点滅用
    [SerializeField] Sprite[] m_costumes;               // コスチューム画像

    private StateType m_state = StateType.None;         // 状態
    private bool m_isDamage = false;                    // 無敵判定
    private int m_shootWait = 0;                        // 弾の間隔用
    private float m_velocity;                           // 速度変更用

    public bool IsDamage
    {
        get => m_isDamage;
        set => m_isDamage = value;
    }

    public StateType State
    {
        get => m_state;
        set => m_state = value;
    }

    public ObjectType BaseObjectType { get; set; } = ObjectType.Player;

    /// <summary>
    /// 初期化
    /// </summary>
    public void Init()
    {
        Assert.IsTrue(m_costumes.Length == GameInfo.PowerType, $"costumeは{GameInfo.PowerType}個、値が設定されている必要があります。");

        ObjectBaseInitialize();

        m_state = StateType.Entry;
        m_isDamage = false;
        m_shootWait = 0;
        m_flash.Reset();

        SetVelocity();

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
            case StateType.Entry:
                PlayerEntry();
                break;
            case StateType.Control:
                PlayerControl();
                break;
            case StateType.Exit:
                PlayerExit();
                break;
            case StateType.GameEnd:
                PlayerEnd();
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
            m_state = StateType.Control;
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
            direction.x += m_velocity;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction.x -= m_velocity;
        }
        if (Input.GetKey(KeyCode.W))
        {
            direction.y += m_velocity;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction.y -= m_velocity;
        }

        position += direction.normalized * m_velocity;

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
    /// 自機退場
    /// </summary>
    void PlayerExit()
    {
        var position = Transform.position;
        position.y += m_velocity;
        m_velocity += 0.1f;
        Transform.position = position;

        if (position.y > GameInfo.Instance.ScreenBound.y + BoundSize.y)
        {
            State = StateType.None;
            NextStage();
        }
    }

    /// <summary>
    /// ゲームクリア後キー待ち
    /// </summary>
    void PlayerEnd()
    {
        if (Input.anyKeyDown)
        {
            m_state = StateType.None;
            StartCoroutine(GameInfo.Instance.MainGame.TransitionToTitle(0.0f));
        }
    }

    /// <summary>
    /// クリア or 次のステージか？
    /// </summary>
    void NextStage()
    {
        if (GameInfo.Instance.StageNo >= (GameInfo.Instance.StageCount - 1))
        {
            // クリア
            UIManager.Instance.GameClear();
            m_state = StateType.GameEnd;
        }
        else
        {
            // 次のステージ
            GameInfo.Instance.StageNo++;
            GameInfo.Instance.StageMove = 0.0f;
            GameInfo.Instance.BackgroundManager.Initialize();

            // 敵グループのリセット
            GameInfo.Instance.EnemyGenerator.Reset();

            // 自機の初期化
            m_state = StateType.Entry;
            m_isDamage = false;
            SetVelocity();
            Transform.position = new Vector3(0.0f, -(GameInfo.Instance.ScreenBound.y + BoundSize.y), 0.0f);
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
        SetVelocity();
    }

    /// <summary>
    /// パワーダウン処理
    /// </summary>
    bool PowerDown(int power)
    {
        int result = GameInfo.Instance.PowerUpCount - power;
        GameInfo.Instance.PowerUpCount = Mathf.Max(0, result);
        SetImageByPower();
        SetVelocity();

        return result < 0;
    }

    /// <summary>
    /// 速度設定
    /// </summary>
    void SetVelocity()
    {
        m_velocity = Speed + GameInfo.Instance.PowerUpCount * PowerUpSpeed;
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
            m_state = StateType.None;
            m_isDamage = false;
            ResourceGenerator.GeneratePlayerExplosion(Transform.position);
            GameInfo.Instance.MainGame.ReStart();
            ObjectPoolManager.Instance.Return(this);
        }
    }
}
