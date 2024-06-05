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

    /// <summary> 指定されたフィールドの値を更新する関数 </summary>
    /// <param name="_useHold"> ホールドの使用判定 </param>
    /// <param name="_firstHold"> ゲーム中で最初のホールドの使用判定 </param>
    /// <remarks>
    /// 指定されていない引数は現在の値を維持
    /// </remarks>
    public static void Update(bool? _useHold = null, bool? _firstHold = null)
    {
        useHold = _useHold ?? useHold;
        firstHold = _firstHold ?? firstHold;
        // TODO: ログの記入
    }

    /// <summary> デフォルトの <see cref="AttackCalculatorStats"/> にリセットする関数 </summary>
    public static void Reset()
    {
        useHold = false;
        firstHold = true;
    }
}

/// <summary>
/// プレイヤーの入力を処理するクラス
/// </summary>
public class PlayerInput : MonoBehaviour
{
    GameManager gameManager;
    Board board;
    GameAutoRunner gameAutoRunner;
    Spawner spawner;
    SpinCheck spinCheck;
    MinoMovement minoMovement;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
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
        if (Input.GetKeyDown(KeyCode.D))
        {
            MoveRightInput();
        }
        else if (Input.GetKey(KeyCode.D) && (Time.time > Timer.NextKeyLeftRightTimer))
        {
            ContinuousMoveRightInput();
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            ReleaseContinuousMoveRightLeftInput();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            MoveLeftInput();
        }
        else if (Input.GetKey(KeyCode.A) && (Time.time > Timer.NextKeyLeftRightTimer))
        {
            ContinuousMoveLeftInput();
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            ReleaseContinuousMoveRightLeftInput();
        }
        else if (Input.GetKey(KeyCode.S) && (Time.time > Timer.NextKeyDownTimer))
        {
            MoveDownInput();
        }
        else if (Input.GetKeyDown(KeyCode.P) && (Time.time > Timer.NextKeyRotateTimer))
        {
            RotateRightInput();
        }
        else if (Input.GetKeyDown(KeyCode.L) && (Time.time > Timer.NextKeyRotateTimer))
        {
            RotateLeftInput();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            HardDropInput();
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            HoldInput();
        }
    }

    /// <summary> 右移動入力時の処理を行う関数 </summary>
    private void MoveRightInput()
    {
        Timer.ContinuousLRKey = false; // キーの連続入力でない判定を付与
        Timer.UpdateLeftRightTimer();
        spawner.ActiveMino.MoveRight();

        if (!board.CheckPosition(spawner.ActiveMino)) // 右に動かせない時
        {
            spawner.ActiveMino.MoveLeft(); // 左に動かす(元に戻すため)
        }
        else // 動かせた時
        {
            AudioManager.Instance.PlaySound(eAudioName.MoveLeftRight);
            spawner.AdjustGhostMinoPosition();
            spinCheck.ResetSpinTypeName();
            minoMovement.ResetStepsSRS();
        }

        IncreaseBottomMoveCount(); // BottomMoveCountの値を1増加
    }

    /// <summary> 連続右移動入力時の処理を行う関数 </summary>
    private void ContinuousMoveRightInput()
    {
        Timer.ContinuousLRKey = true; // キーの連続入力判定を付与
        Timer.UpdateLeftRightTimer();
        spawner.ActiveMino.MoveRight();

        if (!board.CheckPosition(spawner.ActiveMino)) // 右に動かせない時
        {
            spawner.ActiveMino.MoveLeft(); // 左に動かす(元に戻すため)
        }
        else // 動かせた時
        {
            AudioManager.Instance.PlaySound(eAudioName.MoveLeftRight);
            spawner.AdjustGhostMinoPosition();
            spinCheck.ResetSpinTypeName();
            minoMovement.ResetStepsSRS();
        }

        IncreaseBottomMoveCount(); // BottomMoveCountの値を1増加
    }

    /// <summary> 左移動入力時の処理を行う関数 </summary>
    private void MoveLeftInput()
    {
        Timer.ContinuousLRKey = false; // キーの連続入力でない判定を付与
        Timer.UpdateLeftRightTimer();
        spawner.ActiveMino.MoveLeft();

        if (!board.CheckPosition(spawner.ActiveMino)) // 左に動かせない時
        {
            spawner.ActiveMino.MoveRight(); // 右に動かす(元に戻すため)
        }
        else // 動かせた時
        {
            AudioManager.Instance.PlaySound(eAudioName.MoveLeftRight);
            spawner.AdjustGhostMinoPosition();
            spinCheck.ResetSpinTypeName();
            minoMovement.ResetStepsSRS();
        }

        IncreaseBottomMoveCount(); // BottomMoveCountの値を1増加
    }

    /// <summary> 連続左移動入力時の処理を行う関数 </summary>
    private void ContinuousMoveLeftInput()
    {
        Timer.ContinuousLRKey = true; // キーの連続入力判定を付与
        Timer.UpdateLeftRightTimer();
        spawner.ActiveMino.MoveLeft();

        if (!board.CheckPosition(spawner.ActiveMino)) // 左に動かせない時
        {
            spawner.ActiveMino.MoveRight(); // 右に動かす(元に戻すため)
        }
        else // 動かせた時
        {
            AudioManager.Instance.PlaySound(eAudioName.MoveLeftRight);
            spawner.AdjustGhostMinoPosition();
            spinCheck.ResetSpinTypeName();
            minoMovement.ResetStepsSRS();
        }

        IncreaseBottomMoveCount(); // BottomMoveCountの値を1増加
    }

    /// <summary> 連続右、または左移動入力の解除処理を行う関数 </summary>
    private void ReleaseContinuousMoveRightLeftInput()
    {
        Timer.ContinuousLRKey = false;
    }

    /// <summary> 下移動入力時の処理を行う関数 </summary>
    private void MoveDownInput()
    {
        Timer.UpdateDownTimer();
        spawner.ActiveMino.MoveDown();

        if (!board.CheckPosition(spawner.ActiveMino)) // 下に動かせない時
        {
            spawner.ActiveMino.MoveUp(); // 上に動かす(元に戻すため)
        }
        else // 動かせた時
        {
            AudioManager.Instance.PlaySound(eAudioName.MoveDown);

            if (spinCheck.SpinTypeName != SpinTypeNames.Ispin) // I-Spinは下移動しても解除されないようにしている
            {
                spinCheck.ResetSpinTypeName(); // 移動したため、スピン判定をリセット
            }

            minoMovement.ResetStepsSRS();
        }
    }

    /// <summary> 右回転入力時の処理を行う関数 </summary>
    private void RotateRightInput()
    {
        Timer.UpdateRotateTimer();
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
    }

    /// <summary> 左回転入力時の処理を行う関数 </summary>
    private void RotateLeftInput()
    {
        Timer.UpdateRotateTimer();
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
    }

    /// <summary> ハードドロップ入力時の処理を行う関数 </summary>
    private void HardDropInput()
    {
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

            if (spinCheck.SpinTypeName != SpinTypeNames.Ispin) // I-Spinは下移動しても解除されないようにしている
            {
                spinCheck.ResetSpinTypeName();
            }

            minoMovement.ResetStepsSRS();
        }

        gameAutoRunner.ResetRockDown();
        gameAutoRunner.SetMinoFixed();
    }

    /// <summary> ホールド入力時の処理を行う関数 </summary>
    private void HoldInput()
    {
        if (PlayerInputStats.UseHold == false)
        {
            PlayerInputStats.Update(_useHold: true);
            AudioManager.Instance.PlaySound(eAudioName.Hold);
            gameAutoRunner.ResetRockDown();

            if (PlayerInputStats.FirstHold == true) // ゲーム中で最初のHoldだった時
            {
                GameManagerStats.Update(_minoPopNumber: GameManagerStats.MinoPopNumber + 1);
                spawner.CreateHoldMino(PlayerInputStats.FirstHold, GameManagerStats.MinoPopNumber);
                PlayerInputStats.Update(_firstHold: false);
            }
            else
            {
                spawner.CreateHoldMino(PlayerInputStats.FirstHold, GameManagerStats.MinoPopNumber);
            }
        }
    }

    /// <summary> 回転が成功した時の処理をする関数 </summary>
    private void SuccessRotateAction()
    {
        LogHelper.Log(LogHelper.eLogLevel.Debug, "GameManager", "SuccessRotateAction()", "Start");

        /// <summary> ミノの回転後の向き </summary>
        eMinoDirection minoAngleAfter;
        // /// <summary> ミノの回転前の向き </summary>
        // MinoDirections minoAngleBefore;
        /// <summary> スーパーローテーションシステム(SRS)の段階 </summary>
        int stepsSRS;

        spawner.AdjustGhostMinoPosition(); // ゴーストミノの位置調整

        minoMovement.UpdateMinoAngleBeforeToMinoAngleAfter();

        minoAngleAfter = minoMovement.GetMinoAngleAfter();
        // minoAngleBefore = mino.GetMinoAngleBefore();
        stepsSRS = minoMovement.GetStepsSRS();
        spinCheck.CheckSpinType(minoAngleAfter, stepsSRS);

        if (spinCheck.SpinTypeName != SpinTypeNames.None) // スピン判定がない場合
        {
            AudioManager.Instance.PlaySound(eAudioName.Spin);
        }
        else // スピン判定がある場合
        {
            AudioManager.Instance.PlaySound(eAudioName.Rotation);
        }

        LogHelper.Log(LogHelper.eLogLevel.Debug, "GameManager", "SuccessRotateAction()", "End");
    }

    /// <summary> BottomMoveCountを進める関数 </summary>
    private void IncreaseBottomMoveCount()
    {
        LogHelper.Log(LogHelper.eLogLevel.Debug, "GameManager", "IncreaseBottomMoveCount()", "Start");
        GameAutoRunnerStats.Update(_bottomMoveCount: GameAutoRunnerStats.BottomMoveCount + 1);
        LogHelper.Log(LogHelper.eLogLevel.Debug, "GameManager", "IncreaseBottomMoveCount()", "End");
    }
}
