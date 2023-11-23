using System.Collections;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private ParticleSystem m_ps = null;

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="position">生成位置</param>
    public void Initialize(Vector3 position)
    {
        m_ps = GetComponent<ParticleSystem>();
        transform.position = position;

        // パーティクル終了を検知する
        StartCoroutine(ParticleWorking());
    }

    /// <summary>
    /// アニメーション終了時の関数
    /// </summary>
    void OnExplosionExit()
    {
        ObjectPoolManager.Instance.Return(this);
    }

    /// <summary>
    /// パーティクル終了時にオブジェクトを返却する
    /// </summary>
    /// <returns></returns>
    IEnumerator ParticleWorking()
    {
        yield return new WaitWhile(() => m_ps.IsAlive(true));
        ObjectPoolManager.Instance.Return(this);
    }
#if false
    /// <summary>
    /// パーティクル終了時に自動で呼ばれる関数
    /// </summary>
    public void OnParticleSystemStopped()
    {
        ObjectPoolManager.Instance.Return(this);
    }
#endif
}
