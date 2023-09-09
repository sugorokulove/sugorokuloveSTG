using System;
using System.Collections;
using System.Linq;
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

        var px = GeneratePosition(generator.LocationType, generator.Count);
        int index = 0;

        while (count > 0)
        {
            if ((GameInfo.Instance.StageMove - distance) >= generator.Interval)
            {
                GenerateEnemy(generator.EnemyType, px[index]);
                distance += generator.Interval;
                count--;
                index++;
            }
            yield return null;
        }
    }

    /// <summary>
    /// 敵の生成
    /// </summary>
    void GenerateEnemy(EnemyType type, int px)
    {
        var prefab = Resources.Load<GameObject>($"Prefabs/Plane/{type.ToString()}");
        var gameobject = UnityEngine.GameObject.Instantiate(prefab);
        var enemy = gameobject.GetComponent<EnemyBase>();
        enemy.Init();
        enemy.Position = new Vector3(px * 40.0f, GameInfo.Instance.ScreenBound.y + enemy.BoundSize.y, 0.0f);
    }

    /// <summary>
    /// 出現位置座標の作成
    /// </summary>
    /// <param name="type">初期配置タイプ</param>
    /// <param name="count">個数</param>
    /// <returns>位置配列</returns>
    int[] GeneratePosition(LocationType type, int count)
    {
        int[] positions = new int[count];

        switch (type)
        {
            // ゼロ地点のみ
            case LocationType.None:
                positions = Enumerable.Repeat<int>(0, count).ToArray();
                break;
            // 横一列のランダム一点
            case LocationType.RandomPoint:
                int max = count / 2;
                int min = -max;
                positions = Enumerable.Repeat<int>(UnityEngine.Random.Range(min, max), count).ToArray();
                break;
            // 横一列のランダム
            case LocationType.RandomHorizontal:
                positions = Enumerable.Range(-count / 2, count).ToArray();      // 連続の値で埋める
                positions = positions.OrderBy(_ => Guid.NewGuid()).ToArray();   // シャッフル
                break;
        }

        return positions;
    }
}
