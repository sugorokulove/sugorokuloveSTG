using UnityEngine;

public abstract class EnemyBase : ObjectBase
{
    [SerializeField] int m_hp;                      // HP
    [SerializeField] bool m_isAttack;               // 攻撃の有無
    [SerializeField] SpriteFlash m_flash;           // 白点滅用
    [SerializeField] private Countdown m_countdown; // 攻撃用カウントダウン

    public abstract void Move();

    void Start()
    {
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
            GenerateExplosion();
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 爆発の生成
    /// </summary>
    void GenerateExplosion()
    {
        var prefab = Resources.Load<GameObject>("Prefabs/Explosion/EnemyExplosion");
        var explosion = Instantiate(prefab);
        explosion.GetComponent<Explosion>().Initialize(transform.position);
    }

    /// <summary>
    /// ミサイルの生成
    /// </summary>
    public void GenerateMissile()
    {
        var prefab = Resources.Load<GameObject>("Prefabs/Missile/Missile");
        var missile = Instantiate(prefab);
        missile.GetComponent<Missile>().Initialize(transform.position);
    }
}
