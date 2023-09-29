using UnityEngine;

public abstract class EnemyBase : ObjectBase
{
    [SerializeField] int m_hp;                              // HP
    [SerializeField] bool m_isAttack;                       // 攻撃の有無
    [SerializeField] bool m_snipe;                          // 狙撃弾にするか？
    [SerializeField] SpriteFlash m_flash;                   // 白点滅用
    [SerializeField] private Countdown m_countdown;         // 攻撃用カウントダウン

    public abstract void Init(float px, Vector3 target);    // 初期化
    public abstract void Move();                            // 移動

    public EnemyGroup MemberGroup { get; set; }             // 所属しているグループ

    private bool m_judgeScreen = false;                     // 画面内外判定用

    void Start()
    {
        m_judgeScreen = false;

        if (m_isAttack)
        {
            m_countdown.Initialize(() => GenerateMissile());
        }
    }

    /// <summary>
    /// 更新
    /// </summary>
    void Update()
    {
        Move();

        // 画面内外判定
        if (!m_judgeScreen)
        {
            JudgeInScreen();
        }
        else
        {
            JudgeOutScreen();
        }
    }

    // 画面内判定
    private void JudgeInScreen()
    {
        if (Transform.position.x >= -(GameInfo.Instance.ScreenBound.x + BoundSize.x) &&
            Transform.position.x <= (GameInfo.Instance.ScreenBound.x + BoundSize.x) &&
            Transform.position.y >= -(GameInfo.Instance.ScreenBound.y + BoundSize.y) &&
            Transform.position.y <= (GameInfo.Instance.ScreenBound.y + BoundSize.y))
        {
            m_judgeScreen = true;
        }
    }

    // 画面外判定
    private void JudgeOutScreen()
    {
        if (JudgeOutOfScreenLeft() ||
            JudgeOutOfScreenRight() ||
            JudgeOutOfScreenTop() ||
            JudgeOutOfScreenBottom())
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// ダメージ処理
    /// </summary>
    /// <param name="power">攻撃力(ダメージ値)</param>
    public void Damage(int power)
    {
        m_flash.FlashLoop(3);

        m_hp -= power;
        if (m_hp <= 0)
        {
            ItemGenerateCheck();
            GenerateExplosion();
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// アイテム生成チェック
    /// </summary>
    void ItemGenerateCheck()
    {
        if (MemberGroup.IsItem)
        {
            MemberGroup.ItemCount--;
            if (MemberGroup.ItemCount <= 0)
            {
                GenerateItem();
            }
        }
    }

    /// <summary>
    /// アイテム生成
    /// </summary>
    void GenerateItem()
    {
        var prefab = Resources.Load<GameObject>("Prefabs/Item/Item");
        var item = Instantiate(prefab);
        item.GetComponent<Item>().Init(new Vector3(Transform.position.x, Transform.position.y));
    }

    /// <summary>
    /// 爆発の生成
    /// </summary>
    void GenerateExplosion()
    {
        var prefab = Resources.Load<GameObject>("Prefabs/Explosion/EnemyExplosion");
        var explosion = Instantiate(prefab);
        explosion.GetComponent<Explosion>().Initialize(Transform.position);
    }

    /// <summary>
    /// ミサイルの生成
    /// </summary>
    public void GenerateMissile()
    {
        var move = Transform.up;

        if (m_snipe)
        {
            if (GameInfo.Instance.Player != null)
            {
                move = (GameInfo.Instance.Player.Transform.position - Transform.position).normalized;
            }
        }

        var prefab = Resources.Load<GameObject>("Prefabs/Missile/Missile");
        var missile = Instantiate(prefab);
        missile.GetComponent<Missile>().Init(Transform.position, move);
    }
}
