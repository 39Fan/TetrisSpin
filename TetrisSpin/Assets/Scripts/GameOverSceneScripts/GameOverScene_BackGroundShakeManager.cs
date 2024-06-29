using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// GameOverシーンの背景の振動演出を管理するクラス
/// </summary>
public class GameOverScene_BackGroundShakeManager : MonoBehaviour
{
    // スクロール可能な画像 //
    [SerializeField] private RectTransform gameOverBackGroundRect1;
    [SerializeField] private Image gameOverBackGroundImage1;

    /// <summary> 背景の振動間隔 </summary>
    private float nextShakeTime = 0f;

    void Update()
    {
        if (Time.time >= nextShakeTime)
        {
            StartCoroutine(GameOverShake());
            nextShakeTime = Time.time + Random.Range(3, 7); // 次の揺れまでの間隔を3秒から7秒のランダムで設定
        }
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