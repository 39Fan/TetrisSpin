using UnityEngine;
using UnityEngine.Analytics;

///// ゲームマネージャー /////


// ↓このスクリプトで可能なこと↓ //

// ゲームの進行


/// <summary>
/// ゲームマネージャー
/// </summary>
public class GameManager : MonoBehaviour
{
    // ミノの生成数、または設置数 //
    private int MinoPopNumber = 0;
    private int MinoPutNumber = 0; // Holdを使用すると、MinoPopNumberより1少なくなる

    // ロックダウン //
    private int BottomMoveCount = 0;
    private int BottomMoveCountLimit = 15;
    private int BottomBlockPosition_y = 20;
    private int StartingBottomBlockPosition_y = 20;

    // Hold //
    private bool UseHold = false; // Holdが使用されたか判別する変数
    private bool FirstHold = true; // ゲーム中で最初のHoldかどうかを判別する変数

    // オーディオ //

    // "GameOver"
    // "Hard_Drop"
    // "Hold"
    // "Move_Down"
    // "Move_Left_Right"
    // "Normal_Destroy"
    // "Normal_Drop"
    // "Rotation"
    // "Spin"
    // "Spin_Destroy"
    // "Start_or_Retry"
    // "Tetris"

    // 以上のオーディオが登録されている。

    // 以下の関数で呼び出す
    // AudioManager.Instance.PlaySound("オーディオ名")


    // 干渉するスクリプト //
    Board board;
    GameStatus gameStatus;
    TextEffect textEffect;
    Mino mino;
    SceneTransition sceneTransition;
    Spawner spawner;
    SpinCheck spinCheck;
    Timer timer;


    // インスタンス化 //
    private void Awake()
    {
        board = FindObjectOfType<Board>();
        gameStatus = FindObjectOfType<GameStatus>();
        textEffect = FindObjectOfType<TextEffect>();
        mino = FindObjectOfType<Mino>();
        sceneTransition = FindObjectOfType<SceneTransition>();
        spawner = FindObjectOfType<Spawner>();
        spinCheck = FindObjectOfType<SpinCheck>();
        timer = FindObjectOfType<Timer>();
    }

    private void Start()
    {
        timer.ResetTimer(); // タイマーの初期設定

        int length = 2; // 2回繰り返す

        for (int i = 0; i < length; i++)
        {
            spawner.DetermineSpawnMinoOrder(); // ゲーム開始時、0から13番目のミノの順番を決める
        }

        // // mino.activeMinoのゴーストミノの生成
        // gameStatus.GhostMino = spawner.SpawnMino_Ghost();

        //spawner.CreateNewNextMinos(MinoPopNumber - 1); // Nextの表示

        //mainSceneText.ReadtGoAnimation(); // "Ready Go!" の表示

        // yield return new WaitForSeconds(5.6f);

        spawner.CreateNewActiveMino(MinoPopNumber); // 新しいActiveMinoの生成

        spawner.CreateNewNextMinos(MinoPopNumber); // 新しいNextMinosの生成

        // // Updateの開始
        // while (true)
        // {
        //     Update();
        //     yield return null; // 次のフレームまで待機
        // }
    }

    private void Update()
    {
        if (gameStatus.gameOver)
        {
            return;
        }

        RockDown(); // ロックダウン判定

        PlayerInput(); // プレイヤーが制御できるコマンド

        if (Time.time > timer.AutoDropTimer) // 自動落下
        {
            LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "AutoDown()", "Start");
            AutoDown();
            LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "AutoDown()", "End");
        }
        // if (!board.CheckPosition(spawner.activeMino))
        // {
        //     Debug.LogError("[GameManager Update()] ゲームボードからミノがはみ出した。または、ブロックに重なった。");
        // }

        // if (!board.CheckPosition(spawner.ghostMino))
        // {
        //     Debug.LogError("[GameManager Update()] ゲームボードからゴーストミノがはみ出した。または、ブロックに重なった。");
        // }
    }

    // キーの入力を検知してブロックを動かす関数
    void PlayerInput()
    {
        // 右移動入力
        if (Input.GetKeyDown(KeyCode.D)) // Dキーに割り当て
        {
            LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "MoveRightInput()", "Start");
            MoveRightInput();
            LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "MoveRightInput()", "End");
        }
        // 連続右移動入力
        else if (Input.GetKey(KeyCode.D) && (Time.time > timer.NextKeyLeftRightTimer)) // Dキーが長押しされている時
        {
            LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "ContinuousMoveRightInput()", "Start");
            ContinuousMoveRightInput();
            LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "ContinuousMoveRightInput()", "End");
        }
        // 連続右移動入力の解除
        else if (Input.GetKeyUp(KeyCode.D)) // Dキーを離した時
        {
            LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "ReleaseContinuousMoveRightInput()", "Start");
            ReleaseContinuousMoveRightInput();
            LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "ReleaseContinuousMoveRightInput()", "End");
        }
        // 左移動入力
        else if (Input.GetKeyDown(KeyCode.A)) // Aキーに割り当て
        {
            LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "MoveLeftInput()", "Start");
            MoveLeftInput();
            LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "MoveLeftInput()", "End");
        }
        // 連続左移動入力
        else if (Input.GetKey(KeyCode.A) && (Time.time > timer.NextKeyLeftRightTimer)) // Aキーが長押しされている時
        {
            LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "ContinuousMoveLeftInput()", "Start");
            ContinuousMoveLeftInput();
            LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "ContinuousMoveLeftInput()", "End");
        }
        // 連続左移動入力の解除
        else if (Input.GetKeyUp(KeyCode.A)) // Aキーを離した時
        {
            LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "ReleaseContinuousMoveLeftInput()", "Start");
            ReleaseContinuousMoveLeftInput();
            LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "ReleaseContinuousMoveLeftInput()", "End");
        }
        // 下移動入力
        else if (Input.GetKey(KeyCode.S) && (Time.time > timer.NextKeyDownTimer)) // Sキーに割り当て
        {
            LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "MoveDownInput()", "Start");
            MoveDownInput();
            LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "MoveDownInput()", "End");
        }
        // 右回転入力
        else if (Input.GetKeyDown(KeyCode.P) && (Time.time > timer.NextKeyRotateTimer)) // Pキーに割り当て
        {
            LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "RotateRightInput()", "Start");
            RotateRightInput();
            LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "RotateRightInput()", "End");
        }
        // 左回転入力
        else if (Input.GetKeyDown(KeyCode.L) && (Time.time > timer.NextKeyRotateTimer)) // Lキーに割り当て
        {
            LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "RotateLeftInput()", "Start");
            RotateLeftInput();
            LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "RotateLeftInput()", "End");
        }
        // ハードドロップ入力
        else if (Input.GetKeyDown(KeyCode.Space)) // Spaceキーに割り当て
        {
            LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "HardDropInput()", "Start");
            HardDropInput();
            LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "HardDropInput()", "End");
        }
        // ホールド入力
        else if (Input.GetKeyDown(KeyCode.Return)) // Enter(Return)キーに割り当て
        {
            LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "HoldInput()", "Start");
            HoldInput();
            LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "HoldInput()", "End");
        }
    }

    // 右移動入力時の処理を行う関数 //
    private void MoveRightInput()
    {
        timer.ContinuousLRKey = false; // キーの連続入力でない

        timer.UpdateLeftRightTimer(); // タイマーのアップデート

        spawner.activeMino.MoveRight(); // 右に動かす

        if (!board.CheckPosition(spawner.activeMino)) // 右に動かせない時
        {
            // DebugHelper.Log("Move right failed: Cannot move to the right - Reverting to original position", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

            spawner.activeMino.MoveLeft(); // 左に動かす(元に戻す)
        }
        else // 動かせた時
        {
            // DebugHelper.Log("Move right succeeded: Mino successfully moved to the right", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

            AudioManager.Instance.PlaySound(AudioNames.MoveLeftRight);  // オーディオの再生

            spawner.AdjustGhostMinoPosition(); // ゴーストミノの位置調整

            spinCheck.Reset_SpinTypeName(); // 移動したため、スピン判定をリセット

            gameStatus.Reset_StepsSRS(); // 移動したため、StepsSRSの値を0に
        }

        IncreaseBottomMoveCount(); // BottomMoveCountの値を1増加
    }

    // 連続右移動入力時の処理を行う関数 //
    private void ContinuousMoveRightInput()
    {
        timer.ContinuousLRKey = true; // キーの連続入力がされた

        timer.UpdateLeftRightTimer(); // タイマーのアップデート

        spawner.activeMino.MoveRight(); // 右に動かす

        if (!board.CheckPosition(spawner.activeMino)) // 右に動かせない時
        {
            // DebugHelper.Log("Continuous move right failed: Cannot move to the right - Reverting to original position", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

            spawner.activeMino.MoveLeft(); // 左に動かす(元に戻す)
        }
        else // 動かせた時
        {
            // DebugHelper.Log("Continuous move right succeeded: Mino successfully moved to the right", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

            AudioManager.Instance.PlaySound(AudioNames.MoveLeftRight);  // オーディオの再生

            spawner.AdjustGhostMinoPosition(); // ゴーストミノの位置調整

            spinCheck.Reset_SpinTypeName(); // 移動したため、スピン判定をリセット

            gameStatus.Reset_StepsSRS(); // 移動したため、StepsSRSの値を0に
        }

        IncreaseBottomMoveCount(); // BottomMoveCountの値を1増加
    }

    // 連続右移動入力の解除処理を行う関数 //
    private void ReleaseContinuousMoveRightInput()
    {
        timer.ContinuousLRKey = false; // キーの連続入力でない
    }

    // 左移動入力時の処理を行う関数 //
    private void MoveLeftInput()
    {
        timer.ContinuousLRKey = false; // キーの連続入力でない

        timer.UpdateLeftRightTimer(); // タイマーのアップデート

        spawner.activeMino.MoveLeft(); // 左に動かす

        if (!board.CheckPosition(spawner.activeMino)) // 左に動かせない時
        {
            // DebugHelper.Log("Move left failed: Cannot move to the left - Reverting to original position", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

            spawner.activeMino.MoveRight(); // 右に動かす(元に戻す)
        }
        else // 動かせた時
        {
            // DebugHelper.Log("Move left succeeded: Mino successfully moved to the left", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

            AudioManager.Instance.PlaySound(AudioNames.MoveLeftRight);  // オーディオの再生

            spawner.AdjustGhostMinoPosition(); // ゴーストミノの位置調整

            spinCheck.Reset_SpinTypeName(); // 移動したため、スピン判定をリセット

            gameStatus.Reset_StepsSRS(); // 移動したため、StepsSRSの値を0に
        }

        IncreaseBottomMoveCount(); // BottomMoveCountの値を1増加
    }

    // 連続左移動入力時の処理を行う関数 //
    private void ContinuousMoveLeftInput()
    {
        timer.ContinuousLRKey = true; // キーの連続入力がされた

        timer.UpdateLeftRightTimer(); // タイマーのアップデート

        spawner.activeMino.MoveLeft(); // 左に動かす

        if (!board.CheckPosition(spawner.activeMino)) // 左に動かせない時
        {
            // DebugHelper.Log("Continuous move left failed: Cannot move to the left - Reverting to original position", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

            spawner.activeMino.MoveRight(); // 右に動かす(元に戻す)
        }
        else // 動かせた時
        {
            // DebugHelper.Log("Continuous move left succeeded: Mino successfully moved to the left", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

            AudioManager.Instance.PlaySound(AudioNames.MoveLeftRight);  // オーディオの再生

            spawner.AdjustGhostMinoPosition(); // ゴーストミノの位置調整

            spinCheck.Reset_SpinTypeName(); // 移動したため、スピン判定をリセット

            gameStatus.Reset_StepsSRS(); // 移動したため、StepsSRSの値を0に
        }

        IncreaseBottomMoveCount(); // BottomMoveCountの値を1増加
    }

    // 連続左移動入力の解除処理を行う関数 //
    private void ReleaseContinuousMoveLeftInput()
    {
        timer.ContinuousLRKey = false; // キーの連続入力でない
    }

    // 下移動入力時の処理を行う関数 //
    private void MoveDownInput()
    {
        timer.UpdateDownTimer(); // タイマーのアップデート

        spawner.activeMino.MoveDown(); // 下に動かす

        if (!board.CheckPosition(spawner.activeMino)) // 下に動かせない時
        {
            // DebugHelper.Log("Move down failed: Cannot move down - Reverting to original position", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

            spawner.activeMino.MoveUp(); // 上に動かす(元に戻す)
        }
        else // 動かせた時
        {
            // DebugHelper.Log("Move down succeeded: Mino successfully moved down", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

            AudioManager.Instance.PlaySound(AudioNames.MoveDown);  // オーディオの再生

            if (spinCheck.spinTypeName != SpinTypeNames.I_Spin) // I-Spinは下移動しても解除されないようにしている
            {
                spinCheck.Reset_SpinTypeName(); // 移動したため、スピン判定をリセット
            }

            gameStatus.Reset_StepsSRS(); // 移動したため、StepsSRSの値を0に
        }
    }

    // 右回転入力時の処理を行う関数 //
    private void RotateRightInput()
    {
        timer.UpdateRotateTimer(); // タイマーのアップデート

        gameStatus.Reset_StepsSRS(); // StepsSRSのリセット

        spawner.activeMino.RotateRight(); // 右回転

        if (!board.CheckPosition(spawner.activeMino)) // 通常回転ができなかった時
        {
            // DebugHelper.Log("Normal rotation failed: Trying Super Rotation System (SRS)", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

            if (!mino.SuperRotationSystem()) // SRSもできなかった時
            {
                // DebugHelper.Log("Super Rotation System (SRS) failed: Reverting rotation", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

                gameStatus.Reset_MinoAngleAfter(); // MinoAngleAfterのリセット

                AudioManager.Instance.PlaySound(AudioNames.Rotation);
            }
            else // SRSが成功した時
            {
                // DebugHelper.Log("Super Rotation System (SRS) succeeded", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

                SuccessRotateAction(); // 回転が成功した時の処理を実行
            }
        }
        else // 通常回転が成功した時
        {
            // DebugHelper.Log("Normal rotation succeeded", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

            SuccessRotateAction(); // 回転が成功した時の処理を実行
        }

        IncreaseBottomMoveCount(); // BottomMoveCountの値を1増加
    }

    // 左回転入力時の処理を行う関数 //
    private void RotateLeftInput()
    {
        timer.UpdateRotateTimer(); // タイマーのアップデート

        gameStatus.Reset_StepsSRS(); // StepsSRSのリセット

        spawner.activeMino.RotateLeft(); // 左回転

        if (!board.CheckPosition(spawner.activeMino)) // 通常回転ができなかった時
        {
            // DebugHelper.Log("Normal rotation failed: Trying Super Rotation System (SRS)", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

            if (!mino.SuperRotationSystem()) // SRSもできなかった時
            {
                // DebugHelper.Log("Super Rotation System (SRS) failed: Reverting rotation", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

                gameStatus.Reset_MinoAngleAfter(); // MinoAngleAfterのリセット

                AudioManager.Instance.PlaySound(AudioNames.Rotation);
            }
            else // SRSが成功した時
            {
                // DebugHelper.Log("Super Rotation System (SRS) succeeded", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

                SuccessRotateAction(); // 回転が成功した時の処理を実行
            }
        }
        else // 通常回転が成功した時
        {
            // DebugHelper.Log("Normal rotation succeeded", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

            SuccessRotateAction(); // 回転が成功した時の処理を実行
        }

        IncreaseBottomMoveCount(); // BottomMoveCountの値を1増加
    }

    // 回転が成功した時の処理をする関数 //
    private void SuccessRotateAction()
    {
        LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "SuccessRotateAction()", "Start");

        spawner.AdjustGhostMinoPosition(); // ゴーストミノの位置調整

        gameStatus.UpdateMinoAngleBefore(); // MinoAngleBeforeのアップデート

        spinCheck.CheckSpinType(); // スピン判定のチェック

        if (spinCheck.spinTypeName != SpinTypeNames.None) // スピン判定がない場合
        {
            AudioManager.Instance.PlaySound(AudioNames.Spin);
        }
        else // スピン判定がある場合
        {
            AudioManager.Instance.PlaySound(AudioNames.Rotation);
        }

        LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "SuccessRotateAction()", "End");
    }

    // BottomMoveCountを進める関数 //
    private void IncreaseBottomMoveCount()
    {
        LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "IncreaseBottomMoveCount()", "Start");

        if (BottomMoveCount < BottomMoveCountLimit) // BottomMoveCount が15未満の時
        {
            BottomMoveCount++;
        }

        LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "IncreaseBottomMoveCount()", "End");
    }

    // ハードドロップ入力時の処理を行う関数 //
    private void HardDropInput()
    {
        AudioManager.Instance.PlaySound(AudioNames.HardDrop); // ハードドロップの音を再生

        int height = 20; // ゲームボードの高さの値

        for (int i = 0; i < height; i++) // heightの値分繰り返す
        {
            spawner.activeMino.MoveDown(); // ミノを下に動かす

            if (!board.CheckPosition(spawner.activeMino)) // 底にぶつかった時
            {
                // DebugHelper.Log("Hard drop: Mino hit the bottom, reverting to last valid position", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

                spawner.activeMino.MoveUp(); // ミノを正常な位置に戻す

                break; // For文を抜ける
            }

            gameStatus.Reset_StepsSRS(); // 1マスでも下に移動した時、StepsSRSの値を0にする
        }

        // DebugHelper.Log("Hard drop: Mino reached the bottom", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

        Reset_RockDown(); // RockDownに関する変数のリセット

        SetMinoFixed(); // 底に着いたときの処理
    }

    // ホールド入力時の処理を行う関数 //
    private void HoldInput()
    {
        // Holdは1度使うと、ミノを設置するまで使えない
        // ミノを設置すると、useHold = false になる
        if (UseHold == false)
        {
            // DebugHelper.Log("Hold action initiated", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

            UseHold = true; // ホールドの使用

            AudioManager.Instance.PlaySound(AudioNames.Hold);

            Reset_RockDown(); // RockDownに関する変数のリセット

            if (FirstHold == true) // ゲーム中で最初のHoldだった時
            {
                // DebugHelper.Log("First hold action", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

                MinoPopNumber++;

                // ホールドの処理
                spawner.CreateNewHoldMino(FirstHold, MinoPopNumber); // ActiveMinoをホールドに移動して、新しいミノを生成する

                FirstHold = false; // ゲームオーバーまでfalse
            }
            else
            {
                // DebugHelper.Log("Subsequent hold action", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

                spawner.CreateNewHoldMino(FirstHold, MinoPopNumber);
            }
        }
        else
        {
            // DebugHelper.Log("Hold action ignored: Hold already used", DebugHelper.LogLevel.Warning, "GameManager", "PlayerInput()");
        }
    }

    // 時間経過で落ちる時の処理をする関数 //
    void AutoDown()
    {
        timer.UpdateDownTimer();

        spawner.activeMino.MoveDown();

        if (!board.CheckPosition(spawner.activeMino))
        {
            spawner.activeMino.MoveUp(); // ミノを正常な位置に戻す
        }
        else
        {
            if (spinCheck.spinTypeName != SpinTypeNames.I_Spin) // I-Spinは下移動しても解除されないようにしている
            {
                spinCheck.Reset_SpinTypeName(); // 移動したため、スピン判定をリセット
            }

            gameStatus.Reset_StepsSRS();
        }
    }

    // ロックダウンの処理をする関数 //
    private void RockDown()
    {
        LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "RockDown()", "Start");

        int newBottomBlockPosition_y = board.CheckActiveMinoBottomBlockPosition_y(spawner.activeMino); // ActiveMino の1番下のブロックのy座標を取得

        if (BottomBlockPosition_y <= newBottomBlockPosition_y) // ActivaMinoが、前回のy座標以上の位置にある時
        {
            spawner.activeMino.MoveDown();

            // 1マス下が底の時((底に面している時)
            // かつインターバル時間を超過している、または15回以上移動や回転を行った時
            if (!board.CheckPosition(spawner.activeMino) && (Time.time >= timer.BottomTimer || BottomMoveCount >= BottomMoveCountLimit))
            {
                spawner.activeMino.MoveUp(); // 元の位置に戻す

                // AudioManager.Instance.PlaySound(AudioNames.NormalDrop_Audio);

                SetMinoFixed(); // ミノの設置判定
            }
            else
            {
                spawner.activeMino.MoveUp(); // 元の位置に戻す
            }
        }
        else // ActivaMinoが、前回のy座標より下の位置にある時
        {
            BottomMoveCount = 0; // リセットする

            BottomBlockPosition_y = newBottomBlockPosition_y; // BottomPositionの更新
        }

        LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "RockDown()", "End");
    }

    // RockDownに関する変数のリセット //
    public void Reset_RockDown()
    {
        LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "Reset_RockDown()", "Start");

        BottomBlockPosition_y = StartingBottomBlockPosition_y;
        BottomMoveCount = 0;

        LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "Reset_RockDown()", "End");
    }

    // ミノの設置場所が確定した時の処理をする関数 //
    void SetMinoFixed()
    {
        LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "SetMinoFixed()", "Start");

        if (board.CheckGameOver(spawner.activeMino)) // ミノの設置時にゲームオーバーの条件を満たした場合
        {
            textEffect.StopAnimation();

            gameStatus.Set_GameOver();

            sceneTransition.GameOver();

            return;
        }

        // 各種変数のリセット
        Reset_RockDown();
        UseHold = false;
        timer.ResetTimer();

        board.SaveBlockInGrid(spawner.activeMino); //mino.activeMinoをセーブ

        gameStatus.AddLineClearCountHistory(board.ClearAllRows(), MinoPutNumber); // 横列が埋まっていれば消去し、消去数を記録する

        textEffect.TextDisplay(gameStatus.lineClearCountHistory[MinoPutNumber]); // 消去数、Spinに対応したテキストを表示し、それに対応したSEも鳴らす

        // 各種変数のリセット
        gameStatus.Reset_StepsSRS();
        spinCheck.Reset_SpinTypeName();
        gameStatus.Reset_Angle();

        // Numberを1進める
        MinoPutNumber++;
        MinoPopNumber++;

        spawner.CreateNewActiveMino(MinoPopNumber);

        spawner.CreateNewNextMinos(MinoPopNumber);

        if (!board.CheckPosition(spawner.activeMino)) // ミノを生成した際に、ブロックと重なってしまった場合
        {
            textEffect.StopAnimation();

            gameStatus.Set_GameOver();

            sceneTransition.GameOver();

            return;
        }

        LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "SetMinoFixed()", "End");
    }


    /*void SpinEffect(int i)
    {

    }*/

    //     internal static class AssemblyState
    //     {
    //         public const bool IsDebug =
    // #if DEBUG
    //                 true;
    // #else
    //             false;
    // #endif
    //     }

    // public void Foo()
    // {
    //     if(AssemblyState.IsDebug)
    //     {
    //         Console.WriteLine("デバッグ時に実行");
    //     }
    //     else
    //     {
    //         Console.WriteLine("デバッグ以外で実行");
    //     }
    // }

    // private IEnumerator GameStart()
    // {
    //     mainSceneText.ReadtGoAnimation(); // "Ready Go!" の表示
    //     yield return new WaitForSeconds(2f); // "Ready Go"の表示時間（例えば2秒）
    //     readyGoText.SetActive(false);

    //     // Updateの開始
    //     while (true)
    //     {
    //         Update();
    //         yield return null; // 次のフレームまで待機
    //     }
    // }
}
