using System;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField] float m_speed;
    [SerializeField] Background[] m_background;

    void Start()
    {
        // Backgroundの初期設定
        for (int i = 0; i < m_background.Length; i++)
        {
            m_background[i].Initialize(i, new Vector3(0.0f, i * 360.0f, 0.0f), m_speed);
            m_background[i].SetBackgroundSprite(GetStageSprite(m_background[i].Index));
        }
    }

    void Update()
    {
        var size = GameInfo.Instance.ScreenSize;
        var lastIndex = GameInfo.Instance.StageBgFiles.Length;

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
                    bg.SetBackgroundSprite(GetStageSprite(bg.Index += 2));
                }
            }
        }

        GameInfo.Instance.StageMove += Math.Abs(m_speed);
    }

    /// <summary>
    /// 背景画像の取得
    /// </summary>
    /// <param name="no">何番目</param>
    /// <returns>Sprite</returns>
    Sprite GetStageSprite(int no)
    {
        if (no >= GameInfo.Instance.StageBgFiles.Length)
        {
            no = GameInfo.Instance.StageBgFiles.Length - 1;
        }

        return GameInfo.Instance.StageBgFiles[no];
    }
}
