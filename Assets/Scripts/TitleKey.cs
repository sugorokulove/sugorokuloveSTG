using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleKey : MonoBehaviour
{
    [SerializeField] SpriteFlash m_flash;

    void Start()
    {
        // 「PRESS ANY KEY」の点滅開始
        m_flash.FlashLoop();
    }

#if true
    private void Update()
    {
        // 何かキーが押されたらゲーム開始
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene("StgScene");
        }
    }
#else
    public void TransitionToGame()
    {
        SceneManager.LoadScene("StgScene");
    }
#endif
}
