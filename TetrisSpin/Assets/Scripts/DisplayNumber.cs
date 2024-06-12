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
        attackLinesText.text = $"{AttackCalculatorStats.AttackLines}";
    }
}
