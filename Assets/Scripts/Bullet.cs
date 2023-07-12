using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Bullet : MonoBehaviour
{
    [SerializeField] Sprite[] costumes;                 // パワーアップ画像

    private const float BulletBoundWidth = 32.0f;       // 弾境界の幅
    private const float BulletBoundHeight = 32.0f;      // 弾境界の高

    private const float Speed = 1.0f;                   // 弾の速度

    // Start is called before the first frame update
    void Start()
    {
        Assert.IsTrue(costumes.Length == GameInfo.PowerUpMax, $"costumeは{GameInfo.PowerUpMax}個、値が設定されている必要があります。");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0.0f, Speed, 0.0f);
        if (transform.position.y >= (GameInfo.ScreenBoundHeight + BulletBoundHeight))
        {
            GameInfo.BulletCount--;
            if (GameInfo.BulletCount <= 0)
            {
                GameInfo.BulletCount = 0;
            }
            Destroy(gameObject);
        }
    }
}
