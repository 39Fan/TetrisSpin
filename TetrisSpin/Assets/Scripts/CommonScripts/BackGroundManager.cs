using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 背景の演出を管理するクラス
/// </summary>
public class BackGroundManager : MonoBehaviour
{
    // スクロール可能な画像 //
    [SerializeField] private RectTransform gameClearBackGroundRect1;
    [SerializeField] private RectTransform gameClearBackGroundRect2;
    [SerializeField] private RectTransform gameOverBackGroundRect1;
    [SerializeField] private Image gameOverBackGroundImage1;
    [SerializeField] private RectTransform scoreBackGroundRect1;
    [SerializeField] private RectTransform scoreBackGroundRect2;

    /// <summary> GameClearのスクロール速度 </summary>
    private readonly int gameClearScrollSpeed = 50;
    /// <summary> GameClearのスクロール速度 </summary>
    private readonly int scoreScrollSpeed = 25;

    /// <summary> 背景の振動間隔 </summary>
    private float nextShakeTime = 0f;

    void Update()
    {
        if (GameStateManager.GameClear == true)
        {
            GameClearScroll();
        }
        if (Time.time >= nextShakeTime && GameStateManager.GameOver)
        {
            StartCoroutine(GameOverShake());
            nextShakeTime = Time.time + Random.Range(3, 7); // 次の揺れまでの間隔を3秒から7秒のランダムで設定
        }
        if (GameStateManager.Score == true)
        {
            ScoreScroll();
        }
        GameClearScroll();
    }

    /// <summary> ゲームクリア画面のスクロールをする関数 </summary>
    private void GameClearScroll()
    {
        float scrollAmount = Time.deltaTime * gameClearScrollSpeed;
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

    /// <summary> スコア画面のスクロールをする関数 </summary>
    private void ScoreScroll()
    {
        float scrollAmount = Time.deltaTime * scoreScrollSpeed;
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

    /// <summary> ゲームオーバー画面の振動をする関数 </summary>
    private IEnumerator GameOverShake()
    {
        Vector2 originalPosition = gameOverBackGroundImage1.rectTransform.anchoredPosition;
        Color originalColor = gameOverBackGroundImage1.color;

        // 透明度を下げる
        gameOverBackGroundImage1.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.02f);

        // 右に振る
        gameOverBackGroundImage1.rectTransform.anchoredPosition = new Vector2(originalPosition.x + 30, originalPosition.y);
        yield return new WaitForSeconds(0.03f); // 短い待機

        // 左に振る
        gameOverBackGroundImage1.rectTransform.anchoredPosition = new Vector2(originalPosition.x - 30, originalPosition.y);
        yield return new WaitForSeconds(0.03f); // 短い待機

        // 元の位置に戻す
        gameOverBackGroundImage1.rectTransform.anchoredPosition = originalPosition;

        // 透明度を戻す
        gameOverBackGroundImage1.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.04f);
    }
}