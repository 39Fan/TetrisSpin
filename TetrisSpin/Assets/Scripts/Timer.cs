using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// タイマー
public class Timer : MonoBehaviour
{
    // 自動でミノが落ちるまでのインターバル時間
    // 次にブロックが落ちるまでの時間
    // 入力受付タイマー(4種類)
    [SerializeField] private float AutoDropInteaval { get; set; } = 1f;
    public float NextKeyLeftRightTimer { get; private set; }
    public float NextKeyRotateTimer { get; private set; }
    public float NextKeyDownTimer { get; private set; }
    public float AutoDropTimer { get; private set; }

    // ミノが底に面していられる時間
    public float keyReceptionTimer { get; set; } // 修正予定

    // 入力インターバル(4種類)
    public float NextKeyLeftRightInterval_Normal { get; private set; } = 0.25f;
    public float NextKeyLeftRightInterval_Short { get; private set; } = 0.05f;
    public float NextKeyRotateInterval { get; private set; } = 0.05f;
    public float NextKeyDownInterval { get; private set; } = 0.05f;


    public float keyReceptionInterval { get; set; } = 1f; //　修正予定

    // 連続右入力、または連続左入力の判定
    public bool ContinuousLRKey { get; set; } = false;

    // // タイマーの設定
    // public void SetTimer()
    // {
    //     NextKeyDownTimer = Time.time + NextKeyDownInterval;
    //     NextKeyLeftRightTimer = Time.time + NextKeyLeftRightInterval;
    //     NextKeyRotateTimer = Time.time + NextKeyRotateInterval;
    //     keyReceptionTimer = Time.time + keyReceptionInterval;
    // }

    // ゲームスタート時と、ミノが設置された時のタイマー初期化
    public void ResetTimer()
    {
        NextKeyDownTimer = Time.time + AutoDropTimer;
        NextKeyLeftRightTimer = Time.time;
        NextKeyRotateTimer = Time.time;
        keyReceptionTimer = Time.time;

        keyReceptionInterval = 1f;
    }

    // public void ResetNextKeyInterval()
    // {
    //     NextKeyDownInterval =
    // }

    // 右入力、または左入力のタイマー更新
    public void UpdateLeftRightTimer()
    {
        // 連続入力なら
        if (ContinuousLRKey == true)
        {
            NextKeyLeftRightTimer = Time.time + NextKeyLeftRightInterval_Short;
        }
        else
        {
            NextKeyLeftRightTimer = Time.time + NextKeyLeftRightInterval_Normal;
        }
    }

    // 回転入力のタイマー更新
    public void UpdateRotateTimer()
    {
        NextKeyRotateTimer = Time.time + NextKeyRotateInterval;
    }

    // 下入力、自動落下のタイマー更新
    public void UpdateDownTimer()
    {
        NextKeyDownTimer = Time.time + NextKeyDownInterval;
        AutoDropTimer = Time.time + AutoDropInteaval;
    }
}
