using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class Player : MonoBehaviour
{
    private const float PlayerBoundWidth = (32.0f / 2.0f);      // 自機境界の幅
    private const float PlayerBoundHeight = (24.0f / 2.0f);     // 自機境界の高
    private const int ShootWaitTime = 40;                       // 弾の間隔値

    [SerializeField] SpriteFlash flash;                         // 白点滅用
    [SerializeField] Sprite[] costumes;                         // コスチューム画像

    private int state = 0;                                      // 状態
    private bool noDamage = false;                              // 無敵判定
    private int shootWait = 0;                                  // 弾の間隔用

    private float speed = 0.0f;                                 // 自機の速度
    private Vector3 position = Vector3.zero;                    // 自機の座標位置

    // Start is called before the first frame update
    void Start()
    {
        Assert.IsTrue(costumes.Length == GameInfo.PowerUpMax, $"costumeは{GameInfo.PowerUpMax}個、値が設定されている必要があります。");

        state = 1;
        noDamage = true;
        shootWait = 0;

        speed = 0.5f;
        position = new Vector3(0.0f, -200.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case 1:
                position.y++;
                if (position.y >= -120)
                {
                    state = 2;
                    noDamage = false;
                }
                flash.Flash();
                break;
            case 2:
                PlayerControl();
                break;
        }

        transform.position = position;
    }

    /// <summary>
    /// 自機操作
    /// </summary>
    void PlayerControl()
    {
        // キーボード操作
        if (Input.GetKey(KeyCode.D))
        {
            position.x += speed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            position.x -= speed;
        }
        if (Input.GetKey(KeyCode.W))
        {
            position.y += speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            position.y -= speed;
        }

        // 画面内移動範囲
        if (position.x <= -(GameInfo.ScreenBoundWidth - PlayerBoundWidth))
        {
            position.x = -(GameInfo.ScreenBoundWidth - PlayerBoundWidth);
        }
        if (position.x >= (GameInfo.ScreenBoundWidth - PlayerBoundWidth))
        {
            position.x = (GameInfo.ScreenBoundWidth - PlayerBoundWidth);
        }
        if (position.y <= -(GameInfo.ScreenBoundHeight - PlayerBoundHeight))
        {
            position.y = -(GameInfo.ScreenBoundHeight - PlayerBoundHeight);
        }
        if (position.y >= (GameInfo.ScreenBoundHeight - PlayerBoundHeight))
        {
            position.y = (GameInfo.ScreenBoundHeight - PlayerBoundHeight);
        }

        // 弾発射
        shootWait--;
        if ((shootWait <= 0) && (GameInfo.BulletCount < GameInfo.BulletMax))
        {
            shootWait = 0;

            if (Input.GetKey(KeyCode.Space))
            {
                shootWait = ShootWaitTime;

                Shoot();
            }
        }
    }

    /// <summary>
    /// 弾生成
    /// </summary>
    void Shoot()
    {
        var prefab = Resources.Load<GameObject>("Prefabs/Bullet/Bullet");
        var bullet = Instantiate(prefab);
        bullet.transform.position = position;

        GameInfo.BulletCount++;
        if (GameInfo.BulletCount >= GameInfo.BulletMax)
        {
            GameInfo.BulletCount = GameInfo.BulletMax;
        }
    }

    void Damage()
    {
        if (!noDamage)
        {

        }
    }
}
