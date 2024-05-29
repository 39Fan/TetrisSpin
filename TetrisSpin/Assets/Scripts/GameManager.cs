using UnityEngine;
using UnityEngine.Analytics;

/// <summary>
/// ゲームの進行を管理するクラス
/// </summary>
public class GameManager : MonoBehaviour
{
    // // ミノの生成数、または設置数 //
    // private int MinoPopNumber = 0;
    // private int MinoPutNumber = 0; // Holdを使用すると、MinoPopNumberより1少なくなる

    // // ロックダウン //
    // private int BottomMoveCount = 0;
    // private int BottomMoveCountLimit = 15;
    // private int BottomBlockPosition_y = 20;
    // private int StartingBottomBlockPosition_y = 20;

    // // Hold //
    // private bool UseHold = false; // Holdが使用されたか判別する変数
    // private bool FirstHold = true; // ゲーム中で最初のHoldかどうかを判別する変数

    /// <summary>ミノの生成数</summary>
    private int minoPopNumber = 0;
    /// <summary>ミノの設置数。ホールドを使用すると、その時点からミノの生成数より1小さくなる</summary>
    private int minoPutNumber = 0;

    /// <summary>ロックダウンの移動回数</summary>
    private int bottomMoveCount = 0;
    /// <summary>ロックダウンの移動回数制限</summary>
    private int bottomMoveCountLimit = 15;
    /// <summary>ロックダウンの底の y 座標</summary>
    private int bottomBlockPositionY = 20;
    /// <summary>ロックダウンの初期 y 座標</summary>
    private int startingBottomBlockPositionY = 20;

    /// <summary>ホールドが使用されたか判別するフラグ</summary>
    private bool useHold = false;
    /// <summary>ゲーム中で最初のホールドかどうかを判別するフラグ</summary>
    private bool firstHold = true;

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


    /// <summary>
    /// インスタンス化時の処理
    /// </summary>
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

    /// <summary>
    /// ゲーム開始時の処理
    /// </summary>
    private void Start()
    {
        // タイマーの初期設定
        timer.ResetTimer();

        // ゲーム開始時余分にミノの順番を決める
        spawner.DetermineSpawnMinoOrder();

        // // mino.activeMinoのゴーストミノの生成
        // gameStatus.GhostMino = spawner.SpawnMino_Ghost();

        //spawner.CreateNewNextMinos(MinoPopNumber - 1); // Nextの表示

        //mainSceneText.ReadtGoAnimation(); // "Ready Go!" の表示

        // yield return new WaitForSeconds(5.6f);

        // 操作するミノの生成
        spawner.CreateNewActiveMino(minoPopNumber);

        // 新しいネクストミノの生成
        spawner.CreateNewNextMinos(minoPopNumber);

        // // Updateの開始
        // while (true)
        // {
        //     Update();
        //     yield return null; // 次のフレームまで待機
        // }
    }

    /// <summary>
    /// 毎フレームの処理
    /// </summary>
    private void Update()
    {
        if (gameStatus.GameOver)
        {
            return;
        }

        RockDown();

        PlayerInput();

        // 自動落下
        if (Time.time > timer.AutoDropTimer)
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

    /// <summary>
    /// キーの入力を検知してブロックを動かす関数
    /// </summary>
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
            LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "ReleaseContinuousMoveRightLeftInput()", "Start");
            ReleaseContinuousMoveRightLeftInput();
            LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "ReleaseContinuousMoveRightLeftInput()", "End");
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
            LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "ReleaseContinuousMoveRightLeftInput()", "Start");
            ReleaseContinuousMoveRightLeftInput();
            LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "ReleaseContinuousMoveRightLeftInput()", "End");
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

    /// <summary>
    /// 右移動入力時の処理を行う関数
    /// </summary>
    private void MoveRightInput()
    {
        timer.ContinuousLRKey = false; // キーの連続入力でない判定を付与

        timer.UpdateLeftRightTimer();

        spawner.activeMino.MoveRight();

        if (!board.CheckPosition(spawner.activeMino)) // 右に動かせない時
        {
            // DebugHelper.Log("Move right failed: Cannot move to the right - Reverting to original position", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

            spawner.activeMino.MoveLeft(); // 左に動かす(元に戻すため)
        }
        else // 動かせた時
        {
            // DebugHelper.Log("Move right succeeded: Mino successfully moved to the right", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

            AudioManager.Instance.PlaySound(AudioNames.MoveLeftRight);

            spawner.AdjustGhostMinoPosition(); // ゴーストミノの位置調整

            spinCheck.ResetSpinTypeName(); // 移動したため、スピン判定をリセット

            gameStatus.ResetStepsSRS(); // 移動したため、StepsSRSの値を0に
        }

        IncreaseBottomMoveCount(); // BottomMoveCountの値を1増加
    }

    /// <summary>
    /// 連続右移動入力時の処理を行う関数
    /// </summary>
    private void ContinuousMoveRightInput()
    {
        timer.ContinuousLRKey = true; // キーの連続入力判定を付与

        timer.UpdateLeftRightTimer();

        spawner.activeMino.MoveRight(); // 右に動かす

        if (!board.CheckPosition(spawner.activeMino)) // 右に動かせない時
        {
            // DebugHelper.Log("Continuous move right failed: Cannot move to the right - Reverting to original position", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

            spawner.activeMino.MoveLeft(); // 左に動かす(元に戻すため)
        }
        else // 動かせた時
        {
            // DebugHelper.Log("Continuous move right succeeded: Mino successfully moved to the right", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

            AudioManager.Instance.PlaySound(AudioNames.MoveLeftRight);

            spawner.AdjustGhostMinoPosition(); // ゴーストミノの位置調整

            spinCheck.ResetSpinTypeName(); // 移動したため、スピン判定をリセット

            gameStatus.ResetStepsSRS(); // 移動したため、StepsSRSの値を0に
        }

        IncreaseBottomMoveCount(); // BottomMoveCountの値を1増加
    }

    /// <summary>
    /// 連続右、または左移動入力の解除処理を行う関数
    /// </summary>
    private void ReleaseContinuousMoveRightLeftInput()
    {
        timer.ContinuousLRKey = false;
    }

    /// <summary>
    /// 左移動入力時の処理を行う関数
    /// </summary>
    private void MoveLeftInput()
    {
        timer.ContinuousLRKey = false; // キーの連続入力でない

        timer.UpdateLeftRightTimer();

        spawner.activeMino.MoveLeft();

        if (!board.CheckPosition(spawner.activeMino)) // 左に動かせない時
        {
            // DebugHelper.Log("Move left failed: Cannot move to the left - Reverting to original position", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

            spawner.activeMino.MoveRight(); // 右に動かす(元に戻すため)
        }
        else // 動かせた時
        {
            // DebugHelper.Log("Move left succeeded: Mino successfully moved to the left", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

            AudioManager.Instance.PlaySound(AudioNames.MoveLeftRight);

            spawner.AdjustGhostMinoPosition(); // ゴーストミノの位置調整

            spinCheck.ResetSpinTypeName(); // 移動したため、スピン判定をリセット

            gameStatus.ResetStepsSRS(); // 移動したため、StepsSRSの値を0に
        }

        IncreaseBottomMoveCount(); // BottomMoveCountの値を1増加
    }

    /// <summary>
    /// 連続左移動入力時の処理を行う関数
    /// </summary>
    private void ContinuousMoveLeftInput()
    {
        timer.ContinuousLRKey = true; // キーの連続入力がされた

        timer.UpdateLeftRightTimer();

        spawner.activeMino.MoveLeft();

        if (!board.CheckPosition(spawner.activeMino)) // 左に動かせない時
        {
            // DebugHelper.Log("Continuous move left failed: Cannot move to the left - Reverting to original position", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

            spawner.activeMino.MoveRight(); // 右に動かす(元に戻すため)
        }
        else // 動かせた時
        {
            // DebugHelper.Log("Continuous move left succeeded: Mino successfully moved to the left", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

            AudioManager.Instance.PlaySound(AudioNames.MoveLeftRight);

            spawner.AdjustGhostMinoPosition(); // ゴーストミノの位置調整

            spinCheck.ResetSpinTypeName(); // 移動したため、スピン判定をリセット

            gameStatus.ResetStepsSRS(); // 移動したため、StepsSRSの値を0に
        }

        IncreaseBottomMoveCount(); // BottomMoveCountの値を1増加
    }

    // // 連続左移動入力の解除処理を行う関数 //
    // private void ReleaseContinuousMoveLeftInput()
    // {
    //     timer.ContinuousLRKey = false; // キーの連続入力でない
    // }

    /// <summary>
    /// 下移動入力時の処理を行う関数
    /// </summary>
    private void MoveDownInput()
    {
        timer.UpdateDownTimer();

        spawner.activeMino.MoveDown();

        if (!board.CheckPosition(spawner.activeMino)) // 下に動かせない時
        {
            // DebugHelper.Log("Move down failed: Cannot move down - Reverting to original position", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

            spawner.activeMino.MoveUp(); // 上に動かす(元に戻すため)
        }
        else // 動かせた時
        {
            // DebugHelper.Log("Move down succeeded: Mino successfully moved down", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

            AudioManager.Instance.PlaySound(AudioNames.MoveDown);

            if (spinCheck.spinTypeName != SpinTypeNames.I_Spin) // I-Spinは下移動しても解除されないようにしている
            {
                spinCheck.ResetSpinTypeName(); // 移動したため、スピン判定をリセット
            }

            gameStatus.ResetStepsSRS(); // 移動したため、StepsSRSの値を0に
        }
    }

    /// <summary>
    /// 右回転入力時の処理を行う関数
    /// </summary>
    private void RotateRightInput()
    {
        timer.UpdateRotateTimer();

        gameStatus.ResetStepsSRS(); // StepsSRSのリセット

        spawner.activeMino.RotateRight();

        if (!board.CheckPosition(spawner.activeMino)) // 通常回転ができなかった時
        {
            // DebugHelper.Log("Normal rotation failed: Trying Super Rotation System (SRS)", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

            if (!mino.SuperRotationSystem()) // SRSもできなかった時
            {
                // DebugHelper.Log("Super Rotation System (SRS) failed: Reverting rotation", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

                gameStatus.ResetMinoAngleAfter(); // MinoAngleAfterのリセット

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

    /// <summary>
    /// 左回転入力時の処理を行う関数
    /// </summary>
    private void RotateLeftInput()
    {
        timer.UpdateRotateTimer();

        gameStatus.ResetStepsSRS(); // StepsSRSのリセット

        spawner.activeMino.RotateLeft();

        if (!board.CheckPosition(spawner.activeMino)) // 通常回転ができなかった時
        {
            // DebugHelper.Log("Normal rotation failed: Trying Super Rotation System (SRS)", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

            if (!mino.SuperRotationSystem()) // SRSもできなかった時
            {
                // DebugHelper.Log("Super Rotation System (SRS) failed: Reverting rotation", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

                gameStatus.ResetMinoAngleAfter(); // MinoAngleAfterのリセット

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

    /// <summary>
    /// 回転が成功した時の処理をする関数
    /// </summary>
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

    /// <summary>
    /// BottomMoveCountを進める関数
    /// </summary>
    private void IncreaseBottomMoveCount()
    {
        LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "IncreaseBottomMoveCount()", "Start");

        if (bottomMoveCount < bottomMoveCountLimit) // BottomMoveCount が15未満の時
        {
            bottomMoveCount++;
        }

        LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "IncreaseBottomMoveCount()", "End");
    }

    /// <summary>
    /// ハードドロップ入力時の処理を行う関数
    /// </summary>
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

            gameStatus.ResetStepsSRS(); // 1マスでも下に移動した時、StepsSRSの値を0にする
        }

        // DebugHelper.Log("Hard drop: Mino reached the bottom", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

        ResetRockDown(); // RockDownに関する変数のリセット

        SetMinoFixed(); // 底に着いたときの処理
    }

    /// <summary>
    /// ホールド入力時の処理を行う関数
    /// </summary>
    private void HoldInput()
    {
        // Holdは1度使うと、ミノを設置するまで使えない
        // ミノを設置すると、useHold = false になる
        if (useHold == false)
        {
            // DebugHelper.Log("Hold action initiated", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

            useHold = true; // ホールドの使用

            AudioManager.Instance.PlaySound(AudioNames.Hold);

            ResetRockDown(); // RockDownに関する変数のリセット

            if (firstHold == true) // ゲーム中で最初のHoldだった時
            {
                // DebugHelper.Log("First hold action", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

                minoPopNumber++;

                // ホールドの処理
                spawner.CreateNewHoldMino(firstHold, minoPopNumber); // ActiveMinoをホールドに移動して、新しいミノを生成する

                firstHold = false; // ゲームオーバーまでfalse
            }
            else
            {
                // DebugHelper.Log("Subsequent hold action", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

                spawner.CreateNewHoldMino(firstHold, minoPopNumber);
            }
        }
        else
        {
            // DebugHelper.Log("Hold action ignored: Hold already used", DebugHelper.LogLevel.Warning, "GameManager", "PlayerInput()");
        }
    }

    /// <summary>
    /// 時間経過で落ちる時の処理をする関数
    /// </summary>
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
                spinCheck.ResetSpinTypeName(); // 移動したため、スピン判定をリセット
            }

            gameStatus.ResetStepsSRS();
        }
    }

    /// <summary>
    /// ロックダウンの処理をする関数
    /// </summary>
    private void RockDown()
    {
        LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "RockDown()", "Start");

        int newBottomBlockPosition_y = board.CheckActiveMinoBottomBlockPosition_y(spawner.activeMino); // ActiveMino の1番下のブロックのy座標を取得

        if (bottomBlockPositionY <= newBottomBlockPosition_y) // ActivaMinoが、前回のy座標以上の位置にある時
        {
            spawner.activeMino.MoveDown();

            // 1マス下が底の時((底に面している時)
            // かつインターバル時間を超過している、または15回以上移動や回転を行った時
            if (!board.CheckPosition(spawner.activeMino) && (Time.time >= timer.BottomTimer || bottomMoveCount >= bottomMoveCountLimit))
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
            bottomMoveCount = 0; // リセットする

            bottomBlockPositionY = newBottomBlockPosition_y; // BottomPositionの更新
        }

        LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "RockDown()", "End");
    }

    /// <summary>
    /// RockDownに関する変数のリセット
    /// </summary>
    public void ResetRockDown()
    {
        LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "ResetRockDown()", "Start");

        bottomBlockPositionY = startingBottomBlockPositionY;
        bottomMoveCount = 0;

        LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "ResetRockDown()", "End");
    }

    /// <summary>
    /// ミノの設置場所が確定した時の処理をする関数
    /// </summary>
    void SetMinoFixed()
    {
        LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "SetMinoFixed()", "Start");

        if (board.CheckGameOver(spawner.activeMino)) // ミノの設置時にゲームオーバーの条件を満たした場合
        {
            textEffect.StopAnimation();

            gameStatus.SetGameOver();

            sceneTransition.GameOver();

            return;
        }

        // 各種変数のリセット
        ResetRockDown();
        useHold = false;
        timer.ResetTimer();

        board.SaveBlockInGrid(spawner.activeMino); //mino.activeMinoをセーブ

        gameStatus.AddLineClearCountHistory(board.ClearAllRows()); // 横列が埋まっていれば消去し、消去数を記録する

        textEffect.TextDisplay(gameStatus.LineClearCountHistory[minoPutNumber]); // 消去数、Spinに対応したテキストを表示し、それに対応したSEも鳴らす

        // 各種変数のリセット
        gameStatus.ResetStepsSRS();
        spinCheck.ResetSpinTypeName();
        gameStatus.ResetAngle();

        // Numberを1進める
        minoPutNumber++;
        minoPopNumber++;

        spawner.CreateNewActiveMino(minoPopNumber);

        spawner.CreateNewNextMinos(minoPopNumber);

        if (!board.CheckPosition(spawner.activeMino)) // ミノを生成した際に、ブロックと重なってしまった場合
        {
            textEffect.StopAnimation();

            gameStatus.SetGameOver();

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
