using UnityEngine;

/// <summary> ゲームモードの種類 列挙型 </summary>
public enum eGameModeType
{
    TimeAttack_100,
    SpinMaster,
    Practice,
    None // デフォルト値
}

/// <summary> ゲームレベルの種類 列挙型 </summary>
public enum eGameLevelType
{
    Easy,
    Normal,
    Hard,
    None // デフォルト値
}

/// <summary>
/// ゲームマネージャーの統計情報を保持する静的クラス
/// </summary>
public static class GameManagerStats
{
    /// <summary> ゲームモード </summary>
    private static eGameModeType gameMode;
    /// <summary> ゲームレベル </summary>
    private static eGameLevelType gameLevel;
    /// <summary> ミノの生成数 </summary>
    private static int minoPopNumber = 0;
    /// <summary> ミノの設置数 </summary>
    private static int minoPutNumber = 0;

    // ゲッタープロパティ //
    public static eGameModeType GameMode => gameMode;
    public static eGameLevelType GameLevel => gameLevel;
    public static int MinoPopNumber => minoPopNumber;
    public static int MinoPutNumber => minoPutNumber;

    /// <summary> スタッツログの詳細 </summary>
    private static string logStatsDetail;

    /// <summary> 指定されたフィールドの値を更新する関数 </summary>
    /// <param name="_gameMode"> ゲームモード </param>
    /// <param name="_level"> ゲームレベル </param>
    /// <param name="_minoPopNumber"> ミノの生成数 </param>
    /// <param name="_minoPutNumber"> ミノの設置数 </param>
    /// <remarks>
    /// 指定されていない引数は現在の値を維持
    /// </remarks>
    public static void UpdateStats(eGameModeType? _gameMode = null, eGameLevelType? _gameLevel = null, int? _minoPopNumber = null, int? _minoPutNumber = null)
    {
        LogHelper.DebugLog(eClasses.GameAutoRunnerStats, eMethod.UpdateStats, eLogTitle.Start);

        gameMode = _gameMode ?? gameMode;
        gameLevel = _gameLevel ?? gameLevel;
        minoPopNumber = _minoPopNumber ?? minoPopNumber;
        minoPutNumber = _minoPutNumber ?? minoPutNumber;

        logStatsDetail = $"gameMode : {gameMode}, level : {gameLevel}, minoPopNumber : {minoPopNumber}, minoPutNumber : {minoPutNumber}";
        LogHelper.InfoLog(eClasses.GameAutoRunnerStats, eMethod.UpdateStats, eLogTitle.StatsInfo, logStatsDetail);

        LogHelper.DebugLog(eClasses.GameAutoRunnerStats, eMethod.UpdateStats, eLogTitle.End);
    }

    /// <summary> デフォルトの <see cref="GameManagerStats"/> にリセットする関数 </summary>
    public static void ResetStats()
    {
        LogHelper.DebugLog(eClasses.GameAutoRunnerStats, eMethod.ResetStats, eLogTitle.Start);

        minoPopNumber = 0;
        minoPutNumber = 0;

        LogHelper.DebugLog(eClasses.GameAutoRunnerStats, eMethod.ResetStats, eLogTitle.End);
    }

    /// <summary> ゲームモードをデフォルトにリセットする関数 </summary>
    public static void ResetGameMode()
    {
        LogHelper.DebugLog(eClasses.GameAutoRunnerStats, eMethod.ResetGameMode, eLogTitle.Start);

        gameMode = default;

        LogHelper.DebugLog(eClasses.GameAutoRunnerStats, eMethod.ResetGameMode, eLogTitle.End);
    }

    /// <summary> ゲームレベルをデフォルトにリセットする関数 </summary>
    public static void ResetGameLevel()
    {
        LogHelper.DebugLog(eClasses.GameAutoRunnerStats, eMethod.ResetGameLevel, eLogTitle.Start);

        gameLevel = default;

        LogHelper.DebugLog(eClasses.GameAutoRunnerStats, eMethod.ResetGameLevel, eLogTitle.End);
    }
}


/// <summary>
/// ゲームの進行を管理するクラス
/// </summary>
public class GameManager : MonoBehaviour
{
    // 干渉するスクリプト //
    GameAutoRunner gameAutoRunner;
    PlayerInput playerInput;
    Spawner spawner;

    /// <summary>
    /// インスタンス化
    /// </summary>
    private void Awake()
    {
        gameAutoRunner = FindObjectOfType<GameAutoRunner>();
        playerInput = FindObjectOfType<PlayerInput>();
        spawner = FindObjectOfType<Spawner>();
    }

    /// <summary>
    /// ゲーム開始時の処理
    /// </summary>
    private void Start()
    {
        SpawnerStats.ResetStats();

        // ゲーム開始時余分にミノの順番を決める
        spawner.DetermineSpawnMinoOrder();

        spawner.CreateNewActiveMino(GameManagerStats.MinoPopNumber);
        spawner.CreateNextMinos(GameManagerStats.MinoPopNumber);
    }

    /// <summary>
    /// 毎フレームの処理
    /// </summary>
    private void Update()
    {
        if (!GameSceneManagerStats.PlayScene || GameSceneManagerStats.PoseState)
        {
            return;
        }

        gameAutoRunner.RockDown();

        playerInput.InputInGame();

        // 自動落下
        if (Time.time > CoolDownTimer.AutoDropCoolDownTimer)
        {
            gameAutoRunner.AutoDown();
        }
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

// if (!board.CheckPosition(spawner.ActiveMino))
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
//     Timer.ContinuousLRKey = false; // キーの連続入力でない
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

// /// <summary>
// /// ゲームマネージャーの統計情報を保持する構造体
// /// </summary>
// public struct GameManagerStats
// {
//     /// <summary> ゲームオーバーの判定 </summary>
//     private bool gameOver;
//     /// <summary> ミノの生成数 </summary>
//     /// <remarks>
//     /// 新しいミノが生成されるたびに1ずつ増加する
//     /// </remarks>
//     private int minoPopNumber;
//     /// <summary> ミノの設置数 </summary>
//     /// <remarks>
//     /// ミノが設置されると1ずつ増加する <br/>
//     /// ホールドを使用すると、その時点からミノの生成数より1小さくなる
//     /// </remarks>
//     private int minoPutNumber;
//     /// <summary> ロックダウンの移動回数 </summary>
//     /// <remarks>
//     /// ミノがボトムに到達してからの移動回数を表す
//     /// </remarks>
//     /// <value> 0~15 </value>
//     private int bottomMoveCount;
//     /// <summary> ロックダウンの移動回数制限 </summary>
//     private int bottomMoveCountLimit;
//     /// <summary> 操作中のミノを構成するブロックのうち、最も低い y 座標を保持するプロパティ </summary>
//     /// <remarks>
//     /// ロックダウンの処理に必要 <br/>
//     /// この値が更新されるたびにロックダウンの移動回数制限がリセットされる <br/>
//     /// 初期値はゲームボードの最高高さである 20 に設定
//     /// </remarks>
//     /// <value> 0~20 </value>
//     private int lowestBlockPositionY;
//     /// <summary> ホールドの使用判定 </summary>
//     private bool useHold;
//     /// <summary> ゲーム中で最初のホールドの使用判定 </summary>
//     private bool firstHold;

//     // ゲッタープロパティ //
//     public bool GameOver => gameOver;
//     public int MinoPopNumber => minoPopNumber;
//     public int MinoPutNumber => minoPutNumber;
//     public int BottomMoveCount => bottomMoveCount;
//     public int BottomMoveCountLimit => bottomMoveCountLimit;
//     public int LowestBlockPositionY => lowestBlockPositionY;
//     public bool UseHold => useHold;
//     public bool FirstHold => firstHold;

//     /// <summary> デフォルトコンストラクタ </summary>
//     public GameManagerStats(bool _gameOver, int _minoPopNumber, int _minoPutNumber, int _bottomMoveCount,
//         int _bottomMoveCountLimit, int _lowestBlockPositionY, bool _useHold, bool _firstHold)
//     {
//         gameOver = _gameOver;
//         minoPopNumber = _minoPopNumber;
//         minoPutNumber = _minoPutNumber;
//         bottomMoveCount = _bottomMoveCount;
//         bottomMoveCountLimit = _bottomMoveCountLimit;
//         lowestBlockPositionY = _lowestBlockPositionY;
//         useHold = _useHold;
//         firstHold = _firstHold;
//     }

//     /// <summary> デフォルトの <see cref="GameManagerStats"/> を作成する関数 </summary>
//     /// <returns>
//     /// デフォルト値で初期化された <see cref="GameManagerStats"/> のインスタンス
//     /// </returns>
//     public static GameManagerStats CreateDefault()
//     {
//         return new GameManagerStats
//         {
//             gameOver = false,
//             minoPopNumber = 0,
//             minoPutNumber = 0,
//             bottomMoveCount = 0,
//             bottomMoveCountLimit = 15,
//             lowestBlockPositionY = 20,
//             useHold = false,
//             firstHold = true
//         };
//     }

//     /// <summary> 指定されたフィールドの値を更新する関数 </summary>
//     /// <param name="_gameOver"> ゲームオーバー判定 </param>
//     /// <param name="_minoPopNumber"> ミノの生成数 </param>
//     /// <param name="_minoPutNumber"> ミノの設置数 </param>
//     /// <param name="_bottomMoveCount"> ロックダウンの移動回数 </param>
//     /// <param name="_bottomMoveCountLimit"> ロックダウンの移動回数制限 </param>
//     /// <param name="_lowestBlockPositionY"> 最も低いブロックのy座標 </param>
//     /// <param name="_useHold"> ホールドの使用判定 </param>
//     /// <param name="_firstHold"> ゲーム中で最初のホールドの使用判定 </param>
//     /// <returns> 更新された <see cref="GameManagerStats"/> の新しいインスタンス </returns>
//     /// <remarks>
//     /// 指定されていない引数は現在の値を維持
//     /// </remarks>
//     public GameManagerStats Update(bool? _gameOver = null, int? _minoPopNumber = null, int? _minoPutNumber = null, int? _bottomMoveCount = null,
//         int? _bottomMoveCountLimit = null, int? _lowestBlockPositionY = null, bool? _useHold = null, bool? _firstHold = null)
//     {
//         var updatedStats = new GameManagerStats(
//             _gameOver ?? gameOver,
//             _minoPopNumber ?? minoPopNumber,
//             _minoPutNumber ?? minoPutNumber,
//             _bottomMoveCount ?? bottomMoveCount,
//             _bottomMoveCountLimit ?? bottomMoveCountLimit,
//             _lowestBlockPositionY ?? lowestBlockPositionY,
//             _useHold ?? useHold,
//             _firstHold ?? firstHold
//         );
//         return updatedStats;
//     }
// }


// /// <summary> キーの入力を検知してブロックを動かす関数 </summary>
// void PlayerInput()
// {
//     // 右移動入力
//     if (Input.GetKeyDown(KeyCode.D)) // Dキーに割り当て
//     {
//         LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "MoveRightInput()", "Start");
//         MoveRightInput();
//         LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "MoveRightInput()", "End");
//     }
//     // 連続右移動入力
//     else if (Input.GetKey(KeyCode.D) && (Time.time > Timer.NextKeyLeftRightTimer)) // Dキーが長押しされている時
//     {
//         LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "ContinuousMoveRightInput()", "Start");
//         ContinuousMoveRightInput();
//         LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "ContinuousMoveRightInput()", "End");
//     }
//     // 連続右移動入力の解除
//     else if (Input.GetKeyUp(KeyCode.D)) // Dキーを離した時
//     {
//         LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "ReleaseContinuousMoveRightLeftInput()", "Start");
//         ReleaseContinuousMoveRightLeftInput();
//         LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "ReleaseContinuousMoveRightLeftInput()", "End");
//     }
//     // 左移動入力
//     else if (Input.GetKeyDown(KeyCode.A)) // Aキーに割り当て
//     {
//         LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "MoveLeftInput()", "Start");
//         MoveLeftInput();
//         LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "MoveLeftInput()", "End");
//     }
//     // 連続左移動入力
//     else if (Input.GetKey(KeyCode.A) && (Time.time > Timer.NextKeyLeftRightTimer)) // Aキーが長押しされている時
//     {
//         LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "ContinuousMoveLeftInput()", "Start");
//         ContinuousMoveLeftInput();
//         LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "ContinuousMoveLeftInput()", "End");
//     }
//     // 連続左移動入力の解除
//     else if (Input.GetKeyUp(KeyCode.A)) // Aキーを離した時
//     {
//         LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "ReleaseContinuousMoveRightLeftInput()", "Start");
//         ReleaseContinuousMoveRightLeftInput();
//         LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "ReleaseContinuousMoveRightLeftInput()", "End");
//     }
//     // 下移動入力
//     else if (Input.GetKey(KeyCode.S) && (Time.time > Timer.NextKeyDownTimer)) // Sキーに割り当て
//     {
//         LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "MoveDownInput()", "Start");
//         MoveDownInput();
//         LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "MoveDownInput()", "End");
//     }
//     // 右回転入力
//     else if (Input.GetKeyDown(KeyCode.P) && (Time.time > Timer.NextKeyRotateTimer)) // Pキーに割り当て
//     {
//         LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "RotateRightInput()", "Start");
//         RotateRightInput();
//         LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "RotateRightInput()", "End");
//     }
//     // 左回転入力
//     else if (Input.GetKeyDown(KeyCode.L) && (Time.time > Timer.NextKeyRotateTimer)) // Lキーに割り当て
//     {
//         LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "RotateLeftInput()", "Start");
//         RotateLeftInput();
//         LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "RotateLeftInput()", "End");
//     }
//     // ハードドロップ入力
//     else if (Input.GetKeyDown(KeyCode.Space)) // Spaceキーに割り当て
//     {
//         LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "HardDropInput()", "Start");
//         HardDropInput();
//         LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "HardDropInput()", "End");
//     }
//     // ホールド入力
//     else if (Input.GetKeyDown(KeyCode.Return)) // Enter(Return)キーに割り当て
//     {
//         LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "HoldInput()", "Start");
//         HoldInput();
//         LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "HoldInput()", "End");
//     }
// }

// /// <summary> 右移動入力時の処理を行う関数 </summary>
// private void MoveRightInput()
// {
//     Timer.ContinuousLRKey = false; // キーの連続入力でない判定を付与

//     Timer.UpdateLeftRightTimer();

//     spawner.ActiveMino.MoveRight();

//     if (!board.CheckPosition(spawner.ActiveMino)) // 右に動かせない時
//     {
//         // DebugHelper.Log("Move right failed: Cannot move to the right - Reverting to original position", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

//         spawner.ActiveMino.MoveLeft(); // 左に動かす(元に戻すため)
//     }
//     else // 動かせた時
//     {
//         // DebugHelper.Log("Move right succeeded: Mino successfully moved to the right", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

//         AudioManager.Instance.PlaySound(AudioNames.MoveLeftRight);

//         // 移動したため、以下の処理を実行
//         spawner.AdjustGhostMinoPosition();
//         spinCheck.ResetSpinTypeName();
//         minoMovement.ResetStepsSRS();
//     }

//     IncreaseBottomMoveCount(); // BottomMoveCountの値を1増加
// }

// /// <summary> 連続右移動入力時の処理を行う関数 </summary>
// private void ContinuousMoveRightInput()
// {
//     Timer.ContinuousLRKey = true; // キーの連続入力判定を付与

//     Timer.UpdateLeftRightTimer();

//     spawner.ActiveMino.MoveRight(); // 右に動かす

//     if (!board.CheckPosition(spawner.ActiveMino)) // 右に動かせない時
//     {
//         // DebugHelper.Log("Continuous move right failed: Cannot move to the right - Reverting to original position", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

//         spawner.ActiveMino.MoveLeft(); // 左に動かす(元に戻すため)
//     }
//     else // 動かせた時
//     {
//         // DebugHelper.Log("Continuous move right succeeded: Mino successfully moved to the right", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

//         AudioManager.Instance.PlaySound(AudioNames.MoveLeftRight);

//         // 移動したため、以下の処理を実行
//         spawner.AdjustGhostMinoPosition();
//         spinCheck.ResetSpinTypeName();
//         minoMovement.ResetStepsSRS();
//     }

//     IncreaseBottomMoveCount(); // BottomMoveCountの値を1増加
// }

// /// <summary> 左移動入力時の処理を行う関数 </summary>
// private void MoveLeftInput()
// {
//     Timer.ContinuousLRKey = false; // キーの連続入力でない

//     Timer.UpdateLeftRightTimer();

//     spawner.ActiveMino.MoveLeft();

//     if (!board.CheckPosition(spawner.ActiveMino)) // 左に動かせない時
//     {
//         // DebugHelper.Log("Move left failed: Cannot move to the left - Reverting to original position", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

//         spawner.ActiveMino.MoveRight(); // 右に動かす(元に戻すため)
//     }
//     else // 動かせた時
//     {
//         // DebugHelper.Log("Move left succeeded: Mino successfully moved to the left", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

//         AudioManager.Instance.PlaySound(AudioNames.MoveLeftRight);

//         // 移動したため、以下の処理を実行
//         spawner.AdjustGhostMinoPosition();
//         spinCheck.ResetSpinTypeName();
//         minoMovement.ResetStepsSRS();
//     }

//     IncreaseBottomMoveCount(); // BottomMoveCountの値を1増加
// }

// /// <summary> 連続左移動入力時の処理を行う関数 </summary>
// private void ContinuousMoveLeftInput()
// {
//     Timer.ContinuousLRKey = true; // キーの連続入力がされた

//     Timer.UpdateLeftRightTimer();

//     spawner.ActiveMino.MoveLeft();

//     if (!board.CheckPosition(spawner.ActiveMino)) // 左に動かせない時
//     {
//         // DebugHelper.Log("Continuous move left failed: Cannot move to the left - Reverting to original position", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

//         spawner.ActiveMino.MoveRight(); // 右に動かす(元に戻すため)
//     }
//     else // 動かせた時
//     {
//         // DebugHelper.Log("Continuous move left succeeded: Mino successfully moved to the left", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

//         AudioManager.Instance.PlaySound(AudioNames.MoveLeftRight);

//         // 移動したため、以下の処理を実行
//         spawner.AdjustGhostMinoPosition();
//         spinCheck.ResetSpinTypeName();
//         minoMovement.ResetStepsSRS();
//     }

//     IncreaseBottomMoveCount(); // BottomMoveCountの値を1増加
// }

// /// <summary> 連続右、または左移動入力の解除処理を行う関数 </summary>
// private void ReleaseContinuousMoveRightLeftInput()
// {
//     Timer.ContinuousLRKey = false;
// }

// /// <summary> 下移動入力時の処理を行う関数 </summary>
// private void MoveDownInput()
// {
//     Timer.UpdateDownTimer();

//     spawner.ActiveMino.MoveDown();

//     if (!board.CheckPosition(spawner.ActiveMino)) // 下に動かせない時
//     {
//         // DebugHelper.Log("Move down failed: Cannot move down - Reverting to original position", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

//         spawner.ActiveMino.MoveUp(); // 上に動かす(元に戻すため)
//     }
//     else // 動かせた時
//     {
//         // DebugHelper.Log("Move down succeeded: Mino successfully moved down", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

//         AudioManager.Instance.PlaySound(AudioNames.MoveDown);

//         if (spinCheck.SpinTypeName != SpinTypeNames.I_Spin) // I-Spinは下移動しても解除されないようにしている
//         {
//             spinCheck.ResetSpinTypeName(); // 移動したため、スピン判定をリセット
//         }

//         minoMovement.ResetStepsSRS(); // 移動したため、StepsSRSの値を0に
//     }
// }

// /// <summary> 右回転入力時の処理を行う関数 </summary>
// private void RotateRightInput()
// {
//     Timer.UpdateRotateTimer();

//     minoMovement.ResetStepsSRS();

//     spawner.ActiveMino.RotateRight();

//     if (!board.CheckPosition(spawner.ActiveMino)) // 通常回転ができなかった時
//     {
//         // DebugHelper.Log("Normal rotation failed: Trying Super Rotation System (SRS)", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

//         if (!minoMovement.SuperRotationSystem()) // SRSもできなかった時
//         {
//             // DebugHelper.Log("Super Rotation System (SRS) failed: Reverting rotation", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

//             minoMovement.UpdateMinoAngleAfterToMinoAngleBefore();

//             AudioManager.Instance.PlaySound(AudioNames.Rotation);
//         }
//         else // SRSが成功した時
//         {
//             // DebugHelper.Log("Super Rotation System (SRS) succeeded", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

//             SuccessRotateAction(); // 回転が成功した時の処理を実行
//         }
//     }
//     else // 通常回転が成功した時
//     {
//         // DebugHelper.Log("Normal rotation succeeded", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

//         SuccessRotateAction();
//     }

//     IncreaseBottomMoveCount();
// }

// /// <summary> 左回転入力時の処理を行う関数 </summary>
// private void RotateLeftInput()
// {
//     Timer.UpdateRotateTimer();

//     minoMovement.ResetStepsSRS();

//     spawner.ActiveMino.RotateLeft();

//     if (!board.CheckPosition(spawner.ActiveMino)) // 通常回転ができなかった時
//     {
//         // DebugHelper.Log("Normal rotation failed: Trying Super Rotation System (SRS)", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

//         if (!minoMovement.SuperRotationSystem()) // SRSもできなかった時
//         {
//             // DebugHelper.Log("Super Rotation System (SRS) failed: Reverting rotation", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

//             minoMovement.UpdateMinoAngleAfterToMinoAngleBefore();

//             AudioManager.Instance.PlaySound(AudioNames.Rotation);
//         }
//         else // SRSが成功した時
//         {
//             // DebugHelper.Log("Super Rotation System (SRS) succeeded", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

//             SuccessRotateAction(); // 回転が成功した時の処理を実行
//         }
//     }
//     else // 通常回転が成功した時
//     {
//         // DebugHelper.Log("Normal rotation succeeded", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

//         SuccessRotateAction(); // 回転が成功した時の処理を実行
//     }

//     IncreaseBottomMoveCount(); // BottomMoveCountの値を1増加
// }

// /// <summary> 回転が成功した時の処理をする関数 </summary>
// private void SuccessRotateAction()
// {
//     LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "SuccessRotateAction()", "Start");

//     /// <summary> ミノの回転後の向き </summary>
//     MinoDirections minoAngleAfter;
//     // /// <summary> ミノの回転前の向き </summary>
//     // MinoDirections minoAngleBefore;
//     /// <summary> スーパーローテーションシステム(SRS)の段階 </summary>
//     int stepsSRS;

//     spawner.AdjustGhostMinoPosition(); // ゴーストミノの位置調整

//     minoMovement.UpdateMinoAngleBeforeToMinoAngleAfter();

//     minoAngleAfter = minoMovement.GetMinoAngleAfter();
//     // minoAngleBefore = mino.GetMinoAngleBefore();
//     stepsSRS = minoMovement.GetStepsSRS();
//     spinCheck.CheckSpinType(minoAngleAfter, stepsSRS);

//     if (spinCheck.SpinTypeName != SpinTypeNames.None) // スピン判定がない場合
//     {
//         AudioManager.Instance.PlaySound(AudioNames.Spin);
//     }
//     else // スピン判定がある場合
//     {
//         AudioManager.Instance.PlaySound(AudioNames.Rotation);
//     }

//     LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "SuccessRotateAction()", "End");
// }

// /// <summary> BottomMoveCountを進める関数 </summary>
// private void IncreaseBottomMoveCount()
// {
//     LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "IncreaseBottomMoveCount()", "Start");
//     GameManagerStats.Update(_bottomMoveCount: GameManagerStats.BottomMoveCount + 1);
//     LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "IncreaseBottomMoveCount()", "End");
// }

// /// <summary> ハードドロップ入力時の処理を行う関数 </summary>
// private void HardDropInput()
// {
//     AudioManager.Instance.PlaySound(AudioNames.HardDrop);

//     // Heightの値分繰り返す(20)
//     for (int i = 0; i < board.Height; i++)
//     {
//         spawner.ActiveMino.MoveDown();

//         if (!board.CheckPosition(spawner.ActiveMino)) // 底にぶつかった時
//         {
//             // DebugHelper.Log("Hard drop: Mino hit the bottom, reverting to last valid position", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

//             spawner.ActiveMino.MoveUp(); // ミノを正常な位置に戻す

//             break;
//         }

//         // 以下一マスでも下に移動した時の処理
//         if (spinCheck.SpinTypeName != SpinTypeNames.I_Spin) // I-Spinは下移動しても解除されないようにしている
//         {
//             spinCheck.ResetSpinTypeName();
//         }
//         minoMovement.ResetStepsSRS();
//     }

//     // DebugHelper.Log("Hard drop: Mino reached the bottom", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

//     ResetRockDown();

//     SetMinoFixed();
// }

// /// <summary> ホールド入力時の処理を行う関数 </summary>
// private void HoldInput()
// {
//     // Holdは1度使うと、ミノを設置するまで使えない
//     if (GameManagerStats.UseHold == false)
//     {
//         // DebugHelper.Log("Hold action initiated", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

//         GameManagerStats.Update(_useHold: true);

//         AudioManager.Instance.PlaySound(AudioNames.Hold);

//         ResetRockDown();

//         if (GameManagerStats.FirstHold == true) // ゲーム中で最初のHoldだった時
//         {
//             // DebugHelper.Log("First hold action", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

//             GameManagerStats.Update(_minoPopNumber: GameManagerStats.MinoPopNumber + 1);

//             spawner.CreateHoldMino(GameManagerStats.FirstHold, GameManagerStats.MinoPopNumber);

//             GameManagerStats.Update(_firstHold: false);
//         }
//         else
//         {
//             // DebugHelper.Log("Subsequent hold action", DebugHelper.LogLevel.Debug, "GameManager", "PlayerInput()");

//             spawner.CreateHoldMino(GameManagerStats.FirstHold, GameManagerStats.MinoPopNumber);
//         }
//     }
//     else
//     {
//         // DebugHelper.Log("Hold action ignored: Hold already used", DebugHelper.LogLevel.Warning, "GameManager", "PlayerInput()");
//     }
// }

// /// <summary> 時間経過で落ちる時の処理をする関数 </summary>
//     void AutoDown()
//     {
//         Timer.UpdateDownTimer();

//         spawner.ActiveMino.MoveDown();

//         if (!board.CheckPosition(spawner.ActiveMino))
//         {
//             spawner.ActiveMino.MoveUp(); // ミノを正常な位置に戻す
//         }
//         else
//         {
//             if (spinCheck.SpinTypeName != SpinTypeNames.I_Spin) // I-Spinは下移動しても解除されないようにしている
//             {
//                 spinCheck.ResetSpinTypeName(); // 移動したため、スピン判定をリセット
//             }

//             minoMovement.ResetStepsSRS();
//         }
//     }

//     /// <summary> ロックダウンの処理をする関数 </summary>
//     private void RockDown()
//     {
//         LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "RockDown()", "Start");

//         /// <summary> ロックダウンの移動回数制限 </summary>
//         int bottomMoveCountLimit = 15;

//         int newBottomBlockPosition_y = board.CheckActiveMinoBottomBlockPosition_y(spawner.ActiveMino); // 操作中のミノの1番下のブロックのy座標を取得

//         if (GameManagerStats.LowestBlockPositionY <= newBottomBlockPosition_y) // lowestBlockPositionYが更新されていない場合
//         {
//             spawner.ActiveMino.MoveDown();

//             // 1マス下が底の時((底に面している時)
//             // かつインターバル時間を超過している、または15回以上移動や回転を行った時
//             if (!board.CheckPosition(spawner.ActiveMino) && (Time.time >= Timer.BottomTimer ||
//                 GameManagerStats.BottomMoveCount >= bottomMoveCountLimit))
//             {
//                 spawner.ActiveMino.MoveUp(); // 元の位置に戻す

//                 // AudioManager.Instance.PlaySound(AudioNames.NormalDrop_Audio);

//                 SetMinoFixed(); // ミノの設置判定
//             }
//             else
//             {
//                 spawner.ActiveMino.MoveUp(); // 元の位置に戻す
//             }
//         }
//         else // lowestBlockPositionYが更新された場合
//         {
//             GameManagerStats.Update(_bottomMoveCount: 0);

//             GameManagerStats.Update(_lowestBlockPositionY: newBottomBlockPosition_y); // BottomPositionの更新
//         }

//         LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "RockDown()", "End");
//     }

//     /// <summary> RockDownに関する変数のリセット </summary>
//     public void ResetRockDown()
//     {
//         LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "ResetRockDown()", "Start");
//         GameManagerStats.Update(_bottomMoveCount: 0, _lowestBlockPositionY: 20);
//         LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "ResetRockDown()", "End");
//     }

//     /// <summary> ミノの設置場所が確定した時の処理をする関数 </summary>
//     void SetMinoFixed()
//     {
//         LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "SetMinoFixed()", "Start");

//         /// <summary> 合計の消去ライン数 </summary>
//         int lineClearCount;

//         if (board.CheckGameOver(spawner.ActiveMino)) // ミノの設置時にゲームオーバーの条件を満たした場合
//         {
//             textEffect.StopAnimation();

//             GameManagerStats.Update(_gameOver: true);

//             sceneTransition.GameOver();

//             return;
//         }

//         // 各種変数のリセット
//         ResetRockDown();
//         Timer.Reset();

//         board.SaveBlockInGrid(spawner.ActiveMino);
//         lineClearCount = board.ClearAllRows();
//         board.AddLineClearCountHistory(lineClearCount);
//         attackCalculator.CalculateAttackLines(spinCheck.SpinTypeName, lineClearCount);
//         textEffect.TextDisplay(spinCheck.SpinTypeName, lineClearCount);

//         // 各種変数のリセット
//         spinCheck.ResetSpinTypeName();
//         minoMovement.ResetAngle();
//         minoMovement.ResetStepsSRS();

//         GameManagerStats.Update(_minoPutNumber: GameManagerStats.MinoPutNumber + 1,
//             _minoPopNumber: GameManagerStats.MinoPopNumber + 1, _useHold: false);

//         spawner.CreateNewActiveMino(GameManagerStats.MinoPopNumber);

//         spawner.CreateNextMinos(GameManagerStats.MinoPopNumber);

//         if (!board.CheckPosition(spawner.ActiveMino)) // ミノを生成した際に、ブロックと重なってしまった場合
//         {
//             textEffect.StopAnimation();

//             GameManagerStats.Update(_gameOver: true);

//             sceneTransition.GameOver();

//             return;
//         }

//         LogHelper.Log(LogHelper.LogLevel.Debug, "GameManager", "SetMinoFixed()", "End");
//     }

// /// <summary>
// /// ゲームの状態を管理するクラス
// /// </summary>
// public static class GameStateManager
// {
//     /// <summary> ゲームオーバーの状態 </summary>
//     private static bool gameOver = false;
//     /// <summary> ゲームクリアの状態 </summary>
//     private static bool gameClear = false;
//     /// <summary> スコア画面の状態 </summary>
//     private static bool score = false;
//     /// <summary> オプション画面の状態 </summary>
//     private static bool option = false;
//     /// <summary> メニュー画面の状態 </summary>
//     private static bool menu = true;

//     // ゲッタープロパティ //
//     public static bool GameOver => gameOver;
//     public static bool GameClear => gameClear;
//     public static bool Score => score;
//     public static bool Option => option;
//     public static bool Menu => menu;

//     /// <summary> 指定されたフィールドの値を更新する関数 </summary>
//     /// <param name="_gameOver"> ゲームオーバーの状態 </param>
//     /// <param name="_gameClear"> ゲームクリアの状態 </param>
//     /// <param name="_score"> スコア画面の状態 </param>
//     /// <param name="_option"> オプション画面の状態 </param>
//     /// <param name="_menu"> メニュー画面の状態 </param>
//     /// <remarks>
//     /// 指定された状態を true に設定し、他のすべての状態を false に設定する。
//     /// </remarks>
//     public static void UpdateState(bool? _gameOver = null, bool? _gameClear = null, bool? _score = null, bool? _option = null, bool? _menu = null)
//     {
//         // 最初にすべての状態を false に設定
//         gameOver = false;
//         gameClear = false;
//         score = false;
//         option = false;
//         menu = false;

//         // 引数で true が指定された状態のみを true に設定
//         if (_gameOver == true)
//         {
//             gameOver = true;
//         }
//         else if (_gameClear == true)
//         {
//             gameClear = true;
//         }
//         else if (_score == true)
//         {
//             score = true;
//         }
//         else if (_option == true)
//         {
//             option = true;
//         }
//         else if (_menu == true)
//         {
//             menu = true;
//         }
//     }

//     /// <summary> デフォルトの <see cref="GameStateManager"/> にリセットする関数 </summary>
//     public static void ResetStates()
//     {
//         gameOver = false;
//         gameClear = false;
//         score = false;
//         option = false;
//         menu = true;
//     }
// }

/////////////////////////////////////////////////////////