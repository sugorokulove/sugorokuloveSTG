using UnityEngine;

public abstract class EnemyBase : ObjectBase
{
    [SerializeField] SpriteFlash m_flash;       // 白点滅用
    public SpriteFlash lash => m_flash;

    public int Hp { get; set; } = 0;

    public abstract void Move();

    void Update()
    {
        Move();
    }

    public void Damage(int power)
    {
        Hp -= power;
        if (Hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
