using UnityEngine;

/// <summary>
/// GameClearシーンの背景のスクロール演出を管理するクラス
/// </summary>
public class GameClearScene_BackGroundScrollManager : MonoBehaviour
{
    // スクロール可能な画像 //
    [SerializeField] private RectTransform gameClearBackGroundRect1;
    [SerializeField] private RectTransform gameClearBackGroundRect2;

    /// <summary> スクロール速度 </summary>
    private readonly int ScrollSpeed = 50;

    void Update()
    {
        GameClearScroll();
    }

    /// <summary> ゲームクリア画面のスクロールをする関数 </summary>
    private void GameClearScroll()
    {
        float scrollAmount = Time.deltaTime * ScrollSpeed;
        Vector2 position1 = gameClearBackGroundRect1.anchoredPosition;
        Vector2 position2 = gameClearBackGroundRect2.anchoredPosition;

        position1.y -= scrollAmount;
        position2.y -= scrollAmount;

        if (position1.y <= -1800)
        {
            position1.y = 1800;
        }
        if (position2.y <= -1800)
        {
            position2.y = 1800;
        }

        gameClearBackGroundRect1.anchoredPosition = position1;
        gameClearBackGroundRect2.anchoredPosition = position2;
    }
}