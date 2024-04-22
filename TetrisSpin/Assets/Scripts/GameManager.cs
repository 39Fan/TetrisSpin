using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// ゲームマネージャー
public class GameManager : MonoBehaviour
{
    // 各種干渉するスクリプトの設定
    Board board;
    Calculate calculate;
    MainSceneText mainSceneText;
    Rotation rotation;
    SceneTransition sceneTransition;
    SE se;
    Spawner spawner;

    GameStatus gameStatus;
    Timer timer;
    //TetrisSpinData tetrisSpinData;

    //public Mino gameStatus.ActiveMino; // 操作中のミノ
    //Mino_Ghost gameStatus.GhostMino; // ゴーストミノ
    //public Mino HoldMino; // ホールドミノ

    //private Minos gameStatus.ActiveMino;
    //private Minos gameStatus.GhostMino;
    //private Minos hold_mino;

    // private string[] gameStatus.ActiveMino_angle = new string[]
    // {
    //     "north", "east", "south", "west"
    // };

    // タイマー
    // 次にブロックが落ちるまでのインターバル時間
    // 次にブロックが落ちるまでの時間
    // 入力受付タイマー(3種類)
    // [SerializeField] float dropInteaval;
    // [SerializeField] float nextdropTimer;
    // [SerializeField] float nextKeyDownTimer, nextKeyLeftRightTimer, nextKeyRotateTimer;

    // [SerializeField] float keyReceptionTimer;

    // // 入力インターバル(4種類)
    // [SerializeField] private float nextKeyDownInterval, nextKeyLeftRightInterval, nextKeyRotateInterval, keyReceptionInterval;

    // // ゲームオーバー判定
    // bool gameover;

    // int SpinActions = 7;

    // // 底についた判定
    // bool Bottom = false;

    // bool CanNotMove = false;

    // // ライン消去の履歴を表すリストと
    // // それに対応するカウントの変数
    // // カウントの変数は、ミノの設置で1ずつ増加する
    // [SerializeField]
    // List<int> line_elimination_count_history = new List<int>();
    // int mino_installation_number = 0;

    // インスタンス化
    private void Awake()
    {
        spawner = FindObjectOfType<Spawner>();
        board = FindObjectOfType<Board>();
        calculate = FindObjectOfType<Calculate>();
        mainSceneText = FindObjectOfType<MainSceneText>();
        rotation = FindObjectOfType<Rotation>();
        se = FindObjectOfType<SE>();
        sceneTransition = FindObjectOfType<SceneTransition>();

        gameStatus = FindObjectOfType<GameStatus>();
        timer = FindObjectOfType<Timer>();
        //tetrisSpinData = FindObjectOfType<TetrisSpinData>();
    }

    private void Start()
    {
        // タイマーの初期設定
        timer.ResetTimer();

        if (!gameStatus.ActiveMino)
        {
            // 2回繰り返す
            int length = 2;

            for (int i = 0; i < length; i++)
            {
                // ゲーム開始時、0から13番目のミノの順番を決める
                calculate.DetermineSpawnMinoOrder();
            }

            // 新しいgameStatus.ActiveMinoの生成
            gameStatus.ActiveMino = spawner.SpawnMino(gameStatus.SpawnMinoOrder_List[gameStatus.MinoPopNumber]);

            // gameStatus.ActiveMinoのゴーストミノの生成
            gameStatus.GhostMino = spawner.SpawnMino_Ghost();

            // Nextの表示
            spawner.SpawnNextMinos();
        }
    }

    private void Update()
    {
        if (gameStatus.GameOver)
        {
            return;
        }

        if (Time.time > timer.keyReceptionTimer && gameStatus.Bottom == true)
        {
            gameStatus.ActiveMino.MoveDown();
            BottomBoard();
        }

        PlayerInput();

        AutoDown();
    }

    // キーの入力を検知してブロックを動かす関数
    void PlayerInput()
    {
        // 右入力された時
        // Dキーに割り当て
        if (Input.GetKeyDown(KeyCode.D) && gameStatus.CanNotMove == false)
        {
            gameStatus.ActiveMino.MoveRight();

            timer.ContinuousLRKey = false;

            timer.UpdateLeftRightTimer();

            if (!board.CheckPosition(gameStatus.ActiveMino))
            {
                gameStatus.ActiveMino.MoveLeft();
            }
            else
            {
                se.CallSE(2);

                // ゴーストミノの位置調整を実行
                gameStatus.GhostMino.transform.position = calculate.PositionAdjustmentGhostMino();

                gameStatus.SpinResetFlag();

                gameStatus.SpinActions = 7;
            }

            BottomMove();
        }
        // 連続で右入力がされた時(右入力キーを長押しされている時)
        else if (Input.GetKey(KeyCode.D) && (Time.time > timer.NextKeyLeftRightTimer) && gameStatus.CanNotMove == false)
        {
            timer.ContinuousLRKey = true;

            timer.UpdateLeftRightTimer();

            gameStatus.ActiveMino.MoveRight();

            if (!board.CheckPosition(gameStatus.ActiveMino))
            {
                gameStatus.ActiveMino.MoveLeft();
            }
            else
            {
                se.CallSE(2);

                // ゴーストミノの位置調整を実行
                gameStatus.GhostMino.transform.position = calculate.PositionAdjustmentGhostMino();

                gameStatus.SpinResetFlag();

                gameStatus.SpinActions = 7;
            }

            BottomMove();
        }
        // 連続右入力の解除
        else if (Input.GetKeyUp(KeyCode.D))
        {
            timer.ContinuousLRKey = false;

            if (!board.CheckPosition(gameStatus.ActiveMino))
            {
                gameStatus.ActiveMino.MoveRight();
            }
        }
        // 左入力された時
        // Aキーに割り当て
        else if (Input.GetKeyDown(KeyCode.A) && gameStatus.CanNotMove == false)
        {
            timer.ContinuousLRKey = false;

            timer.UpdateLeftRightTimer();

            gameStatus.ActiveMino.MoveLeft();

            if (!board.CheckPosition(gameStatus.ActiveMino))
            {
                gameStatus.ActiveMino.MoveRight();
            }
            else
            {
                se.CallSE(2);

                // ゴーストミノの位置調整を実行
                gameStatus.GhostMino.transform.position = calculate.PositionAdjustmentGhostMino();

                gameStatus.SpinResetFlag();

                gameStatus.SpinActions = 7;
            }

            BottomMove();
        }
        // 連続で左入力がされた時(左入力キーを長押しされている時)
        else if (Input.GetKey(KeyCode.A) && (Time.time > timer.NextKeyLeftRightTimer) && gameStatus.CanNotMove == false)
        {
            timer.ContinuousLRKey = true;

            timer.UpdateLeftRightTimer();

            gameStatus.ActiveMino.MoveLeft();

            if (!board.CheckPosition(gameStatus.ActiveMino))
            {
                gameStatus.ActiveMino.MoveRight();
            }
            else
            {
                se.CallSE(2);

                // ゴーストミノの位置調整を実行
                gameStatus.GhostMino.transform.position = calculate.PositionAdjustmentGhostMino();

                gameStatus.SpinResetFlag();

                gameStatus.SpinActions = 7;
            }

            BottomMove();
        }
        // 連続右入力の解除
        else if (Input.GetKeyUp(KeyCode.A))
        {
            timer.ContinuousLRKey = false;

            if (!board.CheckPosition(gameStatus.ActiveMino))
            {
                gameStatus.ActiveMino.MoveRight();
            }
        }
        // 下入力された時
        // Sキーに割り当て
        else if (Input.GetKey(KeyCode.S) && (Time.time > timer.NextKeyDownTimer) && gameStatus.CanNotMove == false)
        {
            gameStatus.ActiveMino.MoveDown();

            timer.UpdateDownTimer();

            if (!board.CheckPosition(gameStatus.ActiveMino))
            {
                if (board.OverLimit(gameStatus.ActiveMino))
                {
                    //ゲームオーバー
                    gameStatus.GameOver = true;

                    sceneTransition.GameOver();
                }
                else
                {
                    gameStatus.ActiveMino.MoveUp();

                    gameStatus.Bottom = true;

                    BottomMove();

                    timer.keyReceptionTimer = Time.time + timer.keyReceptionInterval;
                }
            }
            se.CallSE(3);

            gameStatus.SpinResetFlag();

            gameStatus.SpinActions = 7;
        }
        // 右回転入力された時
        // Pキーに割り当て
        else if (Input.GetKeyDown(KeyCode.P) && (Time.time > timer.NextKeyRotateTimer))
        {
            gameStatus.CanNotMove = true;

            gameStatus.SpinSetFlag();

            timer.UpdateRotateTimer();

            gameStatus.ActiveMino.RotateRight(gameStatus.ActiveMino);

            // 回転後の角度(minoAngleAfter)の調整
            calculate.CalibrateMinoAngleAfter();

            if (!board.CheckPosition(gameStatus.ActiveMino))
            {
                if (!rotation.MinoSuperRotation(gameStatus.ActiveMino))
                {
                    Debug.Log("回転禁止");

                    //4 Rotation
                    se.CallSE(4);
                }
                else
                {
                    Debug.Log("スーパーローテーション成功");

                    // ゴーストミノの向きを調整
                    gameStatus.GhostMino.RotateRight(gameStatus.GhostMino);

                    // ゴーストミノの位置を調整
                    gameStatus.GhostMino.transform.position = calculate.PositionAdjustmentGhostMino();

                    gameStatus.MinoAngleBefore = gameStatus.MinoAngleAfter;

                    gameStatus.SpinActions = rotation.SpinTerminal(gameStatus.ActiveMino);

                    if (gameStatus.SpinActions == 4)
                    {
                        //5 Spin
                        se.CallSE(5);
                    }
                    else
                    {
                        //4 Rotation
                        se.CallSE(4);
                    }
                }
            }
            else
            {
                // ゴーストミノの向きを調整
                gameStatus.GhostMino.RotateRight(gameStatus.GhostMino);

                // ゴーストミノの位置を調整
                gameStatus.GhostMino.transform.position = calculate.PositionAdjustmentGhostMino();

                gameStatus.MinoAngleBefore = gameStatus.MinoAngleAfter;

                gameStatus.SpinActions = rotation.SpinTerminal(gameStatus.ActiveMino);

                if (gameStatus.SpinActions == 4)
                {
                    //5 Spin
                    se.CallSE(5);
                }
                else
                {
                    //4 Rotation
                    se.CallSE(4);
                }
            }

            BottomMove();

            gameStatus.CanNotMove = false;
        }
        // 左回転入力された時
        // Lキーに割り当て
        else if (Input.GetKeyDown(KeyCode.L) && (Time.time > timer.NextKeyRotateTimer))
        {
            gameStatus.CanNotMove = true;

            gameStatus.SpinSetFlag();

            timer.UpdateRotateTimer();

            gameStatus.ActiveMino.Rotateleft(gameStatus.ActiveMino);

            // 回転後の角度(minoAngleAfter)の調整
            calculate.CalibrateMinoAngleAfter();

            if (!board.CheckPosition(gameStatus.ActiveMino))
            {
                if (!rotation.MinoSuperRotation(gameStatus.ActiveMino))
                {
                    Debug.Log("回転禁止");

                    //4 Rotation
                    se.CallSE(4);
                }
                else
                {
                    Debug.Log("スーパーローテーション成功");

                    // ゴーストミノの向きを調整
                    gameStatus.GhostMino.Rotateleft(gameStatus.GhostMino);

                    // ゴーストミノの位置を調整
                    gameStatus.GhostMino.transform.position = calculate.PositionAdjustmentGhostMino();

                    gameStatus.MinoAngleBefore = gameStatus.MinoAngleAfter;

                    gameStatus.SpinActions = rotation.SpinTerminal(gameStatus.ActiveMino);

                    if (gameStatus.SpinActions == 4)
                    {
                        //5 Spin
                        se.CallSE(5);
                    }
                    else
                    {
                        //4 Rotation
                        se.CallSE(4);
                    }
                }
            }
            else
            {
                // ゴーストミノの向きを調整
                gameStatus.GhostMino.Rotateleft(gameStatus.GhostMino);

                // ゴーストミノの位置を調整
                gameStatus.GhostMino.transform.position = calculate.PositionAdjustmentGhostMino();

                gameStatus.MinoAngleBefore = gameStatus.MinoAngleAfter;

                gameStatus.SpinActions = rotation.SpinTerminal(gameStatus.ActiveMino);

                if (gameStatus.SpinActions == 4)
                {
                    //5 Spin
                    se.CallSE(5);
                }
                else
                {
                    //4 Rotation
                    se.CallSE(4);
                }
            }

            BottomMove();

            gameStatus.CanNotMove = false;
        }
        // ハードドロップ入力された時
        // Spaceキーに割り当て
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            gameStatus.UseHardDrop = true;

            for (int i = 0; i < 30; i++)
            {
                gameStatus.ActiveMino.MoveDown();

                if (!board.CheckPosition(gameStatus.ActiveMino))
                {
                    break;
                }

                // 1マスでも落ちたらspin判定は消える。
                gameStatus.SpinResetFlag();

                gameStatus.SpinActions = 7;
            }

            if (board.OverLimit(gameStatus.ActiveMino))
            {
                gameStatus.GameOver = true;

                sceneTransition.GameOver();
            }
            else
            {
                // 底に着いたときの処理
                BottomBoard();
            }
        }
        // ホールド入力された時
        // Enterキーに割り当て
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            // Holdは1度使うと、ミノを設置するまで使えない
            // ミノを設置すると、useHold = false になる
            if (gameStatus.UseHold == false)
            {
                // ホールドの使用
                gameStatus.UseHold = true;

                // HoldのSEを鳴らす
                se.CallSE(se.Hold);

                // ゴーストミノの削除
                Destroy(gameStatus.GhostMino.gameObject);

                // ホールドの処理
                // gameStatus.ActiveMinoをホールドに移動して、新しいミノを生成する
                calculate.Hold();

                // gameStatus.ActiveMinoのゴーストミノを生成
                gameStatus.GhostMino = spawner.SpawnMino_Ghost();
            }
        }
    }

    // 時間経過で落ちる時の処理をする関数
    void AutoDown()
    {
        if (Time.time > timer.AutoDropTimer)
        {
            gameStatus.ActiveMino.MoveDown();

            timer.UpdateDownTimer();

            if (!board.CheckPosition(gameStatus.ActiveMino))
            {
                if (board.OverLimit(gameStatus.ActiveMino))
                {
                    //ゲームオーバー
                    gameStatus.GameOver = true;

                    sceneTransition.GameOver();
                }
                else
                {
                    gameStatus.ActiveMino.MoveUp();

                    gameStatus.Bottom = true;

                    BottomMove();

                    timer.keyReceptionTimer = Time.time + timer.keyReceptionInterval;
                }
            }
            gameStatus.SpinResetFlag();

            gameStatus.SpinActions = 7;

            // nextdropTimer = Time.time + dropInteaval;
        }
    }

    //Bottomで移動、回転したとき
    void BottomMove()
    {
        if (timer.keyReceptionTimer > Time.time && gameStatus.Bottom == true)
        {
            timer.keyReceptionInterval = timer.keyReceptionInterval - (timer.keyReceptionTimer - Time.time) / 10;
            timer.keyReceptionTimer = Time.time + timer.keyReceptionInterval;
        }
    }

    void BottomBoard() //底に着いたときの処理
    {
        Debug.Log("BottomBoard");

        //ゴーストミノの削除
        Destroy(gameStatus.GhostMino.gameObject);

        //初期化
        gameStatus.Bottom = false;

        timer.ResetTimer();

        timer.keyReceptionInterval = 1f;

        gameStatus.ActiveMino.MoveUp(); //ミノを正常な位置に戻す

        board.SaveBlockInGrid(gameStatus.ActiveMino); //gameStatus.ActiveMinoをセーブ

        gameStatus.LineEliminationCountHistory.Add(board.ClearAllRows()); //埋まっていれば削除し、ClearRowCountに消去ライン数を追加していく

        //Tspin判定(SpinActionsが4の時)
        if (gameStatus.SpinActions == 4)
        {
            //Tspinで1列も消去していない時
            //1列も消していなくてもTspin判定は行われる
            if (gameStatus.LineEliminationCountHistory[gameStatus.LineEliminationCountHistoryNumber] == 0)
            {
                //1列も消していない時のSE
                if (gameStatus.UseHardDrop == true)
                {
                    //7 Hard Drop
                    se.CallSE(7);
                }
                else
                {
                    //6 Normal Drop
                    se.CallSE(6);
                }

                //TspinMini判定
                if (gameStatus.UseSpinMini == true)
                {
                    //ゲーム画面に表示
                    mainSceneText.TextDisplay(calculate.Tspin_Mini); //修正予定
                }
                else
                {
                    //ゲーム画面に表示
                    mainSceneText.TextDisplay(calculate.Tspin); //修正予定
                }
            }
            //Tspinで1ライン消去した時
            else if (gameStatus.LineEliminationCountHistory[gameStatus.LineEliminationCountHistoryNumber] == 1)
            {
                //9 Spin Destroy
                se.CallSE(9);

                //TspinMini判定
                if (gameStatus.UseSpinMini == true)
                {
                    //ゲーム画面に表示
                    mainSceneText.TextDisplay(calculate.Tspin_Mini); //修正予定
                }
                else
                {
                    //ゲーム画面に表示
                    mainSceneText.TextDisplay(calculate.Tspin_Single); //修正予定
                }
            }
            //Tspinで2ライン消去した時
            else if (gameStatus.LineEliminationCountHistory[gameStatus.LineEliminationCountHistoryNumber] == 2)
            {
                //9 Spin Destroy
                se.CallSE(9);

                //TspinMini判定
                if (gameStatus.UseSpinMini == true)
                {
                    //ゲーム画面に表示
                    mainSceneText.TextDisplay(calculate.Tspin_Double_Mini);
                }
                else
                {
                    //ゲーム画面に表示
                    mainSceneText.TextDisplay(calculate.Tspin_Double);
                }
            }
            //Tspinで3ライン消去した時(TspinTripleMiniは存在しない)
            else
            {
                //9 Spin Destroy
                se.CallSE(9);

                //ゲーム画面に表示
                mainSceneText.TextDisplay(calculate.Tspin_Triple);
            }
        }
        //4列消えた時(Tetris!)
        else if (gameStatus.LineEliminationCountHistory[gameStatus.LineEliminationCountHistoryNumber] == 4)
        {
            //10 Tetris!
            se.CallSE(10);

            //ゲーム画面に表示
            mainSceneText.TextDisplay(calculate.Tetris);
        }
        //1〜3列消えた時
        else if (gameStatus.LineEliminationCountHistory[gameStatus.LineEliminationCountHistoryNumber] >= 1
            && gameStatus.LineEliminationCountHistory[gameStatus.LineEliminationCountHistoryNumber] <= 3)
        {
            //8 Normal Destroy
            se.CallSE(8);
        }
        //ハードドロップで1列も消していない時
        else if (gameStatus.UseHardDrop == true)
        {
            //7 Hard Drop
            se.CallSE(7);
        }
        //通常ドロップで1列も消していない時
        else
        {
            //6 Normal Drop
            se.CallSE(6);
        }

        gameStatus.SpinResetFlag();

        gameStatus.AllReset();

        gameStatus.MinoPopNumber++;

        gameStatus.LineEliminationCountHistoryNumber++;

        //countが7の倍数の時
        if (gameStatus.MinoPopNumber % 7 == 0)
        {
            //ミノの配列の補充
            calculate.DetermineSpawnMinoOrder();
        }

        //次のgameStatus.ActiveMinoの生成
        gameStatus.ActiveMino = spawner.SpawnMino(gameStatus.SpawnMinoOrder_List[gameStatus.MinoPopNumber]);

        //gameStatus.ActiveMinoのゴーストミノを生成
        gameStatus.GhostMino = spawner.SpawnMino_Ghost();

        if (!board.CheckPosition(gameStatus.ActiveMino))
        {
            gameStatus.GameOver = true;

            sceneTransition.GameOver();
        }

        spawner.SpawnNextMinos(); //Next表示
    }


    /*void SpinEffect(int i)
    {

    }*/
}
