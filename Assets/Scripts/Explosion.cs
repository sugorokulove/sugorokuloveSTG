using UnityEngine;

public class Explosion : MonoBehaviour
{
    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="position">生成位置</param>
    public void Initialize(Vector3 position)
    {
        transform.position = position;
    }

    /// <summary>
    /// アニメーション終了時の関数
    /// </summary>
    void OnExplosionExit()
    {
        Destroy(gameObject);
    }
}
