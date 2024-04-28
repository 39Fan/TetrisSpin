using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// ゲームマネージャー
public class GameManager : MonoBehaviour
{
    // ミノの生成数、または設置数 //
    private int MinoPopNumber = 0;
    private int MinoPutNumber = 0; // Holdを使用すると、MinoPopNumberより1少なくなる

    // ロックダウン //
    [SerializeField] private bool isBottom = false;
    [SerializeField] private int BottomMoveCount = 0;
    [SerializeField] private int BottomMoveCountLimit = 15;
    [SerializeField] private int BottomBlockPosition_y = 20;
    private int StartingBottomBlockPosition_y = 20;

    // Hold //
    private bool UseHold = false; // Holdが使用されたか判別する変数
    private bool FirstHold = true; // ゲーム中で最初のHoldかどうかを判別する変数

    // 干渉するスクリプト //
    Board board;
    //Calculate calculate;
    MainSceneText mainSceneText;
    //Rotation rotation;
    SceneTransition sceneTransition;
    SE se;
    SpinCheck spinCheck;
    Spawner spawner;

    GameStatus gameStatus;
    Timer timer;
    Mino mino;
    //TetrisSpinData tetrisSpinData;

    //public Mino mino.activeMino; // 操作中のミノ
    //Mino_Ghost gameStatus.GhostMino; // ゴーストミノ
    //public Mino HoldMino; // ホールドミノ

    //private Minos mino.activeMino;
    //private Minos gameStatus.GhostMino;
    //private Minos hold_mino;

    // private string[] mino.activeMino_angle = new string[]
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
        //calculate = FindObjectOfType<Calculate>();
        mainSceneText = FindObjectOfType<MainSceneText>();
        //rotation = FindObjectOfType<Rotation>();
        se = FindObjectOfType<SE>();
        sceneTransition = FindObjectOfType<SceneTransition>();
        spinCheck = FindObjectOfType<SpinCheck>();

        gameStatus = FindObjectOfType<GameStatus>();
        timer = FindObjectOfType<Timer>();
        mino = FindObjectOfType<Mino>();
        //tetrisSpinData = FindObjectOfType<TetrisSpinData>();
    }

    private void Start()
    {
        // タイマーの初期設定
        timer.ResetTimer();

        // 2回繰り返す
        int length = 2;

        for (int i = 0; i < length; i++)
        {
            // ゲーム開始時、0から13番目のミノの順番を決める
            spawner.DetermineSpawnMinoOrder();
        }

        // 新しいActiveMinoの生成
        spawner.CreateNewActiveMino(MinoPopNumber);

        // // mino.activeMinoのゴーストミノの生成
        // gameStatus.GhostMino = spawner.SpawnMino_Ghost();

        // Nextの表示
        spawner.CreateNewNextMinos(MinoPopNumber);

    }

    private void Update()
    {
        if (gameStatus.gameOver)
        {
            return;
        }

        // if (Time.time > timer.keyReceptionTimer && gameStatus.Bottom == true)
        // {
        //     spawner.activeMino.MoveDown();
        //     BottomBoard();
        // }

        RockDown();

        PlayerInput(); // プレイヤーが制御できるコマンド

        AutoDown(); // 自動落下
    }

    // キーの入力を検知してブロックを動かす関数
    void PlayerInput()
    {
        // 右入力された時
        // Dキーに割り当て
        if (Input.GetKeyDown(KeyCode.D) && gameStatus.CanNotMove == false)
        {
            spawner.activeMino.MoveRight();

            timer.ContinuousLRKey = false;

            timer.UpdateLeftRightTimer();

            if (!board.CheckPosition(spawner.activeMino))
            {
                spawner.activeMino.MoveLeft();
            }
            else
            {
                se.CallSE(2);

                spawner.AdjustGhostMinoPosition(); // ゴーストミノの位置調整

                gameStatus.SpinResetFlag();

                //gameStatus.SpinActions = 7;
            }

            CheckBottomMoveCount();

            //BottomMove();
        }
        // 連続で右入力がされた時(右入力キーを長押しされている時)
        else if (Input.GetKey(KeyCode.D) && (Time.time > timer.NextKeyLeftRightTimer) && gameStatus.CanNotMove == false)
        {
            timer.ContinuousLRKey = true;

            timer.UpdateLeftRightTimer();

            spawner.activeMino.MoveRight();

            if (!board.CheckPosition(spawner.activeMino))
            {
                spawner.activeMino.MoveLeft();
            }
            else
            {
                se.CallSE(2);

                spawner.AdjustGhostMinoPosition(); // ゴーストミノの位置調整

                gameStatus.SpinResetFlag();

                //gameStatus.SpinActions = 7;
            }

            CheckBottomMoveCount();

            //BottomMove();
        }
        // 連続右入力の解除
        else if (Input.GetKeyUp(KeyCode.D))
        {
            timer.ContinuousLRKey = false;

            if (!board.CheckPosition(spawner.activeMino))
            {
                spawner.activeMino.MoveRight();
            }
        }
        // 左入力された時
        // Aキーに割り当て
        else if (Input.GetKeyDown(KeyCode.A) && gameStatus.CanNotMove == false)
        {
            timer.ContinuousLRKey = false;

            timer.UpdateLeftRightTimer();

            spawner.activeMino.MoveLeft();

            if (!board.CheckPosition(spawner.activeMino))
            {
                spawner.activeMino.MoveRight();
            }
            else
            {
                se.CallSE(2);

                spawner.AdjustGhostMinoPosition(); // ゴーストミノの位置調整

                gameStatus.SpinResetFlag();

                //gameStatus.SpinActions = 7;
            }

            CheckBottomMoveCount();

            //BottomMove();
        }
        // 連続で左入力がされた時(左入力キーを長押しされている時)
        else if (Input.GetKey(KeyCode.A) && (Time.time > timer.NextKeyLeftRightTimer) && gameStatus.CanNotMove == false)
        {
            timer.ContinuousLRKey = true;

            timer.UpdateLeftRightTimer();

            spawner.activeMino.MoveLeft();

            if (!board.CheckPosition(spawner.activeMino))
            {
                spawner.activeMino.MoveRight();
            }
            else
            {
                se.CallSE(2);

                spawner.AdjustGhostMinoPosition(); // ゴーストミノの位置調整

                gameStatus.SpinResetFlag();

                //gameStatus.SpinActions = 7;
            }

            CheckBottomMoveCount();

            //BottomMove();
        }
        // 連続右入力の解除
        else if (Input.GetKeyUp(KeyCode.A))
        {
            timer.ContinuousLRKey = false;

            if (!board.CheckPosition(spawner.activeMino))
            {
                spawner.activeMino.MoveRight();
            }
        }
        // 下入力された時
        // Sキーに割り当て
        else if (Input.GetKey(KeyCode.S) && (Time.time > timer.NextKeyDownTimer) && gameStatus.CanNotMove == false)
        {
            spawner.activeMino.MoveDown();

            timer.UpdateDownTimer();

            if (!board.CheckPosition(spawner.activeMino))
            {
                if (board.OverLimit(spawner.activeMino))
                {
                    //ゲームオーバー
                    gameStatus.GameOverAction();

                    sceneTransition.GameOver();
                }
                else
                {
                    spawner.activeMino.MoveUp();
                    // spawner.activeMino.MoveUp();

                    // gameStatus.Bottom = true;

                    // BottomMove();

                    //timer.keyReceptionTimer = Time.time + timer.keyReceptionInterval;
                }
            }
            se.CallSE(3);

            gameStatus.SpinResetFlag();

            //gameStatus.SpinActions = 7;
        }
        // 右回転入力された時
        // Pキーに割り当て
        else if (Input.GetKeyDown(KeyCode.P) && (Time.time > timer.NextKeyRotateTimer))
        {
            gameStatus.CanNotMove = true;

            gameStatus.SpinSetFlag();

            timer.UpdateRotateTimer();

            spawner.activeMino.RotateRight();

            Debug.Log("通過");

            // 回転後の角度(minoAngleAfter)の調整
            //calculate.CalibrateMinoAngleAfter();

            if (!board.CheckPosition(spawner.activeMino))
            {
                if (!mino.SuperRotationSystem())
                {
                    Debug.Log("回転禁止");

                    gameStatus.ResetMinoAngleAfter();

                    //4 Rotation
                    se.CallSE(4);
                }
                else
                {
                    Debug.Log("スーパーローテーション成功");

                    // ゴーストミノの向きを調整
                    //gameStatus.GhostMino.RotateRight(gameStatus.GhostMino);

                    spawner.AdjustGhostMinoPosition(); // ゴーストミノの位置調整

                    gameStatus.UpdateMinoAngleBefore();

                    spinCheck.CheckSpinType();

                    if (spinCheck.spinTypeName != "None")
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
                //gameStatus.GhostMino.RotateRight(gameStatus.GhostMino);

                spawner.AdjustGhostMinoPosition(); // ゴーストミノの位置を調整

                gameStatus.UpdateMinoAngleBefore();

                spinCheck.CheckSpinType();

                if (spinCheck.spinTypeName != "None")
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

            CheckBottomMoveCount();

            //BottomMove();

            gameStatus.CanNotMove = false;
        }
        // 左回転入力された時
        // Lキーに割り当て
        else if (Input.GetKeyDown(KeyCode.L) && (Time.time > timer.NextKeyRotateTimer))
        {
            gameStatus.CanNotMove = true;

            gameStatus.SpinSetFlag();

            timer.UpdateRotateTimer();

            spawner.activeMino.Rotateleft();

            // 回転後の角度(minoAngleAfter)の調整
            //calculate.CalibrateMinoAngleAfter();

            if (!board.CheckPosition(spawner.activeMino))
            {
                if (!mino.SuperRotationSystem())
                {
                    Debug.Log("回転禁止");

                    gameStatus.ResetMinoAngleAfter();

                    //4 Rotation
                    se.CallSE(4);
                }
                else
                {
                    Debug.Log("スーパーローテーション成功");

                    // ゴーストミノの向きを調整
                    //gameStatus.GhostMino.Rotateleft(gameStatus.GhostMino);

                    spawner.AdjustGhostMinoPosition(); // ゴーストミノの位置を調整

                    gameStatus.UpdateMinoAngleBefore();

                    spinCheck.CheckSpinType();

                    if (spinCheck.spinTypeName != "None")
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
                spawner.AdjustGhostMinoPosition(); // ゴーストミノの位置を調整

                gameStatus.UpdateMinoAngleBefore();

                spinCheck.CheckSpinType();

                if (spinCheck.spinTypeName != "None")
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

            CheckBottomMoveCount();

            //BottomMove();

            gameStatus.CanNotMove = false;
        }
        // ハードドロップ入力された時
        // Spaceキーに割り当て
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            gameStatus.UseHardDrop = true;

            for (int i = 0; i < 30; i++)
            {
                spawner.activeMino.MoveDown();

                if (!board.CheckPosition(spawner.activeMino))
                {
                    spawner.activeMino.MoveUp(); // ミノを正常な位置に戻す

                    break;
                }

                // 1マスでも落ちたらspin判定は消える。
                gameStatus.SpinResetFlag();

                //gameStatus.SpinActions = 7;
            }

            if (board.OverLimit(spawner.activeMino))
            {
                gameStatus.GameOverAction();

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
            if (UseHold == false)
            {
                // ホールドの使用
                UseHold = true;

                // HoldのSEを鳴らす
                se.CallSE(se.Hold);

                // ゴーストミノの削除
                //Destroy(gameStatus.GhostMino.gameObject);

                // mino.activeMinoのゴーストミノを生成
                //gameStatus.GhostMino = spawner.SpawnMino_Ghost();

                if (FirstHold == true) // ゲーム中で最初のHoldだった時
                {
                    MinoPopNumber++;

                    // ホールドの処理
                    spawner.CreateNewHoldMino(FirstHold, MinoPopNumber); // ActiveMinoをホールドに移動して、新しいミノを生成する

                    FirstHold = false; // ゲームオーバーまでfalse
                }
                else
                {
                    spawner.CreateNewHoldMino(FirstHold, MinoPopNumber);
                }
            }
        }
    }

    // 時間経過で落ちる時の処理をする関数
    void AutoDown()
    {
        if (Time.time > timer.AutoDropTimer)
        {
            spawner.activeMino.MoveDown();

            timer.UpdateDownTimer();

            if (!board.CheckPosition(spawner.activeMino))
            {
                if (board.OverLimit(spawner.activeMino))
                {
                    //ゲームオーバー
                    gameStatus.GameOverAction();

                    sceneTransition.GameOver();
                }
                else
                {
                    spawner.activeMino.MoveUp(); // ミノを正常な位置に戻す

                    // spawner.activeMino.MoveUp();

                    // gameStatus.Bottom = true;

                    // BottomMove();

                    // timer.keyReceptionTimer = Time.time + timer.keyReceptionInterval;
                }
            }
            gameStatus.SpinResetFlag();

            //gameStatus.SpinActions = 7;

            // nextdropTimer = Time.time + dropInteaval;
        }
    }

    //Bottomで移動、回転したとき
    // void BottomMove()
    // {
    //     if (timer.keyReceptionTimer > Time.time && gameStatus.Bottom == true)
    //     {
    //         timer.keyReceptionInterval = timer.keyReceptionInterval - (timer.keyReceptionTimer - Time.time) / 10;
    //         timer.keyReceptionTimer = Time.time + timer.keyReceptionInterval;
    //     }
    // }

    private void RockDown()
    {
        int newBottomBlockPosition_y = board.CheckActiveMinoBottomBlockPosition_y(spawner.activeMino, StartingBottomBlockPosition_y); // ActiveMino の1番下のブロックの座標を取得

        if (BottomBlockPosition_y <= newBottomBlockPosition_y) // ActivaMinoが、前回のy座標以上の位置にある時
        {
            spawner.activeMino.MoveDown();

            // 1マス下が底の時((底に面している時)
            // かつインターバル時間を超過している、または15回以上移動や回転を行った時
            if (!board.CheckPosition(spawner.activeMino) && (Time.time >= timer.BottomTimer || BottomMoveCount >= BottomMoveCountLimit))
            {
                spawner.activeMino.MoveUp(); // 元の位置に戻す

                // 変数のリセット
                BottomBlockPosition_y = StartingBottomBlockPosition_y;
                isBottom = false;
                BottomMoveCount = 0;

                BottomBoard(); // ミノの設置判定
            }

            spawner.activeMino.MoveUp(); // 元の位置に戻す
        }
        else // ActivaMinoが、前回のy座標より下の位置にある時
        {
            // isBottom = true; // 底に面している判定

            BottomMoveCount = 0; // リセットする

            BottomBlockPosition_y = newBottomBlockPosition_y; // BottomPositionの更新

            // timer.BottomTimer = Time.time + timer.BottomTimerInterval; // タイマーをスタート
        }

        // // Debug.Log(newBottomBlockPosition_y);

        // spawner.activeMino.MoveDown();

        // if (!board.CheckPosition(spawner.activeMino)) // 1マス下が底の時((底に面している時)
        // {
        //     spawner.activeMino.MoveUp(); // 元の位置に戻す

        //     if (BottomBlockPosition_y <= newBottomBlockPosition_y) // ActivaMinoが、前回のy座標以上の位置にある時
        //     {
        //         if (isBottom == false)
        //         {
        //             isBottom = true; // 底に面している判定

        //             BottomMoveCount = 0; // リセットする

        //             BottomBlockPosition_y = newBottomBlockPosition_y; // BottomPositionの更新

        //             timer.BottomTimer = Time.time + timer.BottomTimerInterval; // タイマーをスタート
        //         }

        //         // 底に面した状態で0.5秒が経過した時
        //         // または、15回移動や回転を行った時
        //         if (Time.time >= timer.BottomTimer || BottomMoveCount >= BottomMoveCountLimit)
        //         {
        //             // 変数のリセット
        //             BottomBlockPosition_y = StartingBottomBlockPosition_y;
        //             isBottom = false;
        //             BottomMoveCount = 0;

        //             BottomBoard(); // ミノの設置判定
        //         }
        //     }
        //     else // ActivaMinoが、前回のy座標より下の位置にある時
        //     {
        //         // isBottom = true; // 底に面している判定

        //         // BottomMoveCount = 0; // リセットする

        //         BottomBlockPosition_y = newBottomBlockPosition_y; // BottomPositionの更新

        //         timer.BottomTimer = Time.time + timer.BottomTimerInterval; // タイマーをスタート
        //     }
        // }
        // else
        // {
        //     spawner.activeMino.MoveUp(); // 元の位置に戻す

        //     isBottom = false;

        //     if (BottomBlockPosition_y <= newBottomBlockPosition_y) // ActivaMinoが、前回のy座標以上の位置にある時(更新していない時)
        //     {

        //     }
        //     else
        //     {
        //         BottomMoveCount = 0; // リセットする
        //     }
        // }

        // ロックダウンの判定
        // 0.5秒動きがない、または同じ高さで15回移動や回転が行われた時
        // かつ、ミノがブロックや壁に埋まっている時
        // if ((Time.time > timer.BottomTimer || BottomMoveCount == 15) && !board.CheckPosition(spawner.activeMino))
        // {
        //     //spawner.activeMino.MoveDown();
        //     BottomBoard();
        // }
    }

    private void CheckBottomMoveCount()
    {
        if (BottomMoveCount < BottomMoveCountLimit) // BottomMoveCount が15未満の時
        {
            BottomMoveCount++;

            // timer.BottomTimer = Time.time + timer.BottomTimerInterval;
        }
    }

    void BottomBoard() //底に着いたときの処理
    {
        Debug.Log("BottomBoard");

        //ゴーストミノの削除
        //Destroy(gameStatus.GhostMino.gameObject);

        //初期化
        //gameStatus.ResetUseHold();

        UseHold = false;

        //gameStatus.Bottom = false;

        timer.ResetTimer();

        //timer.keyReceptionInterval = 1f;

        // spawner.activeMino.MoveUp(); //ミノを正常な位置に戻す

        board.SaveBlockInGrid(spawner.activeMino); //mino.activeMinoをセーブ

        gameStatus.AddLineClearCountHistory(board.ClearAllRows(), MinoPutNumber); // 横列が埋まっていれば消去し、消去数を記録する

        mainSceneText.TextDisplay(gameStatus.lineClearCountHistory[MinoPutNumber]); // 消去数、Spinに対応したテキストを表示し、それに対応したSEも鳴らす

        // //Tspin判定(SpinActionsが4の時)
        // if (gameStatus.SpinActions == 4)
        // {
        //     //Tspinで1列も消去していない時
        //     //1列も消していなくてもTspin判定は行われる
        //     if (gameStatus.LineEliminationCountHistory[gameStatus.LineEliminationCountHistoryNumber] == 0)
        //     {
        //         //1列も消していない時のSE
        //         if (gameStatus.UseHardDrop == true)
        //         {
        //             //7 Hard Drop
        //             se.CallSE(7);
        //         }
        //         else
        //         {
        //             //6 Normal Drop
        //             se.CallSE(6);
        //         }

        //         //TspinMini判定
        //         if (gameStatus.UseSpinMini == true)
        //         {
        //             //ゲーム画面に表示
        //             mainSceneText.TextDisplay(calculate.Tspin_Mini); //修正予定
        //         }
        //         else
        //         {
        //             //ゲーム画面に表示
        //             mainSceneText.TextDisplay(calculate.Tspin); //修正予定
        //         }
        //     }
        //     //Tspinで1ライン消去した時
        //     else if (gameStatus.LineEliminationCountHistory[gameStatus.LineEliminationCountHistoryNumber] == 1)
        //     {
        //         //9 Spin Destroy
        //         se.CallSE(9);

        //         //TspinMini判定
        //         if (gameStatus.UseSpinMini == true)
        //         {
        //             //ゲーム画面に表示
        //             mainSceneText.TextDisplay(calculate.Tspin_Mini); //修正予定
        //         }
        //         else
        //         {
        //             //ゲーム画面に表示
        //             mainSceneText.TextDisplay(calculate.Tspin_Single); //修正予定
        //         }
        //     }
        //     //Tspinで2ライン消去した時
        //     else if (gameStatus.LineEliminationCountHistory[gameStatus.LineEliminationCountHistoryNumber] == 2)
        //     {
        //         //9 Spin Destroy
        //         se.CallSE(9);

        //         //TspinMini判定
        //         if (gameStatus.UseSpinMini == true)
        //         {
        //             //ゲーム画面に表示
        //             mainSceneText.TextDisplay(calculate.Tspin_Double_Mini);
        //         }
        //         else
        //         {
        //             //ゲーム画面に表示
        //             mainSceneText.TextDisplay(calculate.Tspin_Double);
        //         }
        //     }
        //     //Tspinで3ライン消去した時(TspinTripleMiniは存在しない)
        //     else
        //     {
        //         //9 Spin Destroy
        //         se.CallSE(9);

        //         //ゲーム画面に表示
        //         mainSceneText.TextDisplay(calculate.Tspin_Triple);
        //     }
        // }
        // //4列消えた時(Tetris!)
        // else if (gameStatus.LineEliminationCountHistory[gameStatus.LineEliminationCountHistoryNumber] == 4)
        // {
        //     //10 Tetris!
        //     se.CallSE(10);

        //     //ゲーム画面に表示
        //     mainSceneText.TextDisplay(calculate.Tetris);
        // }
        // //1〜3列消えた時
        // else if (gameStatus.LineEliminationCountHistory[gameStatus.LineEliminationCountHistoryNumber] >= 1
        //     && gameStatus.LineEliminationCountHistory[gameStatus.LineEliminationCountHistoryNumber] <= 3)
        // {
        //     //8 Normal Destroy
        //     se.CallSE(8);
        // }
        // //ハードドロップで1列も消していない時
        // else if (gameStatus.UseHardDrop == true)
        // {
        //     //7 Hard Drop
        //     se.CallSE(7);
        // }
        // //通常ドロップで1列も消していない時
        // else
        // {
        //     //6 Normal Drop
        //     se.CallSE(6);
        // }

        gameStatus.SpinResetFlag();

        gameStatus.AllReset();

        MinoPutNumber++;
        MinoPopNumber++;

        //gameStatus.LineEliminationCountHistoryNumber++;

        // //countが7の倍数の時
        // if (gameStatus.MinoPopNumber % 7 == 0)
        // {
        //     //ミノの配列の補充
        //     calculate.DetermineSpawnMinoOrder();
        // }

        // //次のminoMovement.ActiveMinoの生成
        // mino.activeMino = spawner.SpawnMino(gameStatus.SpawnMinoOrder_List[gameStatus.MinoPopNumber]);

        // //mino.activeMinoのゴーストミノを生成
        // gameStatus.GhostMino = spawner.SpawnMino_Ghost();

        spawner.CreateNewActiveMino(MinoPopNumber);

        if (!board.CheckPosition(spawner.activeMino))
        {
            gameStatus.GameOverAction();

            sceneTransition.GameOver();
        }

        spawner.CreateNewNextMinos(MinoPopNumber);

        //spawner.SpawnNextMinos(); //Next表示
    }


    /*void SpinEffect(int i)
    {

    }*/
}
