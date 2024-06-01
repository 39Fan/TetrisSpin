using UnityEngine;

/// <summary>
/// ゲームマネージャーの統計情報を保持する構造体
/// </summary>
public struct GameManagerStats
{
    /// <summary> ゲームオーバーの判定 </summary>
    private bool gameOver;
    /// <summary> ミノの生成数 </summary>
    /// <remarks>
    /// 新しいミノが生成されるたびに1ずつ増加する
    /// </remarks>
    private int minoPopNumber;
    /// <summary> ミノの設置数 </summary>
    /// <remarks>
    /// ミノが設置されると1ずつ増加する <br/>
    /// ホールドを使用すると、その時点からミノの生成数より1小さくなる
    /// </remarks>
    private int minoPutNumber;
    /// <summary> ロックダウンの移動回数 </summary>
    /// <remarks>
    /// ミノがボトムに到達してからの移動回数を表す
    /// </remarks>
    /// <value> 0~15 </value>
    private int bottomMoveCount;
    /// <summary> ロックダウンの移動回数制限 </summary>
    private int bottomMoveCountLimit;
    /// <summary> 操作中のミノを構成するブロックのうち、最も低い y 座標を保持するプロパティ </summary>
    /// <remarks>
    /// ロックダウンの処理に必要 <br/>
    /// この値が更新されるたびにロックダウンの移動回数制限がリセットされる <br/>
    /// 初期値はゲームボードの最高高さである 20 に設定
    /// </remarks>
    /// <value> 0~20 </value>
    private int lowestBlockPositionY;
    /// <summary> ホールドの使用判定 </summary>
    private bool useHold;
    /// <summary> ゲーム中で最初のホールドの使用判定 </summary>
    private bool firstHold;

    // ゲッタープロパティ //
    public bool GameOver => gameOver;
    public int MinoPopNumber => minoPopNumber;
    public int MinoPutNumber => minoPutNumber;
    public int BottomMoveCount => bottomMoveCount;
    public int BottomMoveCountLimit => bottomMoveCountLimit;
    public int LowestBlockPositionY => lowestBlockPositionY;
    public bool UseHold => useHold;
    public bool FirstHold => firstHold;

    /// <summary> デフォルトコンストラクタ </summary>
    public GameManagerStats(bool _gameOver, int _minoPopNumber, int _minoPutNumber, int _bottomMoveCount,
        int _bottomMoveCountLimit, int _lowestBlockPositionY, bool _useHold, bool _firstHold)
    {
        gameOver = _gameOver;
        minoPopNumber = _minoPopNumber;
        minoPutNumber = _minoPutNumber;
        bottomMoveCount = _bottomMoveCount;
        bottomMoveCountLimit = _bottomMoveCountLimit;
        lowestBlockPositionY = _lowestBlockPositionY;
        useHold = _useHold;
        firstHold = _firstHold;
    }

    /// <summary> デフォルトの <see cref="GameManagerStats"/> を作成する関数 </summary>
    /// <returns>
    /// デフォルト値で初期化された <see cref="GameManagerStats"/> のインスタンス
    /// </returns>
    public static GameManagerStats CreateDefault()
    {
        return new GameManagerStats
        {
            gameOver = false,
            minoPopNumber = 0,
            minoPutNumber = 0,
            bottomMoveCount = 0,
            bottomMoveCountLimit = 15,
            lowestBlockPositionY = 20,
            useHold = false,
            firstHold = true
        };
    }

    /// <summary> 指定されたフィールドの値を更新する関数 </summary>
    /// <param name="_gameOver"> ゲームオーバー判定 </param>
    /// <param name="_minoPopNumber"> ミノの生成数 </param>
    /// <param name="_minoPutNumber"> ミノの設置数 </param>
    /// <param name="_bottomMoveCount"> ロックダウンの移動回数 </param>
    /// <param name="_bottomMoveCountLimit"> ロックダウンの移動回数制限 </param>
    /// <param name="_lowestBlockPositionY"> 最も低いブロックのy座標 </param>
    /// <param name="_useHold"> ホールドの使用判定 </param>
    /// <param name="_firstHold"> ゲーム中で最初のホールドの使用判定 </param>
    /// <returns> 更新された <see cref="GameManagerStats"/> の新しいインスタンス </returns>
    /// <remarks>
    /// 指定されていない引数は現在の値を維持
    /// </remarks>
    public GameManagerStats Update(bool? _gameOver = null, int? _minoPopNumber = null, int? _minoPutNumber = null, int? _bottomMoveCount = null,
        int? _bottomMoveCountLimit = null, int? _lowestBlockPositionY = null, bool? _useHold = null, bool? _firstHold = null)
    {
        var updatedStats = new GameManagerStats(
            _gameOver ?? gameOver,
            _minoPopNumber ?? minoPopNumber,
            _minoPutNumber ?? minoPutNumber,
            _bottomMoveCount ?? bottomMoveCount,
            _bottomMoveCountLimit ?? bottomMoveCountLimit,
            _lowestBlockPositionY ?? lowestBlockPositionY,
            _useHold ?? useHold,
            _firstHold ?? firstHold
        );
        // TODO: ログの記入
        return updatedStats;
    }
}

/// <summary>
/// ゲームの進行を管理するクラス
/// </summary>
public class GameManager : MonoBehaviour
{
    // 干渉するスクリプト //
    AttackCalculator attackCalculator;
    Board board;
    GameStatus gameStatus;
    TextEffect textEffect;
    Mino mino;
    SceneTransition sceneTransition;
    Spawner spawner;
    SpinCheck spinCheck;
    Timer timer;

    // 干渉するストラクト //
    GameManagerStats gameManagerStats;

    /// <summary>
    /// インスタンス化
    /// </summary>
    /// <remarks>
    /// GameManagerStatsの初期化も行う
    /// </remarks>
    private void Awake()
    {
        attackCalculator = FindObjectOfType<AttackCalculator>();
        board = FindObjectOfType<Board>();
        gameStatus = FindObjectOfType<GameStatus>();
        textEffect = FindObjectOfType<TextEffect>();
        mino = FindObjectOfType<Mino>();
        sceneTransition = FindObjectOfType<SceneTransition>();
        spawner = FindObjectOfType<Spawner>();
        spinCheck = FindObjectOfType<SpinCheck>();
        timer = FindObjectOfType<Timer>();

        gameManagerStats = GameManagerStats.CreateDefault();
    }

    /// <summary>
    /// ゲーム開始時の処理
    /// </summary>
    private void Start()
    {
        timer.ResetTimer();

        // ゲーム開始時余分にミノの順番を決める
        spawner.DetermineSpawnMinoOrder();

        spawner.CreateNewActiveMino(gameManagerStats.MinoPopNumber);
        spawner.CreateNewNextMinos(gameManagerStats.MinoPopNumber);
    }

    /// <summary>
    /// 毎フレームの処理
    /// </summary>
    private void Update()
    {
        if (gameManagerStats.GameOver == true)
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
    }

    /// <summary> キーの入力を検知してブロックを動かす関数 </summary>
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

    /// <summary> 右移動入力時の処理を行う関数 </summary>
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

            // 移動したため、以下の処理を実行
            spawner.AdjustGhostMinoPosition();
            spinCheck.ResetSpinTypeName();
            gameStatus.ResetStepsSRS();
        }

        IncreaseBottomMoveCount(); // BottomMoveCountの値を1増加
    }

    /// <summary> 連続右移動入力時の処理を行う関数 </summary>
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

            // 移動したため、以下の処理を実行
            spawner.AdjustGhostMinoPosition();
            spinCheck.ResetSpinTypeName();
            gameStatus.ResetStepsSRS();
        }

        IncreaseBottomMoveCount(); // BottomMoveCountの値を1増加
    }

    /// <summary> 左移動入力時の処理を行う関数 </summary>
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

            // 移動したため、以下の処理を実行
            spawner.AdjustGhostMinoPosition();
            spinCheck.ResetSpinTypeName();
            gameStatus.ResetStepsSRS();
        }

        IncreaseBottomMoveCount(); // BottomMoveCountの値を1増加
    }

    /// <summary> 連続左移動入力時の処理を行う関数 </summary>
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

            // 移動したため、以下の処理を実行
            spawner.AdjustGhostMinoPosition();
            spinCheck.ResetSpinTypeName();
            gameStatus.ResetStepsSRS();
        }

        IncreaseBottomMoveCount(); // BottomMoveCountの値を1増加
    }

    /// <summary> 連続右、または左移動入力の解除処理を行う関数 </summary>
    private void ReleaseContinuousMoveRightLeftInput()
    {
        timer.ContinuousLRKey = false;
    }

    /// <summary> 下移動入力時の処理を行う関数 </summary>
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

            if (spinCheck.SpinTypeName != SpinTypeNames.I_Spin) // I-Spinは下移動しても解除されないようにしている
            {
                spinCheck.ResetSpinTypeName(); // 移動したため、スピン判定をリセット
            }

            gameStatus.ResetStepsSRS(); // 移動したため、StepsSRSの値を0に
        }
    }

    /// <summary> 右回転入力時の処理を行う関数 </summary>
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

            SuccessRotateAction();
        }

        IncreaseBottomMoveCount();
    }

    /// <summary> 左回転入力時の処理を行う関数 </summary>
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

    /// <summary> 回転が成功した時の処理をする関数 </summary>
    private void SuccessRotateAction()
    {
        LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "SuccessRotateAction()", "Start");

        spawner.AdjustGhostMinoPosition(); // ゴーストミノの位置調整

        gameStatus.UpdateMinoAngleBefore(); // MinoAngleBeforeのアップデート

        spinCheck.CheckSpinType(); // スピン判定のチェック

        if (spinCheck.SpinTypeName != SpinTypeNames.None) // スピン判定がない場合
        {
            AudioManager.Instance.PlaySound(AudioNames.Spin);
        }
        else // スピン判定がある場合
        {
            AudioManager.Instance.PlaySound(AudioNames.Rotation);
        }

        LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "SuccessRotateAction()", "End");
    }

    /// <summary> BottomMoveCountを進める関数 </summary>
    private void IncreaseBottomMoveCount()
    {
        LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "IncreaseBottomMoveCount()", "Start");

        if (gameManagerStats.BottomMoveCount < gameManagerStats.BottomMoveCountLimit) // BottomMoveCount が15未満の時
        {
            gameManagerStats = gameManagerStats.Update(_bottomMoveCount: gameManagerStats.BottomMoveCount + 1);
        }

        LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "IncreaseBottomMoveCount()", "End");
    }

    /// <summary> ハードドロップ入力時の処理を行う関数 </summary>
    private void HardDropInput()
    {
        AudioManager.Instance.PlaySound(AudioNames.HardDrop);

        // Heightの値分繰り返す(20)
        for (int i = 0; i < board.Height; i++)
        {
            spawner.activeMino.MoveDown();

            if (!board.CheckPosition(spawner.activeMino)) // 底にぶつかった時
            {
                // DebugHelper.Log("Hard drop: Mino hit the bottom, reverting to last valid position", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

                spawner.activeMino.MoveUp(); // ミノを正常な位置に戻す

                break;
            }

            // 以下一マスでも下に移動した時の処理
            if (spinCheck.SpinTypeName != SpinTypeNames.I_Spin) // I-Spinは下移動しても解除されないようにしている
            {
                spinCheck.ResetSpinTypeName();
            }
            gameStatus.ResetStepsSRS();
        }

        // DebugHelper.Log("Hard drop: Mino reached the bottom", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

        ResetRockDown();

        SetMinoFixed();
    }

    /// <summary> ホールド入力時の処理を行う関数 </summary>
    private void HoldInput()
    {
        // Holdは1度使うと、ミノを設置するまで使えない
        if (gameManagerStats.UseHold == false)
        {
            // DebugHelper.Log("Hold action initiated", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

            gameManagerStats = gameManagerStats.Update(_useHold: true);

            AudioManager.Instance.PlaySound(AudioNames.Hold);

            ResetRockDown();

            if (gameManagerStats.FirstHold == true) // ゲーム中で最初のHoldだった時
            {
                // DebugHelper.Log("First hold action", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

                gameManagerStats = gameManagerStats.Update(_minoPopNumber: gameManagerStats.MinoPopNumber + 1);

                spawner.CreateNewHoldMino(gameManagerStats.FirstHold, gameManagerStats.MinoPopNumber);

                gameManagerStats = gameManagerStats.Update(_firstHold: false);
            }
            else
            {
                // DebugHelper.Log("Subsequent hold action", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

                spawner.CreateNewHoldMino(gameManagerStats.FirstHold, gameManagerStats.MinoPopNumber);
            }
        }
        else
        {
            // DebugHelper.Log("Hold action ignored: Hold already used", DebugHelper.LogLevel.Warning, "GameManager", "PlayerInput()");
        }
    }

    /// <summary> 時間経過で落ちる時の処理をする関数 </summary>
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
            if (spinCheck.SpinTypeName != SpinTypeNames.I_Spin) // I-Spinは下移動しても解除されないようにしている
            {
                spinCheck.ResetSpinTypeName(); // 移動したため、スピン判定をリセット
            }

            gameStatus.ResetStepsSRS();
        }
    }

    /// <summary> ロックダウンの処理をする関数 </summary>
    private void RockDown()
    {
        LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "RockDown()", "Start");

        int newBottomBlockPosition_y = board.CheckActiveMinoBottomBlockPosition_y(spawner.activeMino); // 操作中のミノの1番下のブロックのy座標を取得

        if (gameManagerStats.LowestBlockPositionY <= newBottomBlockPosition_y) // lowestBlockPositionYが更新されていない場合
        {
            spawner.activeMino.MoveDown();

            // 1マス下が底の時((底に面している時)
            // かつインターバル時間を超過している、または15回以上移動や回転を行った時
            if (!board.CheckPosition(spawner.activeMino) && (Time.time >= timer.BottomTimer ||
                gameManagerStats.BottomMoveCount >= gameManagerStats.BottomMoveCountLimit))
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
        else // lowestBlockPositionYが更新された場合
        {
            gameManagerStats = gameManagerStats.Update(_bottomMoveCount: 0);

            gameManagerStats = gameManagerStats.Update(_lowestBlockPositionY: newBottomBlockPosition_y); // BottomPositionの更新
        }

        LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "RockDown()", "End");
    }

    /// <summary> RockDownに関する変数のリセット </summary>
    public void ResetRockDown()
    {
        LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "ResetRockDown()", "Start");
        gameManagerStats = gameManagerStats.Update(_bottomMoveCount: 0, _lowestBlockPositionY: 20);
        LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "ResetRockDown()", "End");
    }

    /// <summary> ミノの設置場所が確定した時の処理をする関数 </summary>
    void SetMinoFixed()
    {
        LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "SetMinoFixed()", "Start");

        if (board.CheckGameOver(spawner.activeMino)) // ミノの設置時にゲームオーバーの条件を満たした場合
        {
            textEffect.StopAnimation();

            gameManagerStats.Update(_gameOver: true);

            sceneTransition.GameOver();

            return;
        }

        // 各種変数のリセット
        ResetRockDown();
        timer.ResetTimer();

        board.SaveBlockInGrid(spawner.activeMino);
        gameStatus.AddLineClearCountHistory(board.ClearAllRows());
        textEffect.TextDisplay(spinCheck.SpinTypeName, gameStatus.LineClearCountHistory[gameManagerStats.MinoPutNumber]);
        attackCalculator.CalculateAttackLines(spinCheck.SpinTypeName, gameStatus.LineClearCountHistory[gameManagerStats.MinoPutNumber]);

        // 各種変数のリセット
        gameStatus.ResetStepsSRS();
        spinCheck.ResetSpinTypeName();
        gameStatus.ResetAngle();

        gameManagerStats = gameManagerStats.Update(_minoPutNumber: gameManagerStats.MinoPutNumber + 1,
            _minoPopNumber: gameManagerStats.MinoPopNumber + 1, _useHold: false);

        spawner.CreateNewActiveMino(gameManagerStats.MinoPopNumber);

        spawner.CreateNewNextMinos(gameManagerStats.MinoPopNumber);

        if (!board.CheckPosition(spawner.activeMino)) // ミノを生成した際に、ブロックと重なってしまった場合
        {
            textEffect.StopAnimation();

            gameManagerStats.Update(_gameOver: true);

            sceneTransition.GameOver();

            return;
        }

        LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "SetMinoFixed()", "End");
    }
}

/////////////////// 旧コード ///////////////////

// // 初期化例
// private GameManagerStats minoStats = new GameManagerStats();

// // MinoPopNumberを1増やす
// minoStats = minoStats.Update(minoPopNumber: minoStats.MinoPopNumber + 1);

// // UseHoldをtrueに変更
// minoStats = minoStats.Update(useHold: true);

// /// <summary> ミノの生成数
// /// </summary>
// /// <remarks>
// /// 新しいミノが生成されるたびに1ずつ増加する
// /// </remarks>
// private int minoPopNumber = 0;

// /// <summary>ミノの設置数
// /// ミノが設置されると1ずつ増加する
// /// ホールドを使用すると、その時点からミノの生成数より1小さくなる
// /// </summary>
// private int minoPutNumber = 0;

// /// <summary>ロックダウンの移動回数
// /// </summary>
// /// <remarks>ミノがボトムに到達してからの移動回数を表す</remarks>
// /// <value>0~15</value>
// private int bottomMoveCount = 0;

// /// <summary>ロックダウンの移動回数制限</summary>
// private int bottomMoveCountLimit = 15;

// /// <summary>操作中のミノを構成するブロックのうち、最も低い y 座標を保持するプロパティ
// /// </summary>
// /// <remarks>
// /// ロックダウンの処理に必要 <br/>
// /// この値が更新されるたびにロックダウンの移動回数制限がリセットされる <br/>
// /// 初期値はゲームボードの最高高さである 20 に設定
// /// </remarks>
// /// <value>0~20</value>
// private int lowestBlockPositionY = 20;

// /// <summary>ホールドが使用されたか判別するプロパティ</summary>
// private bool useHold = false;

// /// <summary>ゲーム中で最初のホールドかどうかを判別するプロパティ</summary>
// private bool firstHold = true;

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

// /// <summary>ロックダウンの初期 y 座標</summary>
// private int startinglowestBlockYPositionY = 20;

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


// // mino.activeMinoのゴーストミノの生成
// gameStatus.GhostMino = spawner.SpawnMino_Ghost();

// spawner.CreateNewNextMinos(MinoPopNumber - 1); // Nextの表示

// mainSceneText.ReadtGoAnimation(); // "Ready Go!" の表示

// yield return new WaitForSeconds(5.6f);

// // Updateの開始
// while (true)
// {
//     Update();
//     yield return null; // 次のフレームまで待機
// }

// if (!board.CheckPosition(spawner.activeMino))
// {
//     Debug.LogError("[GameManager Update()] ゲームボードからミノがはみ出した。または、ブロックに重なった。");
// }

// if (!board.CheckPosition(spawner.ghostMino))
// {
//     Debug.LogError("[GameManager Update()] ゲームボードからゴーストミノがはみ出した。または、ブロックに重なった。");
// }

// // 連続左移動入力の解除処理を行う関数 //
// private void ReleaseContinuousMoveLeftInput()
// {
//     timer.ContinuousLRKey = false; // キーの連続入力でない
// }

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
//     mainSceneText.ReadyGoAnimation(); // "Ready Go!" の表示
//     yield return new WaitForSeconds(2f); // "Ready Go"の表示時間（例えば2秒）
//     readyGoText.SetActive(false);

//     // Updateの開始
//     while (true)
//     {
//         Update();
//         yield return null; // 次のフレームまで待機
//     }
// }

/////////////////////////////////////////////////////////