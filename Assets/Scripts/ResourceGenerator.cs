using UnityEngine;

public static class ResourceGenerator
{
    /// <summary>
    /// リソースの読み込み共通
    /// </summary>
    /// <param name="filename">ファイル名</param>
    /// <returns>GameObject</returns>
    private static GameObject ResoucesLoad(string filename)
    {
        var prefab = Resources.Load<GameObject>(filename);
        return GameObject.Instantiate(prefab);
    }

    /// <summary>
    /// 自機の生成
    /// </summary>
    public static Player GeneratePlayer()
    {
        var player = ResoucesLoad("Prefabs/Plane/Player").GetComponent<Player>();
        player.Init();
        return player;
    }

    /// <summary>
    /// 残機の生成
    /// </summary>
    public static GameObject GenerateStock(Vector3 position)
    {
        var stock = ResoucesLoad("Prefabs/UI/Stock");
        stock.transform.position = position;
        return stock;
    }

    /// <summary>
    /// 弾生成
    /// </summary>
    public static void GenerateBullet(Vector3 position)
    {
        ResoucesLoad("Prefabs/Bullet/Bullet")
            .GetComponent<Bullet>()
            .Init(position, GameInfo.Instance.PowerUpCount);

        GameInfo.Instance.BulletCount++;
        GameInfo.Instance.BulletCount = Mathf.Min(GameInfo.Instance.BulletCount, GameInfo.BulletMax);
    }

    /// <summary>
    /// 自機の爆発の生成
    /// </summary>
    public static void GeneratePlayerExplosion(Vector3 position)
    {
        ResoucesLoad("Prefabs/Explosion/PlayerExplosion")
            .GetComponent<Explosion>()
            .Initialize(position);
    }

    /// <summary>
    /// 敵の生成
    /// </summary>
    public static void GenerateEnemy(EnemyGroup group, int px, Vector3 target)
    {
        var enemy = ResoucesLoad($"Prefabs/Plane/{group.EnemyType.ToString()}").GetComponent<EnemyBase>();
        enemy.Init(px * 40.0f, target);
        enemy.MemberGroup = group;
    }

    /// <summary>
    /// 爆発の生成
    /// </summary>
    public static void GenerateEnemyExplosion(Vector3 position)
    {
        ResoucesLoad("Prefabs/Explosion/EnemyExplosion")
            .GetComponent<Explosion>()
            .Initialize(position);
    }

    /// <summary>
    /// ミサイルの生成
    /// </summary>
    public static void GenerateMissile(Vector3 position, Vector3 move)
    {
        ResoucesLoad("Prefabs/Missile/Missile")
            .GetComponent<Missile>()
            .Init(position, move);
    }

    /// <summary>
    /// レーザーの生成
    /// </summary>
    public static void GenerateLaser(Vector3 position, int index)
    {
        var laser = ResoucesLoad("Prefabs/Missile/Laser").GetComponent<Laser>();
        laser.Init(position);
        laser.CannonIndex = index;
    }

    /// <summary>
    /// アイテム生成
    /// </summary>
    public static void GenerateItem(Vector3 position)
    {
        ResoucesLoad("Prefabs/Item/Item")
            .GetComponent<Item>()
            .Init(position);
    }

    /// <summary>
    /// ゲームオーバー生成
    /// </summary>
    public static void GenerateGameover()
    {
        ResoucesLoad("Prefabs/UI/Gameover");
        ResoucesLoad("Prefabs/UI/Veil");
    }
}
