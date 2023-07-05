using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class Player : MonoBehaviour
{
    [SerializeField] SpriteFlash flash;

    private int state = 0;
    private bool noDamage = false;

    private float speed = 0.0f;
    private Vector3 position = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        state = 1;
        noDamage = true;

        speed = 1.0f;
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
        if (position.y <= -168.0f)
        {
            position.y = -168.0f;
        }
        if (position.y >= 168.0f)
        {
            position.y = 168.0f;
        }
        if (position.x <= -224.0f)
        {
            position.x = -224.0f;
        }
        if (position.x >= 224.0f)
        {
            position.x = 224.0f;
        }
    }

    void Damage()
    {
        if (!noDamage)
        {

        }
    }
}
