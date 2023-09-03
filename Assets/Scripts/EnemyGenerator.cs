using System.Collections;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    private int m_generatorIndex = 0;       // 監視している敵グループ

    void Awake()
    {
        m_generatorIndex = 0;
    }

    void Update()
    {
        if (m_generatorIndex < GameInfo.Instance.StageEnemyGroup.Length)
        {
            // 移動距離を監視して敵グループを生成する
            if (GameInfo.Instance.StageEnemyGroup[m_generatorIndex].Distance >= GameInfo.Instance.StageMove)
            {
                StartCoroutine(GenerateGroup(GameInfo.Instance.StageEnemyGroup[m_generatorIndex]));
                m_generatorIndex++;
            }
        }
    }

    /// <summary>
    /// 敵グループ生成(非同期)
    /// </summary>
    /// <param name="generator">グループ情報</param>
    /// <returns></returns>
    IEnumerator GenerateGroup(EnemyGroup generator)
    {
        int count = generator.Count;
        float distance = generator.Distance;

        while (count > 0)
        {
            if ((GameInfo.Instance.StageMove - distance) >= generator.Interval)
            {
                GenerateEnemy(generator.Type);
                distance += generator.Interval;
                count--;
            }
            yield return null;
        }
    }

    /// <summary>
    /// 敵の生成
    /// </summary>
    void GenerateEnemy(EnemyType type)
    {
        var prefab = Resources.Load<GameObject>($"Prefabs/Plane/{type.ToString()}");
        var enemy = UnityEngine.GameObject.Instantiate(prefab);
        enemy.GetComponent<Enemy001>().Initialize();
    }
}
