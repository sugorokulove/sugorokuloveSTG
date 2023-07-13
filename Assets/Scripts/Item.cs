using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private const float ItemBoundWidth = 16.0f;       // 弾境界の幅
    private const float ItemBoundHeight = 16.0f;      // 弾境界の高

    private const float Speed = -0.1f;                // 弾の速度

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0.0f, Speed, 0.0f);
        if (transform.position.y <= -(GameInfo.ScreenBoundHeight + ItemBoundHeight))
        {
            Destroy(gameObject);
        }
    }
}
