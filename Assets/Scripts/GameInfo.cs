using UnityEngine;

public class GameInfo : SingletonMonoBehaviour<GameInfo>
{
    // 定数
    public const int BulletMax = 5;                             // 画面内弾生成制限数
    public const int PowerType = 3;                             // 自機の形状数
    public const int PowerMax = PowerType - 1;                  // パワーアップ回数

    // SerializeField
    [SerializeField] MainGame m_mainGame;                       // MainGameのインスタンス
    public MainGame MainGame => m_mainGame;

    // 変数
    public Vector2 ScreenBound { get; set; } = Vector2.zero;    // 画面境界座標
    public Vector2 ScreenSize { get; set; } = Vector2.zero;     // 画面サイズ
    public int BulletCount { get; set; } = 0;                   // 弾生成カウント
    public int PowerUpCount { get; set; } = 0;                  // パワーアップ回数
    public int StageNo { get; set; } = 0;                       // 現在のステージ数

    protected override void Initialize()
    {
        // 画面境界サイズ作成
        ScreenBound = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        ScreenBound = new Vector2(Mathf.Ceil(ScreenBound.x), Mathf.Ceil(ScreenBound.y));
        ScreenSize = ScreenBound * 2.0f;
    }
}
