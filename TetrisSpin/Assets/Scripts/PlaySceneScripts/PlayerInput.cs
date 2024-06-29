using UnityEngine;

/// <summary>
/// PlayerInputの統計情報を保持する静的クラス
/// </summary>
public static class PlayerInputStats
{
    /// <summary> ホールドの使用判定 </summary>
    private static bool useHold = false;
    /// <summary> ゲーム中で最初のホールドの使用判定 </summary>
    private static bool firstHold = true;

    // ゲッタープロパティ //
    public static bool UseHold => useHold;
    public static bool FirstHold => firstHold;

    /// <summary> スタッツログの詳細 </summary>
    private static string logStatsDetail;

    /// <summary> 指定されたフィールドの値を更新する関数 </summary>
    /// <param name="_useHold"> ホールドの使用判定 </param>
    /// <param name="_firstHold"> ゲーム中で最初のホールドの使用判定 </param>
    /// <remarks>
    /// 指定されていない引数は現在の値を維持
    /// </remarks>
    public static void UpdateStats(bool? _useHold = null, bool? _firstHold = null)
    {
        LogHelper.DebugLog(eClasses.PlayerInputStats, eMethod.UpdateStats, eLogTitle.Start);

        useHold = _useHold ?? useHold;
        firstHold = _firstHold ?? firstHold;

        logStatsDetail = $"useHold : {useHold}, firstHold : {firstHold}";
        LogHelper.InfoLog(eClasses.PlayerInputStats, eMethod.UpdateStats, eLogTitle.StatsInfo, logStatsDetail);

        LogHelper.DebugLog(eClasses.PlayerInputStats, eMethod.UpdateStats, eLogTitle.End);
    }

    /// <summary> デフォルトの <see cref="AttackCalculatorStats"/> にリセットする関数 </summary>
    public static void ResetStats()
    {
        LogHelper.DebugLog(eClasses.PlayerInputStats, eMethod.ResetStats, eLogTitle.Start);

        useHold = false;
        firstHold = true;

        LogHelper.DebugLog(eClasses.PlayerInputStats, eMethod.ResetStats, eLogTitle.End);
    }
}

/// <summary>
/// プレイヤーの入力を処理するクラス
/// </summary>
public class PlayerInput : MonoBehaviour
{
    Board board;
    GameAutoRunner gameAutoRunner;
    Spawner spawner;
    SpinCheck spinCheck;
    MinoMovement minoMovement;

    /// <summary>
    /// インスタンス化
    /// </summary>
    private void Awake()
    {
        board = FindObjectOfType<Board>();
        gameAutoRunner = FindObjectOfType<GameAutoRunner>();
        spawner = FindObjectOfType<Spawner>();
        spinCheck = FindObjectOfType<SpinCheck>();
        minoMovement = FindObjectOfType<MinoMovement>();
    }

    /// <summary>
    /// ゲーム中のプレイヤーの入力を処理する関数
    /// </summary>
    public void InputInGame()
    {
        LogHelper.DebugLog(eClasses.PlayerInput, eMethod.InputInGame, eLogTitle.UpdateFunctionRunning);

        if (Input.GetKeyDown(KeyCode.D))
        {
            MoveRightInput();
            ConfirmMinoMovement();
        }
        else if (Input.GetKey(KeyCode.D) && (Time.time > CoolDownTimer.NextKeyLeftRightCoolDownTimer))
        {
            ContinuousMoveRightInput();
            ConfirmMinoMovement();
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            ReleaseContinuousMoveRightLeftInput();
            ConfirmMinoMovement();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            MoveLeftInput();
            ConfirmMinoMovement();
        }
        else if (Input.GetKey(KeyCode.A) && (Time.time > CoolDownTimer.NextKeyLeftRightCoolDownTimer))
        {
            ContinuousMoveLeftInput();
            ConfirmMinoMovement();
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            ReleaseContinuousMoveRightLeftInput();
            ConfirmMinoMovement();
        }
        else if (Input.GetKey(KeyCode.S) && (Time.time > CoolDownTimer.NextKeyDownCoolDownTimer))
        {
            MoveDownInput();
            ConfirmMinoMovement();
        }
        else if (Input.GetKeyDown(KeyCode.P) && (Time.time > CoolDownTimer.NextKeyRotateCoolDownTimer))
        {
            RotateRightInput();
            ConfirmMinoMovement();
        }
        else if (Input.GetKeyDown(KeyCode.L) && (Time.time > CoolDownTimer.NextKeyRotateCoolDownTimer))
        {
            RotateLeftInput();
            ConfirmMinoMovement();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            HardDropInput();
            ConfirmMinoMovement();
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            HoldInput();
            ConfirmMinoMovement();
        }
    }

    /// <summary> 右移動入力時の処理を行う関数 </summary>
    private void MoveRightInput()
    {
        LogHelper.DebugLog(eClasses.PlayerInput, eMethod.MoveRightInput, eLogTitle.Start);

        CoolDownTimer.ContinuousLRKey = false; // キーの連続入力でない判定を付与
        CoolDownTimer.UpdateMoveLeftRightCoolDownTimer();
        spawner.ActiveMino.MoveRight();

        if (!board.CheckPosition(spawner.ActiveMino)) // 右に動かせない時
        {
            spawner.ActiveMino.MoveLeft(); // 左に動かす(元に戻すため)
        }
        else // 動かせた時
        {
            AudioManager.Instance.PlaySound(eAudioName.MoveLeftRight);
            spawner.AdjustGhostMinoPosition();
            spinCheck.ResetSpinType();
            minoMovement.ResetStepsSRS();
        }

        IncreaseBottomMoveCount();

        LogHelper.DebugLog(eClasses.PlayerInput, eMethod.MoveRightInput, eLogTitle.End);
    }

    /// <summary> 連続右移動入力時の処理を行う関数 </summary>
    private void ContinuousMoveRightInput()
    {
        LogHelper.DebugLog(eClasses.PlayerInput, eMethod.ContinuousMoveRightInput, eLogTitle.Start);

        CoolDownTimer.ContinuousLRKey = true; // キーの連続入力判定を付与
        CoolDownTimer.UpdateMoveLeftRightCoolDownTimer();
        spawner.ActiveMino.MoveRight();

        if (!board.CheckPosition(spawner.ActiveMino)) // 右に動かせない時
        {
            spawner.ActiveMino.MoveLeft(); // 左に動かす(元に戻すため)
        }
        else // 動かせた時
        {
            AudioManager.Instance.PlaySound(eAudioName.MoveLeftRight);
            spawner.AdjustGhostMinoPosition();
            spinCheck.ResetSpinType();
            minoMovement.ResetStepsSRS();
        }

        // IncreaseBottomMoveCount();

        LogHelper.DebugLog(eClasses.PlayerInput, eMethod.ContinuousMoveRightInput, eLogTitle.End);
    }

    /// <summary> 左移動入力時の処理を行う関数 </summary>
    private void MoveLeftInput()
    {
        LogHelper.DebugLog(eClasses.PlayerInput, eMethod.MoveLeftInput, eLogTitle.Start);

        CoolDownTimer.ContinuousLRKey = false; // キーの連続入力でない判定を付与
        CoolDownTimer.UpdateMoveLeftRightCoolDownTimer();
        spawner.ActiveMino.MoveLeft();

        if (!board.CheckPosition(spawner.ActiveMino)) // 左に動かせない時
        {
            spawner.ActiveMino.MoveRight(); // 右に動かす(元に戻すため)
        }
        else // 動かせた時
        {
            AudioManager.Instance.PlaySound(eAudioName.MoveLeftRight);
            spawner.AdjustGhostMinoPosition();
            spinCheck.ResetSpinType();
            minoMovement.ResetStepsSRS();
        }

        IncreaseBottomMoveCount();

        LogHelper.DebugLog(eClasses.PlayerInput, eMethod.MoveLeftInput, eLogTitle.End);
    }

    /// <summary> 連続左移動入力時の処理を行う関数 </summary>
    private void ContinuousMoveLeftInput()
    {
        LogHelper.DebugLog(eClasses.PlayerInput, eMethod.ContinuousMoveLeftInput, eLogTitle.Start);

        CoolDownTimer.ContinuousLRKey = true; // キーの連続入力判定を付与
        CoolDownTimer.UpdateMoveLeftRightCoolDownTimer();
        spawner.ActiveMino.MoveLeft();

        if (!board.CheckPosition(spawner.ActiveMino)) // 左に動かせない時
        {
            spawner.ActiveMino.MoveRight(); // 右に動かす(元に戻すため)
        }
        else // 動かせた時
        {
            AudioManager.Instance.PlaySound(eAudioName.MoveLeftRight);
            spawner.AdjustGhostMinoPosition();
            spinCheck.ResetSpinType();
            minoMovement.ResetStepsSRS();
        }

        // IncreaseBottomMoveCount();

        LogHelper.DebugLog(eClasses.PlayerInput, eMethod.ContinuousMoveLeftInput, eLogTitle.End);
    }

    /// <summary> 連続右、または左移動入力の解除処理を行う関数 </summary>
    private void ReleaseContinuousMoveRightLeftInput()
    {
        LogHelper.DebugLog(eClasses.PlayerInput, eMethod.ReleaseContinuousMoveRightLeftInput, eLogTitle.Start);

        CoolDownTimer.ContinuousLRKey = false;

        LogHelper.DebugLog(eClasses.PlayerInput, eMethod.ReleaseContinuousMoveRightLeftInput, eLogTitle.End);
    }

    /// <summary> 下移動入力時の処理を行う関数 </summary>
    private void MoveDownInput()
    {
        LogHelper.DebugLog(eClasses.PlayerInput, eMethod.MoveDownInput, eLogTitle.Start);

        CoolDownTimer.UpdateMoveDownCoolDownTimer();
        spawner.ActiveMino.MoveDown();

        if (!board.CheckPosition(spawner.ActiveMino)) // 下に動かせない時
        {
            spawner.ActiveMino.MoveUp(); // 上に動かす(元に戻すため)
        }
        else // 動かせた時
        {
            AudioManager.Instance.PlaySound(eAudioName.MoveDown);

            if (spinCheck.SpinType != SpinTypes.Ispin) // I-Spinは下移動しても解除されないようにしている
            {
                spinCheck.ResetSpinType(); // 移動したため、スピン判定をリセット
            }

            minoMovement.ResetStepsSRS();
        }

        LogHelper.DebugLog(eClasses.PlayerInput, eMethod.MoveDownInput, eLogTitle.End);
    }

    /// <summary> 右回転入力時の処理を行う関数 </summary>
    private void RotateRightInput()
    {
        LogHelper.DebugLog(eClasses.PlayerInput, eMethod.RotateRightInput, eLogTitle.Start);

        CoolDownTimer.UpdateRotateCoolDownTimer();
        minoMovement.ResetStepsSRS();
        spawner.ActiveMino.RotateRight();

        if (!board.CheckPosition(spawner.ActiveMino)) // 通常回転ができなかった時
        {
            if (!minoMovement.SuperRotationSystem()) // SRSもできなかった時
            {
                minoMovement.UpdateMinoAngleAfterToMinoAngleBefore();
                AudioManager.Instance.PlaySound(eAudioName.Rotation);
            }
            else // SRSが成功した時
            {
                SuccessRotateAction();
            }
        }
        else // 通常回転が成功した時
        {
            SuccessRotateAction();
        }

        IncreaseBottomMoveCount();

        LogHelper.DebugLog(eClasses.PlayerInput, eMethod.RotateRightInput, eLogTitle.End);
    }

    /// <summary> 左回転入力時の処理を行う関数 </summary>
    private void RotateLeftInput()
    {
        LogHelper.DebugLog(eClasses.PlayerInput, eMethod.RotateLeftInput, eLogTitle.Start);

        CoolDownTimer.UpdateRotateCoolDownTimer();
        minoMovement.ResetStepsSRS();
        spawner.ActiveMino.RotateLeft();

        if (!board.CheckPosition(spawner.ActiveMino)) // 通常回転ができなかった時
        {
            if (!minoMovement.SuperRotationSystem()) // SRSもできなかった時
            {
                minoMovement.UpdateMinoAngleAfterToMinoAngleBefore();
                AudioManager.Instance.PlaySound(eAudioName.Rotation);
            }
            else // SRSが成功した時
            {
                SuccessRotateAction();
            }
        }
        else // 通常回転が成功した時
        {
            SuccessRotateAction();
        }

        IncreaseBottomMoveCount();

        LogHelper.DebugLog(eClasses.PlayerInput, eMethod.RotateLeftInput, eLogTitle.End);
    }

    /// <summary> ハードドロップ入力時の処理を行う関数 </summary>
    private void HardDropInput()
    {
        LogHelper.DebugLog(eClasses.PlayerInput, eMethod.HardDropInput, eLogTitle.Start);

        AudioManager.Instance.PlaySound(eAudioName.HardDrop);

        // Heightの値分繰り返す(20)
        for (int i = 0; i < board.Height; i++)
        {
            spawner.ActiveMino.MoveDown();

            if (!board.CheckPosition(spawner.ActiveMino)) // 底にぶつかった時
            {
                spawner.ActiveMino.MoveUp(); // ミノを正常な位置に戻す
                break;
            }

            if (spinCheck.SpinType != SpinTypes.Ispin) // I-Spinは下移動しても解除されないようにしている
            {
                spinCheck.ResetSpinType();
            }

            minoMovement.ResetStepsSRS();
        }

        gameAutoRunner.ResetRockDown();
        gameAutoRunner.SetMinoFixed();

        LogHelper.DebugLog(eClasses.PlayerInput, eMethod.HardDropInput, eLogTitle.End);
    }

    /// <summary> ホールド入力時の処理を行う関数 </summary>
    private void HoldInput()
    {
        LogHelper.DebugLog(eClasses.PlayerInput, eMethod.HoldInput, eLogTitle.Start);

        if (PlayerInputStats.UseHold == false)
        {
            PlayerInputStats.UpdateStats(_useHold: true);
            AudioManager.Instance.PlaySound(eAudioName.Hold);
            gameAutoRunner.ResetRockDown();

            if (PlayerInputStats.FirstHold == true) // ゲーム中で最初のHoldだった時
            {
                GameManagerStats.UpdateStats(_minoPopNumber: GameManagerStats.MinoPopNumber + 1);
                spawner.CreateHoldMino(PlayerInputStats.FirstHold, GameManagerStats.MinoPopNumber);
                PlayerInputStats.UpdateStats(_firstHold: false);
            }
            else
            {
                spawner.CreateHoldMino(PlayerInputStats.FirstHold, GameManagerStats.MinoPopNumber);
            }
        }

        LogHelper.DebugLog(eClasses.PlayerInput, eMethod.HoldInput, eLogTitle.End);
    }

    /// <summary> 回転が成功した時の処理をする関数 </summary>
    private void SuccessRotateAction()
    {
        LogHelper.DebugLog(eClasses.PlayerInput, eMethod.SuccessRotateAction, eLogTitle.Start);

        spawner.AdjustGhostMinoPosition();
        minoMovement.UpdateMinoAngleBeforeToMinoAngleAfter();
        spinCheck.CheckSpinType(MinoMovementStats.MinoAngleAfter, MinoMovementStats.StepsSRS);

        if (spinCheck.SpinType != SpinTypes.None) // スピン判定がない場合
        {
            AudioManager.Instance.PlaySound(eAudioName.Spin);
        }
        else // スピン判定がある場合
        {
            AudioManager.Instance.PlaySound(eAudioName.Rotation);
        }

        LogHelper.DebugLog(eClasses.PlayerInput, eMethod.SuccessRotateAction, eLogTitle.End);
    }

    /// <summary> BottomMoveCountを進める関数 </summary>
    private void IncreaseBottomMoveCount()
    {
        LogHelper.DebugLog(eClasses.PlayerInput, eMethod.IncreaseBottomMoveCount, eLogTitle.Start);

        GameAutoRunnerStats.UpdateStats(_bottomMoveCount: GameAutoRunnerStats.BottomMoveCount + 1);

        LogHelper.DebugLog(eClasses.PlayerInput, eMethod.IncreaseBottomMoveCount, eLogTitle.End);
    }

    /// <summary> 移動後に想定外の挙動をしていないか最終確認を行う関数 </summary>
    private void ConfirmMinoMovement()
    {
        LogHelper.DebugLog(eClasses.PlayerInput, eMethod.ConfirmMinoMovement, eLogTitle.Start);

        if (!board.CheckPosition(spawner.ActiveMino))
        {
            LogHelper.ErrorLog(eClasses.PlayerInput, eMethod.ConfirmMinoMovement, eLogTitle.InvalidMinosPositionDetected);
        }

        LogHelper.DebugLog(eClasses.PlayerInput, eMethod.ConfirmMinoMovement, eLogTitle.End);
    }
}
