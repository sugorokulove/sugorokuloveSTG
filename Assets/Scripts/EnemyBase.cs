using UnityEngine;

public abstract class EnemyBase : ObjectBase
{
    enum AttackType
    {
        None = 0,       // 00:攻撃なし
        Down,           // 01:真下
        Direction,      // 02:進行方向
        Snipe           // 03:狙撃
    }

    [SerializeField] int m_hp;                              // HP設定用
    [SerializeField] int m_score;                           // スコア
    [SerializeField] AttackType m_type;                     // 攻撃方法
    [SerializeField] SpriteFlash m_flash;                   // 白点滅用
    [SerializeField] private Countdown m_countdown;         // 攻撃用カウントダウン

    public abstract void Init(float px, Vector3 target);    // 初期化
    public abstract void Move();                            // 移動

    public EnemyGroup MemberGroup { get; set; }             // 所属しているグループ

    private int m_hpNow = 0;                                // HP現在値
    private bool m_judgeScreen = false;                     // 画面内外判定用

    public void EnemyBaseInitialize()
    {
        ObjectBaseInitialize();

        m_flash.Reset();

        m_hpNow = m_hp;
        m_judgeScreen = false;

        if (m_type != AttackType.None)
        {
            m_countdown.Initialize(() => GenerateMissile());
        }

        Transform.rotation = Quaternion.FromToRotation(Vector3.up, Vector3.down);
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
            ObjectPoolManager.Instance.Return(this);
        }
    }

    /// <summary>
    /// ダメージ処理
    /// </summary>
    /// <param name="power">攻撃力(ダメージ値)</param>
    public void Damage(int power)
    {
        m_flash.FlashLoop(3);

        if (m_hpNow > 0)
        {
            m_hpNow -= power;
            if (m_hpNow <= 0)
            {
                m_hpNow = 0;
                UpdateScore();
                ItemGenerateCheck();
                ResourceGenerator.GenerateEnemyExplosion(Transform.position);
                ObjectPoolManager.Instance.Return(this);
            }
        }
    }

    /// <summary>
    /// スコア更新
    /// </summary>
    public void UpdateScore()
    {
        UIManager.Instance.UpdateScore(m_score);
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
                ResourceGenerator.GenerateItem(Transform.position);
            }
        }
    }

    /// <summary>
    /// ミサイルの生成
    /// </summary>
    public void GenerateMissile()
    {
        var move = Vector3.down;

        switch (m_type)
        {
            case AttackType.Down:
                move = Vector3.down;
                break;
            case AttackType.Direction:
                move = Transform.up;
                break;
            case AttackType.Snipe:
                if (GameInfo.Instance.Player != null)
                {
                    move = (GameInfo.Instance.Player.Transform.position - Transform.position).normalized;
                }
                break;
        }

        ResourceGenerator.GenerateMissile(Transform.position, move);
    }
}
