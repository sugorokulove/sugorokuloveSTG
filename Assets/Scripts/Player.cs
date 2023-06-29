using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class Player : MonoBehaviour
{
    [SerializeField] SpriteFlash flash;

    private int state = 0;
    private bool noDamage = false;

    // Start is called before the first frame update
    void Start()
    {
        state = 1;
        noDamage = true;
        transform.localPosition = new Vector3(0.0f, -200.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        var position = transform.position;

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
        }

        transform.position = position;
    }

    void Damage()
    {
        if (!noDamage)
        {

        }
    }
}
