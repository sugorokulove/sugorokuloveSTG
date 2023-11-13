/// <summary>
/// 全オブジェクト
/// </summary>
public enum ObjectType
{
    None = 0,                       // 00:なし

    Player,                         // 01:自機
    Bullet,                         // 02:自機弾
    Item,                           // 03:アイテム
    Enemy001,                       // 04:敵01
    Enemy002,                       // 05:敵02
    Enemy003,                       // 06:敵03
    Enemy004,                       // 07:敵04
    Enemy005,                       // 08:敵05
    Enemy006,                       // 09:敵06
    Enemy007,                       // 10:敵07
    Enemy008,                       // 11:敵08
    Enemy009,                       // 12:敵09
    Boss,                           // 13:ボス
    Missile,                        // 14:ミサイル
    Laser,                          // 15:レーザー
    PlayerExplosion,                // 16:自機爆発
    EnemyExplosion,                 // 17:敵爆発
    Stock                           // 18:残機
}