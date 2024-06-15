using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

/// <summary>
/// DisplayNumberのテキストの表示に関する時間情報を保持する静的クラス
/// </summary>
internal static class DisplayNumberSettings
{
    // AttackLines の設定値 //
    public static float ATTACK_LINES_FADE_IN_INTERVAL { get; set; } = 0.3f;
    public static float ATTACK_LINES_FADE_OUT_INTERVAL { get; set; } = 1f;
    public static float ATTACK_LINES_WAIT_INTERVAL { get; set; } = 1.5f;

    // // Go テキストの設定値 //
    // public static float GO_FADE_IN_INTERVAL { get; set; } = 0.3f;
    // public static float GO_FADE_OUT_INTERVAL { get; set; } = 0.5f;
    // public static float GO_WAIT_INTERVAL { get; set; } = 1f;

    // // Spin, LineClear テキストの設定値 //
    // public static float SPIN_AND_LINE_CLEAR_FADE_IN_INTERVAL { get; set; } = 0.3f;
    // public static float SPIN_AND_LINE_CLEAR_FADE_OUT_INTERVAL { get; set; } = 0.5f;
    // public static float SPIN_AND_LINE_CLEAR_WAIT_INTERVAL_1 { get; set; } = 2f;
    // public static float SPIN_AND_LINE_CLEAR_WAIT_INTERVAL_2 { get; set; } = 2f;
    // public static float SPIN_AND_LINE_CLEAR_MOVE_INTERVAL_X { get; set; } = 2f;
    // public static float SPIN_AND_LINE_CLEAR_MOVE_INTERVAL_Y { get; set; } = 1.2f;
    // public static float SPIN_AND_LINE_CLEAR_MOVE_DISTANCE { get; set; } = 600f;

    // // BackToBack テキストの設定値 //
    // public static float BACK_TO_BACK_FADE_IN_INTERVAL { get; set; } = 0.3f;
    // public static float BACK_TO_BACK_FADE_OUT_INTERVAL { get; set; } = 1f;
    // public static float BACK_TO_BACK_WAIT_INTERVAL { get; set; } = 2f;

    // // PerfectClear テキストの設定値 //
    // public static float PERFECT_CLEAR_FADE_IN_INTERVAL { get; set; } = 0.3f;
    // public static float PERFECT_CLEAR_FADE_OUT_INTERVAL { get; set; } = 1f;
    // public static float PERFECT_CLEAR_WAIT_INTERVAL { get; set; } = 2f;

    // 透明度 //
    public static int ALPHA0 { get; set; } = 0; // 0の時は透明
    public static int ALPHA1 { get; set; } = 1; // 1の時は不透明
}

/// <summary>
/// ゲーム画面の数値を表示するクラス
/// </summary>
public class DisplayNumber : MonoBehaviour
{
    /// <summary> タイマーを表示するTextMeshProUGUIオブジェクト </summary>
    [SerializeField] private TextMeshProUGUI timerText;

    /// <summary> 攻撃ライン数を表示するTextMeshProUGUIオブジェクト </summary>
    [SerializeField] private TextMeshProUGUI attackLinesText;

    /// <summary> 攻撃ライン数テキストのTween </summary>
    private Tween attackLinesTextTween;

    /// <summary> 合計攻撃ライン数を表示するTextMeshProUGUIオブジェクト </summary>
    [SerializeField] private TextMeshProUGUI sumAttackLinesText;

    /// <summary> RENを表示するTextMeshProUGUIオブジェクト </summary>
    [SerializeField] private TextMeshProUGUI renText;

    /// <summary> ゲームフレームを表示するオブジェクト </summary>
    [SerializeField] private Image gameFrameImage;

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
    /// <remarks>
    /// 攻撃ライン数によって色の変化も行う。 <br/>
    /// ゲームフレームの色も同時に変化させる。
    /// </remarks>
    public void DisplaySumAttackLines()
    {
        /// 合計攻撃ライン数を取得
        int sumAttackLines = AttackCalculatorStats.SumAttackLines;

        // テキストの色を設定
        Color targetColor = Color.white;

        // テキストの色を設定
        if (sumAttackLines >= 0 && sumAttackLines < 20)
        {
            targetColor = Color.Lerp(Color.white, Color.green, (float)sumAttackLines / 20); // 0-19: 白から緑
        }
        else if (sumAttackLines >= 20 && sumAttackLines < 40)
        {
            targetColor = Color.Lerp(Color.green, Color.yellow, (float)(sumAttackLines - 20) / 20); // 20-39: 緑から黄色
        }
        else if (sumAttackLines >= 40 && sumAttackLines < 60)
        {
            targetColor = Color.Lerp(Color.yellow, new Color(1.0f, 0.65f, 0.0f), (float)(sumAttackLines - 40) / 20); // 40-59: 黄色からオレンジ
        }
        else if (sumAttackLines >= 60 && sumAttackLines < 80)
        {
            targetColor = Color.Lerp(new Color(1.0f, 0.65f, 0.0f), Color.red, (float)(sumAttackLines - 60) / 20); // 60-79: オレンジから赤
        }
        else if (sumAttackLines >= 80 && sumAttackLines <= 99)
        {
            targetColor = Color.Lerp(Color.red, new Color(0.5f, 0.0f, 0.0f), (float)(sumAttackLines - 80) / 20); // 80-100: 赤から濃い赤
        }
        else if (sumAttackLines == 100)
        {
            targetColor = Color.red;
        }
        else
        {
            // LogHelper.ErrorLog(eClasses.DisplayNumber, eMethod.DisplaysumAttackLines, eLogTitle.OutOfRangeValue);
        }

        sumAttackLinesText.color = targetColor;
        gameFrameImage.color = targetColor;

        sumAttackLinesText.text = $"{sumAttackLines}";
    }

    /// <summary> 攻撃ライン数を実際に表示する関数 </summary>
    /// <param name="_attackLines"> 今回の攻撃値 </param>
    public void DisplayAttackLines(int _attackLines)
    {
        // 既存のアニメーションを停止
        if (attackLinesTextTween != null && attackLinesTextTween.IsActive())
        {
            attackLinesTextTween.Kill();
        }

        string attackLinesTextString = "";

        // テキストの色とサイズを設定
        if (_attackLines >= 0 && _attackLines < 10)
        {
            attackLinesText.color = Color.Lerp(Color.white, Color.red, (float)_attackLines / 10); // 0-9: 白から赤
            float fontSize = Mathf.Lerp(30, 40, (float)_attackLines / 10); // 0-9: フォントサイズ 30から40
            attackLinesTextString = $"+<size={fontSize}>{_attackLines}</size>";
        }
        else if (_attackLines >= 10)
        {
            attackLinesText.color = Color.red; // 10以上: 赤
            attackLinesTextString = $"+<size=50>{_attackLines}</size>"; // 10以上: フォントサイズ 50
        }

        // 攻撃ライン数をテキストに設定
        attackLinesText.text = attackLinesTextString;

        attackLinesText.alpha = 0;

        // // フェードイン
        // attackLinesTextTween = attackLinesText.DOFade(1, 2f).OnComplete(() =>
        // {
        //     // 1秒後にフェードアウト
        //     attackLinesTextTween = attackLinesText.DOFade(0, 1f).SetDelay(1f).OnComplete(() => attackLinesText.text = "");
        // });

        // アニメーションシーケンスの作成
        var sequence = DOTween.Sequence();
        sequence
            .Append(attackLinesText.DOFade(DisplayNumberSettings.ALPHA1, DisplayNumberSettings.ATTACK_LINES_FADE_IN_INTERVAL))
            .AppendInterval(DisplayNumberSettings.ATTACK_LINES_WAIT_INTERVAL)
            .Append(attackLinesText.DOFade(DisplayNumberSettings.ALPHA0, DisplayNumberSettings.ATTACK_LINES_FADE_OUT_INTERVAL))
            .OnComplete(() => attackLinesText.text = "");

        sequence.Play();

        LogHelper.DebugLog(eClasses.TextEffect, eMethod.SpinAndLineClearTextFadeInAndOut, eLogTitle.End);
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
