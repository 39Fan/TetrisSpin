using UnityEngine;

/// <summary>
/// Scorerシーンの背景のスクロール演出を管理するクラス
/// </summary>
public class ScoreScene_BackGroundScrollManager : MonoBehaviour
{
    // スクロール可能な画像 //
    [SerializeField] private RectTransform scoreBackGroundRect1;
    [SerializeField] private RectTransform scoreBackGroundRect2;

    /// <summary> スクロール速度 </summary>
    private readonly int ScrollSpeed = 25;

    void Update()
    {
        ScoreScroll();
    }

    /// <summary> スコア画面のスクロールをする関数 </summary>
    private void ScoreScroll()
    {
        // スクロール量
        float scrollAmount = Time.deltaTime * ScrollSpeed;

        // それぞれの画像の位置
        Vector2 position1 = scoreBackGroundRect1.anchoredPosition;
        Vector2 position2 = scoreBackGroundRect2.anchoredPosition;

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

        scoreBackGroundRect1.anchoredPosition = position1;
        scoreBackGroundRect2.anchoredPosition = position2;
    }
}