using UnityEngine;

/// <summary>
/// GameAutoRunnerの統計情報を保持する静的クラス
/// </summary>
public static class GameAutoRunnerStats
{
    /// <summary> ロックダウンの移動回数 </summary>
    /// <remarks>
    /// ミノがボトムに到達してからの移動回数を表す
    /// </remarks>
    /// <value> 0~15 </value>
    private static int bottomMoveCount = 0;
    /// <summary> 操作中のミノを構成するブロックのうち、最も低い y 座標を保持する変数 </summary>
    /// <remarks>
    /// ロックダウンの処理に必要 <br/>
    /// この値が更新されるたびにロックダウンの移動回数制限がリセットされる <br/>
    /// 初期値はゲームボードの最高高さである 20 に設定
    /// </remarks>
    /// <value> 0~20 </value>
    private static int lowestBlockPositionY = 20;

    // ゲッタープロパティ //
    public static int BottomMoveCount => bottomMoveCount;
    public static int LowestBlockPositionY => lowestBlockPositionY;

    /// <summary> 指定されたフィールドの値を更新する関数 </summary>
    /// <param name="_bottomMoveCount"> ロックダウンの移動回数 </param>
    /// <param name="_lowestBlockPositionY"> 操作中のミノを構成するブロックのうち、最も低い y 座標を保持する変数 </param>
    /// <remarks>
    /// 指定されていない引数は現在の値を維持
    /// </remarks>
    public static void UpdateStats(int? _bottomMoveCount = null, int? _lowestBlockPositionY = null)
    {
        bottomMoveCount = _bottomMoveCount ?? bottomMoveCount;
        lowestBlockPositionY = _lowestBlockPositionY ?? lowestBlockPositionY;
        // TODO: ログの記入
    }

    /// <summary> デフォルトの <see cref="AttackCalculatorStats"/> にリセットする関数 </summary>
    public static void ResetStats()
    {
        bottomMoveCount = 0;
        lowestBlockPositionY = 20;
    }
}

/// <summary>
/// ゲーム中の自動処理を行うクラス
/// </summary>
public class GameAutoRunner : MonoBehaviour
{
    AttackCalculator attackCalculator;
    Board board;
    MinoMovement minoMovement;
    SceneTransition sceneTransition;
    Spawner spawner;
    SpinCheck spinCheck;
    TextEffect textEffect;

    private void Awake()
    {
        attackCalculator = FindObjectOfType<AttackCalculator>();
        board = FindObjectOfType<Board>();
        minoMovement = FindObjectOfType<MinoMovement>();
        sceneTransition = FindObjectOfType<SceneTransition>();
        spawner = FindObjectOfType<Spawner>();
        spinCheck = FindObjectOfType<SpinCheck>();
        textEffect = FindObjectOfType<TextEffect>();
    }

    /// <summary> ロックダウンの処理をする関数 </summary>
    public void RockDown()
    {
        if (Application.isEditor) LogHelper.DebugLog(eClasses.GameAutoRunner, eMethod.RockDown, eLogTitle.Start);

        int bottomMoveCountLimit = 15;
        int newBottomBlockPositionY = board.CheckActiveMinoBottomBlockPositionY(spawner.ActiveMino);

        if (GameAutoRunnerStats.LowestBlockPositionY <= newBottomBlockPositionY) // lowestBlockPositionYが更新されていない場合
        {
            spawner.ActiveMino.MoveDown();

            if (!board.CheckPosition(spawner.ActiveMino) && (Time.time >= Timer.BottomTimer || GameAutoRunnerStats.BottomMoveCount >= bottomMoveCountLimit))
            {
                spawner.ActiveMino.MoveUp(); // 元の位置に戻す
                SetMinoFixed(); // ミノの設置判定
            }
            else
            {
                spawner.ActiveMino.MoveUp(); // 元の位置に戻す
            }
        }
        else // lowestBlockPositionYが更新された場合
        {
            GameAutoRunnerStats.UpdateStats(_bottomMoveCount: 0);
            GameAutoRunnerStats.UpdateStats(_lowestBlockPositionY: newBottomBlockPositionY); // BottomPositionの更新
        }

        if (Application.isEditor) LogHelper.DebugLog(eClasses.GameAutoRunner, eMethod.RockDown, eLogTitle.End);
    }

    /// <summary> RockDownに関する変数のリセット </summary>
    public void ResetRockDown()
    {
        if (Application.isEditor) LogHelper.DebugLog(eClasses.GameAutoRunner, eMethod.ResetRockDown, eLogTitle.Start);
        GameAutoRunnerStats.UpdateStats(_bottomMoveCount: 0, _lowestBlockPositionY: 20);
        if (Application.isEditor) LogHelper.DebugLog(eClasses.GameAutoRunner, eMethod.ResetRockDown, eLogTitle.End);
    }

    /// <summary> 自動落下の処理をする関数 </summary>
    public void AutoDown()
    {
        if (Application.isEditor) LogHelper.DebugLog(eClasses.GameAutoRunner, eMethod.AutoDown, eLogTitle.Start);

        Timer.UpdateMoveDownTimer();
        spawner.ActiveMino.MoveDown();

        if (!board.CheckPosition(spawner.ActiveMino))
        {
            spawner.ActiveMino.MoveUp();
        }
        else
        {
            if (spinCheck.SpinTypeName != SpinTypeNames.Ispin)
            {
                spinCheck.ResetSpinTypeName(); // 移動したため、スピン判定をリセット
            }
            minoMovement.ResetStepsSRS();
        }

        if (Application.isEditor) LogHelper.DebugLog(eClasses.GameAutoRunner, eMethod.AutoDown, eLogTitle.End);
    }

    /// <summary> ミノの設置場所が確定した時の処理をする関数 </summary>
    public void SetMinoFixed()
    {
        if (Application.isEditor) LogHelper.DebugLog(eClasses.GameAutoRunner, eMethod.SetMinoFixed, eLogTitle.Start);

        /// <summary> 合計の消去ライン数 </summary>
        int lineClearCount;

        if (board.CheckGameOver(spawner.ActiveMino)) // ミノの設置時にゲームオーバーの条件を満たした場合
        {
            textEffect.StopAnimation();

            GameManagerStats.UpdateStats(_gameOver: true);

            sceneTransition.GameOver();

            return;
        }

        // 各種変数のリセット
        ResetRockDown();
        Timer.ResetTimer();

        board.SaveBlockInGrid(spawner.ActiveMino);
        lineClearCount = board.ClearAllRows();
        board.AddLineClearCountHistory(lineClearCount);
        attackCalculator.CalculateAttackLines(spinCheck.SpinTypeName, lineClearCount);
        textEffect.TextDisplay(spinCheck.SpinTypeName, lineClearCount);

        // 各種変数のリセット
        spinCheck.ResetSpinTypeName();
        minoMovement.ResetAngle();
        minoMovement.ResetStepsSRS();

        GameManagerStats.UpdateStats(_minoPutNumber: GameManagerStats.MinoPutNumber + 1,
            _minoPopNumber: GameManagerStats.MinoPopNumber + 1);
        PlayerInputStats.UpdateStats(_useHold: false);

        spawner.CreateNewActiveMino(GameManagerStats.MinoPopNumber);

        spawner.CreateNextMinos(GameManagerStats.MinoPopNumber);

        if (!board.CheckPosition(spawner.ActiveMino)) // ミノを生成した際に、ブロックと重なってしまった場合
        {
            textEffect.StopAnimation();

            GameManagerStats.UpdateStats(_gameOver: true);

            sceneTransition.GameOver();

            return;
        }

        if (Application.isEditor) LogHelper.DebugLog(eClasses.GameAutoRunner, eMethod.SetMinoFixed, eLogTitle.End);
    }
}
