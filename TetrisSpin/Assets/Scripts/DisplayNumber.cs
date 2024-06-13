using UnityEngine;
using TMPro;
using DG.Tweening;

/// <summary>
/// ゲーム画面の数値を表示するクラス
/// </summary>
public class DisplayNumber : MonoBehaviour
{
    /// <summary> タイマーを表示するTextMeshProUGUIオブジェクト </summary>
    [SerializeField] private TextMeshProUGUI timerText;

    /// <summary> 合計攻撃ライン数を表示するTextMeshProUGUIオブジェクト </summary>
    [SerializeField] private TextMeshProUGUI sumAttackLinesText;

    /// <summary> RENを表示するTextMeshProUGUIオブジェクト </summary>
    [SerializeField] private TextMeshProUGUI renText;

    /// <summary> タイマーの開始時刻 </summary>
    private float startTime;

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

    /// <summary> 合計攻撃ライン数を実際に表示する関数 </summary>
    public void DisplaySumAttackLines()
    {
        /// 合計攻撃ライン数を取得
        int sumAttackLines = AttackCalculatorStats.SumAttackLines;

        // テキストの色を設定
        if (sumAttackLines >= 0 && sumAttackLines < 20)
        {
            sumAttackLinesText.color = Color.Lerp(Color.white, Color.green, (float)sumAttackLines / 20); // 0-19: 白から緑
        }
        else if (sumAttackLines >= 20 && sumAttackLines < 40)
        {
            sumAttackLinesText.color = Color.Lerp(Color.green, Color.yellow, (float)(sumAttackLines - 20) / 20); // 20-39: 緑から黄色
        }
        else if (sumAttackLines >= 40 && sumAttackLines < 60)
        {
            sumAttackLinesText.color = Color.Lerp(Color.yellow, new Color(1.0f, 0.65f, 0.0f), (float)(sumAttackLines - 40) / 20); // 40-59: 黄色からオレンジ
        }
        else if (sumAttackLines >= 60 && sumAttackLines < 80)
        {
            sumAttackLinesText.color = Color.Lerp(new Color(1.0f, 0.65f, 0.0f), Color.red, (float)(sumAttackLines - 60) / 20); // 60-79: オレンジから赤
        }
        else if (sumAttackLines >= 80 && sumAttackLines <= 99)
        {
            sumAttackLinesText.color = Color.Lerp(Color.red, new Color(0.5f, 0.0f, 0.0f), (float)(sumAttackLines - 80) / 20); // 80-100: 赤から濃い赤
        }
        else if (sumAttackLines == 100)
        {
            sumAttackLinesText.color = Color.red;
        }
        else
        {
            // LogHelper.ErrorLog(eClasses.DisplayNumber, eMethod.DisplaysumAttackLines, eLogTitle.OutOfRangeValue);
        }

        sumAttackLinesText.text = $"{sumAttackLines}";
    }

    /// <summary> RENを実際に表示する関数 </summary>
    public void DisplayRen()
    {
        // RENの値を取得
        int ren = AttackCalculatorStats.Ren;

        // テキストの色とサイズを設定
        string renTextString = "";
        if (ren >= 2 && ren < 5)
        {
            renText.color = Color.Lerp(Color.white, Color.green, (float)(ren - 2) / 3); // 2-4: 白から緑
            renTextString = $"<size={Mathf.Lerp(30, 35, (float)(ren - 2) / 3)}>{ren}</size><size=30><color=white>REN!</color></size>";
        }
        else if (ren >= 5 && ren < 10)
        {
            renText.color = Color.Lerp(Color.green, Color.yellow, (float)(ren - 5) / 5); // 5-9: 緑から黄色
            renTextString = $"<size={Mathf.Lerp(35, 40, (float)(ren - 5) / 5)}>{ren}</size><size=30><color=white>REN!</color></size>";
        }
        else if (ren >= 10 && ren < 15)
        {
            renText.color = Color.Lerp(Color.yellow, Color.red, (float)(ren - 10) / 5); // 10-14: 黄色から赤
            renTextString = $"<size={Mathf.Lerp(40, 45, (float)(ren - 10) / 5)}>{ren}</size><size=30><color=white>REN!</color></size>";
        }
        else if (ren >= 15 && ren <= 21)
        {
            renText.color = Color.red;
            renTextString = $"<size=45>{ren}</size><size=30><color=white>REN!</color></size>";
        }
        else
        {
            // LogHelper.ErrorLog(eClasses.DisplayNumber, eMethod.DisplaysumAttackLines, eLogTitle.OutOfRangeValue);
        }

        // RENの値をテキストに設定
        renText.text = renTextString;
    }

    /// <summary> RENを非表示にする関数 </summary>
    public void ResetDisplayRen()
    {
        renText.DOFade(0, 1f).OnComplete(() => renText.text = "");
    }
}
