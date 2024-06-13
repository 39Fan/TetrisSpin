using TMPro;
using UnityEngine;

/// <summary>
/// ゲーム画面の数値を表示するクラス
/// </summary>
public class DisplayNumber : MonoBehaviour
{
    /// <summary> タイマーを表示するTextMeshProUGUIオブジェクト </summary>
    [SerializeField] private TextMeshProUGUI timerText;

    /// <summary> タイマーの開始時刻 </summary>
    private float startTime;

    /// <summary> 列消去数を表示するTextMeshProUGUIオブジェクト </summary>
    [SerializeField] private TextMeshProUGUI attackLinesText;

    private void Start()
    {
        startTime = Time.time;
    }

    /// <summary>
    /// フレームごとにタイマーを表示させる
    /// </summary>
    private void Update()
    {
        if (GameManagerStats.GameOver == false)
        {
            DisplayTimer();
        }
    }

    /// <summary> タイマーの時間を計算し、実際に表示する関数 </summary>
    private void DisplayTimer()
    {
        float t = Time.time - startTime;

        string minutes = ((int)t / 60).ToString();
        string seconds = (t % 60).ToString("f2");

        timerText.text = minutes + ":" + seconds;
    }

    /// <summary> 列消去数を実際に表示する関数 </summary>
    public void DisplayAttackLines()
    {
        /// 攻撃ライン数を取得
        int attackLines = AttackCalculatorStats.AttackLines;

        // テキストの色を設定
        if (attackLines >= 0 && attackLines < 20)
        {
            attackLinesText.color = Color.Lerp(Color.white, Color.green, (float)attackLines / 20); // 0-19: 白から緑
        }
        else if (attackLines >= 20 && attackLines < 40)
        {
            attackLinesText.color = Color.Lerp(Color.green, Color.yellow, (float)(attackLines - 20) / 20); // 20-39: 緑から黄色
        }
        else if (attackLines >= 40 && attackLines < 60)
        {
            attackLinesText.color = Color.Lerp(Color.yellow, new Color(1.0f, 0.65f, 0.0f), (float)(attackLines - 40) / 20); // 40-59: 黄色からオレンジ
        }
        else if (attackLines >= 60 && attackLines < 80)
        {
            attackLinesText.color = Color.Lerp(new Color(1.0f, 0.65f, 0.0f), Color.red, (float)(attackLines - 60) / 20); // 60-79: オレンジから赤
        }
        else if (attackLines >= 80 && attackLines <= 99)
        {
            attackLinesText.color = Color.Lerp(Color.red, new Color(0.5f, 0.0f, 0.0f), (float)(attackLines - 80) / 20); // 80-100: 赤から濃い赤
        }
        else if (attackLines == 100)
        {
            attackLinesText.color = Color.red;
        }
        else
        {
            // LogHelper.ErrorLog(eClasses.DisplayNumber, eMethod.DisplayAttackLines, eLogTitle.OutOfRangeValue);
        }

        attackLinesText.text = $"{AttackCalculatorStats.AttackLines}";
    }
}
