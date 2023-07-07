using System.Collections;
using System.Collections.Generic;

public static class GameInfo
{
    // 定数
    public const float ScreenBoundWidth = (480.0f / 2.0f);      // 画面境界座標：幅
    public const float ScreenBoundHeight = (360.0f / 2.0f);     // 画面境界座標：高
    public const int BulletMax = 5;                             // 画面内弾生成制限数

    // 変数
    public static int BulletCount { get; set; } = 0;            // 弾生成カウント
}
