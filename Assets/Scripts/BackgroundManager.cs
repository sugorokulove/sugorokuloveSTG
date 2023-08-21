using System;
using UnityEngine;

[Serializable]
public class StageSpriteFile
{
    [SerializeField] Sprite[] m_files;
    public Sprite[] Files => m_files;
}

public class BackgroundManager : MonoBehaviour
{
    [SerializeField] float m_speed;
    [SerializeField] Background[] m_background;
    [SerializeField] StageSpriteFile[] m_stages;

    void Start()
    {
        // Backgroundの初期設定
        for (int i = 0; i < m_background.Length; i++)
        {
            m_background[i].Initialize(i, new Vector3(0.0f, i * 360.0f, 0.0f), m_speed);
            m_background[i].SetBackgroundSprite(GetStageSprite(GameInfo.Instance.StageNo, m_background[i].Index));
        }
    }

    void Update()
    {
        var size = GameInfo.Instance.ScreenSize;
        var lastIndex = m_stages[GameInfo.Instance.StageNo].Files.Length;

        foreach (var bg in m_background)
        {
            if (bg.Position.y <= 0.0f)
            {
                // 最後
                if (bg.Index == lastIndex - 1)
                {
                    bg.Speed = 0.0f;
                    bg.Position = new Vector3(0.0f, 0.0f, 0.0f);
                    continue;
                }
            }

            if (bg.Position.y <= -size.y)
            {
                // 最後から2枚目
                if (bg.Index == lastIndex - 2)
                {
                    bg.Speed = 0.0f;
                    bg.Position = new Vector3(0.0f, -size.y, 0.0f);
                }
                else
                {
                    // 「位置の調整」と「画像の入れ替え」
                    bg.Position = new Vector3(0.0f, size.y + (bg.Position.y + size.y), 0.0f);
                    bg.SetBackgroundSprite(GetStageSprite(GameInfo.Instance.StageNo, bg.Index += 2));
                }
            }
        }
    }

    /// <summary>
    /// 背景画像の取得
    /// </summary>
    /// <param name="stage">ステージ数</param>
    /// <param name="no">何番目</param>
    /// <returns>Sprite</returns>
    Sprite GetStageSprite(int stage, int no)
    {
        if (stage >= m_stages.Length)
        {
            stage = m_stages.Length - 1;
        }

        if (no >= m_stages[stage].Files.Length)
        {
            no = m_stages[stage].Files.Length - 1;
        }

        return m_stages[stage].Files[no];
    }
}
