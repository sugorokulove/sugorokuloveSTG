using System;
using System.Linq;
using UnityEngine;

public class GameInfo : SingletonMonoBehaviour<GameInfo>
{
    // 定数
    public const int BulletMax = 5;                             // 画面内弾生成制限数
    public const int PowerType = 3;                             // 自機の形状数
    public const int PowerMax = PowerType - 1;                  // パワーアップ回数

    // SerializeField
    [SerializeField] private MainGame m_mainGame;               // MainGameのインスタンス
    public MainGame MainGame => m_mainGame;

    [SerializeField] private int m_stock;                       // 残機数指定
    public int Stock => m_stock;

    [SerializeField] private StageInfo[] m_stages;              // ステージ情報
    public Sprite[] StageBgFiles => m_stages[StageNo].Files;
    public EnemyGroup[] StageEnemyGroup => m_stages[StageNo].EnemyGroup;

    // 変数
    public Vector2 ScreenBound { get; set; } = Vector2.zero;    // 画面境界座標
    public Vector2 ScreenSize { get; set; } = Vector2.zero;     // 画面サイズ
    public int StageNo { get; set; } = 0;                       // 現在のステージ数
    public float StageMove { get; set; } = 0.0f;                // 移動距離
    public int BulletCount { get; set; } = 0;                   // 弾生成カウント
    public int PowerUpCount { get; set; } = 0;                  // パワーアップ回数
    public Player Player { get; set; } = null;                  // Playerクラス

    /// <summary>
    /// 変更があった場合、昇順に並べ替える
    /// </summary>
    private void OnValidate()
    {
        foreach(var stage in m_stages)
        {
            Array.Sort(stage.Files.Select(n => n.name).ToArray(), stage.Files);
            Array.Sort(stage.EnemyGroup.Select(n => n.Distance).ToArray(), stage.EnemyGroup);
        }
    }

    protected override void Initialize()
    {
        StageNo = 0;
        StageMove = 0.0f;
        BulletCount = 0;
        PowerUpCount = 0;

        // 画面境界サイズ作成
        ScreenBound = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        ScreenBound = new Vector2(Mathf.Ceil(ScreenBound.x), Mathf.Ceil(ScreenBound.y));
        ScreenSize = ScreenBound * 2.0f;
    }
}
