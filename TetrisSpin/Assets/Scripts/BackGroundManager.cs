using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 背景の演出を管理するクラス
/// </summary>
public class BackGroundManager : MonoBehaviour
{
    // スクロール可能な画像 //
    [SerializeField] private RectTransform GameClearBackGroundRect1;
    [SerializeField] private RectTransform GameClearBackGroundRect2;
    [SerializeField] private RectTransform GameOverBackGroundRect1;
    [SerializeField] private Image GameOverBackGroundImage1;

    /// <summary> 背景のスクロール速度 </summary>
    private readonly int scrollSpeed = 50;

    /// <summary> 背景の振動間隔 </summary>
    private float nextShakeTime = 0f;

    void Update()
    {
        if (GameManagerStats.GameClear == true)
        {
            GameClearScroll();
        }
        if (Time.time >= nextShakeTime && GameManagerStats.GameOver)
        {
            StartCoroutine(GameOverShake());
            nextShakeTime = Time.time + Random.Range(3, 7); // 次の揺れまでの間隔を3秒から7秒のランダムで設定
        }
    }

    /// <summary> ゲームクリア画面のスクロールをする関数 </summary>
    private void GameClearScroll()
    {
        float scrollAmount = Time.deltaTime * scrollSpeed;
        Vector2 position1 = GameClearBackGroundRect1.anchoredPosition;
        Vector2 position2 = GameClearBackGroundRect2.anchoredPosition;

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

        GameClearBackGroundRect1.anchoredPosition = position1;
        GameClearBackGroundRect2.anchoredPosition = position2;
    }

    /// <summary> ゲームオーバー画面の振動をする関数 </summary>
    private IEnumerator GameOverShake()
    {
        Vector2 originalPosition = GameOverBackGroundImage1.rectTransform.anchoredPosition;
        Color originalColor = GameOverBackGroundImage1.color;

        // 透明度を下げる
        GameOverBackGroundImage1.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.02f);

        // 右に振る
        GameOverBackGroundImage1.rectTransform.anchoredPosition = new Vector2(originalPosition.x + 30, originalPosition.y);
        yield return new WaitForSeconds(0.03f); // 短い待機

        // 左に振る
        GameOverBackGroundImage1.rectTransform.anchoredPosition = new Vector2(originalPosition.x - 30, originalPosition.y);
        yield return new WaitForSeconds(0.03f); // 短い待機

        // 元の位置に戻す
        GameOverBackGroundImage1.rectTransform.anchoredPosition = originalPosition;

        // 透明度を戻す
        GameOverBackGroundImage1.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.04f);
    }
}