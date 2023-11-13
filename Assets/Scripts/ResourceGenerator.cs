using UnityEngine;

public static class ResourceGenerator
{
    /// <summary>
    /// 自機の生成
    /// </summary>
    public static Player GeneratePlayer()
    {
        var player = ObjectPoolManager.Instance.Rent(ObjectType.Player) as Player;
        player.Init();
        return player;
    }

    /// <summary>
    /// 残機の生成
    /// </summary>
    public static Stock GenerateStock()
    {
        var stock = ObjectPoolManager.Instance.Rent(ObjectType.Stock).GetComponent<Stock>();
        UIManager.Instance.AddStock(stock.gameObject);
        return stock;
    }

    /// <summary>
    /// 弾生成
    /// </summary>
    public static void GenerateBullet(Vector3 position)
    {
        var bullet = ObjectPoolManager.Instance.Rent(ObjectType.Bullet) as Bullet;
        bullet.Init(position, GameInfo.Instance.PowerUpCount);

        GameInfo.Instance.BulletCount++;
        GameInfo.Instance.BulletCount = Mathf.Min(GameInfo.Instance.BulletCount, GameInfo.BulletMax);
    }

    /// <summary>
    /// 自機の爆発の生成
    /// </summary>
    public static void GeneratePlayerExplosion(Vector3 position)
    {
        var explostion = ObjectPoolManager.Instance.Rent(ObjectType.PlayerExplosion) as PlayerExplosion;
        explostion.Initialize(position);
    }

    /// <summary>
    /// 敵の生成
    /// </summary>
    public static void GenerateEnemy(EnemyGroup group, int px, Vector3 target)
    {
        var enemy = ObjectPoolManager.Instance.Rent(group.EnemyType) as EnemyBase;
        enemy.Init(px * 40.0f, target);
        enemy.MemberGroup = group;
    }

    /// <summary>
    /// 爆発の生成
    /// </summary>
    public static void GenerateEnemyExplosion(Vector3 position)
    {
        var explostion = ObjectPoolManager.Instance.Rent(ObjectType.EnemyExplosion) as EnemyExplosion;
        explostion.Initialize(position);
    }

    /// <summary>
    /// ミサイルの生成
    /// </summary>
    public static void GenerateMissile(Vector3 position, Vector3 move)
    {
        var missile = ObjectPoolManager.Instance.Rent(ObjectType.Missile) as Missile;
        missile.Init(position, move);
    }

    /// <summary>
    /// レーザーの生成
    /// </summary>
    public static void GenerateLaser(Vector3 position, int index)
    {
        var laser = ObjectPoolManager.Instance.Rent(ObjectType.Laser) as Laser;
        laser.Init(position);
        laser.CannonIndex = index;
    }

    /// <summary>
    /// アイテム生成
    /// </summary>
    public static void GenerateItem(Vector3 position)
    {
        var item = ObjectPoolManager.Instance.Rent(ObjectType.Item) as Item;
        item.Init(position);
    }
}
