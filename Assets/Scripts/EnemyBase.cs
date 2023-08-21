using UnityEngine;

public abstract class EnemyBase : ObjectBase
{
    [SerializeField] SpriteFlash m_flash;       // 白点滅用
    public SpriteFlash Flash => m_flash;

    public int Hp { get; set; } = 0;

    public abstract void Move();

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
        Hp -= power;
        if (Hp <= 0)
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
