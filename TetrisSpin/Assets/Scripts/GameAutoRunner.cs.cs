using UnityEngine;

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
        LogHelper.Log(LogHelper.LogLevel.Debug, "GameAutoRunner", "RockDown()", "Start");

        int bottomMoveCountLimit = 15;
        int newBottomBlockPositionY = board.CheckActiveMinoBottomBlockPositionY(spawner.ActiveMino);

        if (GameManagerStats.LowestBlockPositionY <= newBottomBlockPositionY) // lowestBlockPositionYが更新されていない場合
        {
            spawner.ActiveMino.MoveDown();

            if (!board.CheckPosition(spawner.ActiveMino) && (Time.time >= Timer.BottomTimer || GameManagerStats.BottomMoveCount >= bottomMoveCountLimit))
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
            GameManagerStats.Update(_bottomMoveCount: 0);
            GameManagerStats.Update(_lowestBlockPositionY: newBottomBlockPositionY); // BottomPositionの更新
        }

        LogHelper.Log(LogHelper.LogLevel.Debug, "GameAutoRunner", "RockDown()", "End");
    }

    /// <summary> RockDownに関する変数のリセット </summary>
    public void ResetRockDown()
    {
        LogHelper.Log(LogHelper.LogLevel.Debug, "GameAutoRunner", "ResetRockDown()", "Start");
        GameManagerStats.Update(_bottomMoveCount: 0, _lowestBlockPositionY: 20);
        LogHelper.Log(LogHelper.LogLevel.Debug, "GameAutoRunner", "ResetRockDown()", "End");
    }

    /// <summary> 自動落下の処理をする関数 </summary>
    public void AutoDown()
    {
        LogHelper.Log(LogHelper.LogLevel.Debug, "GameAutoRunner", "AutoDown()", "Start");

        Timer.UpdateDownTimer();
        spawner.ActiveMino.MoveDown();

        if (!board.CheckPosition(spawner.ActiveMino))
        {
            spawner.ActiveMino.MoveUp();
        }
        else
        {
            if (spinCheck.SpinTypeName != SpinTypeNames.I_Spin)
            {
                spinCheck.ResetSpinTypeName(); // 移動したため、スピン判定をリセット
            }
            minoMovement.ResetStepsSRS();
        }

        LogHelper.Log(LogHelper.LogLevel.Debug, "GameAutoRunner", "AutoDown()", "End");
    }

    /// <summary> ミノの設置場所が確定した時の処理をする関数 </summary>
    public void SetMinoFixed()
    {
        LogHelper.Log(LogHelper.LogLevel.Debug, "GameAutoRunner", "SetMinoFixed()", "Start");

        /// <summary> 合計の消去ライン数 </summary>
        int lineClearCount;

        if (board.CheckGameOver(spawner.ActiveMino)) // ミノの設置時にゲームオーバーの条件を満たした場合
        {
            textEffect.StopAnimation();

            GameManagerStats.Update(_gameOver: true);

            sceneTransition.GameOver();

            return;
        }

        // 各種変数のリセット
        ResetRockDown();
        Timer.Reset();

        board.SaveBlockInGrid(spawner.ActiveMino);
        lineClearCount = board.ClearAllRows();
        board.AddLineClearCountHistory(lineClearCount);
        attackCalculator.CalculateAttackLines(spinCheck.SpinTypeName, lineClearCount);
        textEffect.TextDisplay(spinCheck.SpinTypeName, lineClearCount);

        // 各種変数のリセット
        spinCheck.ResetSpinTypeName();
        minoMovement.ResetAngle();
        minoMovement.ResetStepsSRS();

        GameManagerStats.Update(_minoPutNumber: GameManagerStats.MinoPutNumber + 1,
            _minoPopNumber: GameManagerStats.MinoPopNumber + 1, _useHold: false);

        spawner.CreateNewActiveMino(GameManagerStats.MinoPopNumber);

        spawner.CreateNextMinos(GameManagerStats.MinoPopNumber);

        if (!board.CheckPosition(spawner.ActiveMino)) // ミノを生成した際に、ブロックと重なってしまった場合
        {
            textEffect.StopAnimation();

            GameManagerStats.Update(_gameOver: true);

            sceneTransition.GameOver();

            return;
        }

        LogHelper.Log(LogHelper.LogLevel.Debug, "GameAutoRunner", "SetMinoFixed()", "End");
    }
}
