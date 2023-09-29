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
            if (GameInfo.Instance.StageMove >= GameInfo.Instance.StageEnemyGroup[m_generatorIndex].Distance)
            {
                StartCoroutine(GenerateGroup(GameInfo.Instance.StageEnemyGroup[m_generatorIndex]));
                m_generatorIndex++;
            }
        }
    }

    /// <summary>
    /// 敵グループ生成(非同期)
    /// </summary>
    /// <param name="group">グループ情報</param>
    /// <returns></returns>
    IEnumerator GenerateGroup(EnemyGroup group)
    {
        int index = 0;
        int count = group.Count;                                            // 生成数
        float distance = group.Distance - group.Interval;                   // 即時生成調整
        int[] entry = GeneratePosition(group.LocationType, group.Count);    // 出現位置
        int[] middle = CreateMiddlePoint();                                 // 中継地点

        var target = Vector3.zero;
        if (group.TargetType == TargetType.First)
        {
            if (GameInfo.Instance.Player != null)
            {
                target = GameInfo.Instance.Player.Transform.position;
            }
        }

        while (count > 0)
        {
            if ((GameInfo.Instance.StageMove - distance) >= group.Interval)
            {
                // 毎回自機の位置をターゲットとする
                if (group.TargetType == TargetType.Every)
                {
                    target = (GameInfo.Instance.Player != null) ? GameInfo.Instance.Player.Transform.position : Vector3.zero;
                }

                // 中継地点をターゲットとする
                if (group.TargetType == TargetType.Middle)
                {
                    target = ConvertMiddlePoint(middle[index]);
                }

                GenerateEnemy(group, entry[index], target);
                distance += group.Interval;
                count--;
                index++;
            }
            yield return null;
        }
    }

    /// <summary>
    /// 敵の生成
    /// </summary>
    void GenerateEnemy(EnemyGroup group, int px, Vector3 target)
    {
        var prefab = Resources.Load<GameObject>($"Prefabs/Plane/{group.EnemyType.ToString()}");
        var gameobject = UnityEngine.GameObject.Instantiate(prefab);
        var enemy = gameobject.GetComponent<EnemyBase>();
        enemy.Init(px * 40.0f, target);
        enemy.MemberGroup = group;
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
                if (GameInfo.Instance.Player != null)
                {
                    int point = GameInfo.LocationArea / 2;
                    if (GameInfo.Instance.Player.Transform.position.x >= 0)
                    {
                        point = UnityEngine.Random.Range(-point, 0);
                    }
                    else
                    {
                        point = UnityEngine.Random.Range(0, point);
                    }
                    positions = Enumerable.Repeat<int>(point, count).ToArray();
                }
                break;
            // 横一列のランダム
            case LocationType.RandomHorizontal:
                // 連続の値で埋めてシャッフル
                positions = Enumerable.
                    Range(-(GameInfo.LocationArea / 2), GameInfo.LocationArea).
                    OrderBy(_ => Guid.NewGuid()).
                    ToArray();
                break;
            // 画面端
            case LocationType.Side:
                if (GameInfo.Instance.Player != null)
                {
                    int point = GameInfo.LocationArea / 2;
                    if (GameInfo.Instance.Player.Transform.position.x >= 0)
                    {
                        point = -point;
                    }
                    positions = Enumerable.Repeat<int>(point, count).ToArray();
                }
                break;
        }

        return positions;
    }

    /// <summary>
    /// 中継地点の作成
    /// </summary>
    /// <returns>中継地点配列</returns>
    int[] CreateMiddlePoint()
    {
        return Enumerable.
            Range(-(GameInfo.LocationArea / 2), GameInfo.LocationArea).
            OrderBy(_ => Guid.NewGuid()).
            ToArray();
    }

    /// <summary>
    /// 中継地点をVector3型に変換
    /// </summary>
    /// <param name="point">横の位置</param>
    /// <returns>Vector3型の位置</returns>
    Vector3 ConvertMiddlePoint(int point)
    {
        return new Vector3(point * 40.0f, (point & 1) * 40.0f, 0.0f);
    }
}
