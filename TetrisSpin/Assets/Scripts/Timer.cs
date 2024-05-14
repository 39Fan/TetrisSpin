using UnityEngine;

///// タイマーに関するスクリプト /////


// ↓このスクリプトで可能なこと↓ //

// キー入力の受付時間の管理

public class Timer : MonoBehaviour
{
    // 入力受付タイマー(3種類)
    public float NextKeyLeftRightTimer { get; private set; }
    public float NextKeyRotateTimer { get; private set; }
    public float NextKeyDownTimer { get; private set; }

    // 入力インターバル(4種類)
    public float NextKeyLeftRightInterval_Normal { get; private set; } = 0.20f;
    public float NextKeyLeftRightInterval_Short { get; private set; } = 0.05f; // 連続左右入力用
    public float NextKeyRotateInterval { get; private set; } = 0.05f;
    public float NextKeyDownInterval { get; private set; } = 0.05f;

    // 自動でミノが落ちるまでの時間とそのインターバル
    public float AutoDropTimer { get; private set; }
    public float AutoDropInteaval { get; private set; } = 1f;

    // 連続右入力、または連続左入力の判定
    public bool ContinuousLRKey { get; set; } = false;

    // ロックダウン //
    public float BottomTimer { get; private set; }
    public float BottomTimerInterval { get; private set; } = 1f;


    // ゲームスタート時と、ミノが設置された時のタイマーを初期化する関数 //
    public void ResetTimer()
    {
        NextKeyDownTimer = Time.time;
        NextKeyLeftRightTimer = Time.time;
        NextKeyRotateTimer = Time.time;
        AutoDropTimer = Time.time + AutoDropInteaval;
        BottomTimer = Time.time + BottomTimerInterval;
    }

    // 右入力、または左入力のタイマーを更新する関数 //
    public void UpdateLeftRightTimer()
    {
        if (ContinuousLRKey == true) // 連続入力なら
        {
            NextKeyLeftRightTimer = Time.time + NextKeyLeftRightInterval_Short;
        }
        else
        {
            NextKeyLeftRightTimer = Time.time + NextKeyLeftRightInterval_Normal;
        }

        BottomTimer = Time.time + BottomTimerInterval;
    }

    // 回転入力のタイマーを更新する関数 //
    public void UpdateRotateTimer()
    {
        NextKeyRotateTimer = Time.time + NextKeyRotateInterval;

        BottomTimer = Time.time + BottomTimerInterval;
    }

    // 下入力、自動落下のタイマーの更新をする関数 //
    public void UpdateDownTimer()
    {
        NextKeyDownTimer = Time.time + NextKeyDownInterval;

        AutoDropTimer = Time.time + AutoDropInteaval;

        BottomTimer = Time.time + BottomTimerInterval;
    }
}