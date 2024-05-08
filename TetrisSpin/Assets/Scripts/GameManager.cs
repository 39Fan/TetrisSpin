using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

// ゲームマネージャー //
public class GameManager : MonoBehaviour
{
    // ミノの生成数、または設置数 //
    private int MinoPopNumber = 0;
    private int MinoPutNumber = 0; // Holdを使用すると、MinoPopNumberより1少なくなる

    // ロックダウン //
    // [SerializeField] private bool isBottom = false;
    [SerializeField] private int BottomMoveCount = 0;
    [SerializeField] private int BottomMoveCountLimit = 15;
    [SerializeField] private int BottomBlockPosition_y = 20;
    private int StartingBottomBlockPosition_y = 20;

    // Hold //
    private bool UseHold = false; // Holdが使用されたか判別する変数
    private bool FirstHold = true; // ゲーム中で最初のHoldかどうかを判別する変数

    // オーディオ //

    // "GameOver"
    // "Hard_Drop
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


    // 干渉するスクリプト //
    Board board;
    GameStatus gameStatus;
    MainSceneText mainSceneText;
    Mino mino;
    SceneTransition sceneTransition;
    Spawner spawner;
    SpinCheck spinCheck;
    Timer timer;
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

    // インスタンス化 //
    private void Awake()
    {
        board = FindObjectOfType<Board>();
        gameStatus = FindObjectOfType<GameStatus>();
        mainSceneText = FindObjectOfType<MainSceneText>();
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

        // if (Time.time > timer.keyReceptionTimer && gameStatus.Bottom == true)
        // {
        //     spawner.activeMino.MoveDown();
        //     SetMinoFixed();
        // }

        RockDown(); // ロックダウン判定

        PlayerInput(); // プレイヤーが制御できるコマンド

        AutoDown(); // 自動落下

        if (!board.CheckPosition(spawner.activeMino))
        {
            Debug.LogError("[GameManager] ゲームボードからミノがはみ出した。または、ブロックに重なった。");
        }

        if (!board.CheckPosition(spawner.ghostMino))
        {
            Debug.LogError("[GameManager] ゲームボードからゴーストミノがはみ出した。または、ブロックに重なった。");
        }
    }

    // キーの入力を検知してブロックを動かす関数 //
    void PlayerInput()
    {
        // 右入力された時
        if (Input.GetKeyDown(KeyCode.D)) // Dキーに割り当て
        {
            timer.ContinuousLRKey = false; // キーの連続入力でない

            timer.UpdateLeftRightTimer(); // タイマーのアップデート

            spawner.activeMino.MoveRight(); // 右に動かす

            if (!board.CheckPosition(spawner.activeMino)) // 右に動かせない時
            {
                spawner.activeMino.MoveLeft(); // 左に動かす(元に戻す)
            }
            else // 動かせた時
            {
                AudioManager.Instance.PlaySound("Move_Left_Right");  // オーディオの再生

                spawner.AdjustGhostMinoPosition(); // ゴーストミノの位置調整

                gameStatus.Reset_LastSRS(); // LastSRSの値を0に

                //gameStatus.SpinActions = 7;
            }

            IncreaseBottomMoveCount(); // BottomMoveCountの値を1増加

            //BottomMove();
        }
        // 連続で右入力がされた時(右入力キーを長押しされている時)
        else if (Input.GetKey(KeyCode.D) && (Time.time > timer.NextKeyLeftRightTimer))
        {
            timer.ContinuousLRKey = true; // キーの連続入力がされた

            timer.UpdateLeftRightTimer();

            spawner.activeMino.MoveRight();

            if (!board.CheckPosition(spawner.activeMino))
            {
                spawner.activeMino.MoveLeft();
            }
            else
            {
                AudioManager.Instance.PlaySound("Move_Left_Right");

                spawner.AdjustGhostMinoPosition();

                gameStatus.Reset_LastSRS();

                //gameStatus.SpinActions = 7;
            }

            IncreaseBottomMoveCount();

            //BottomMove();
        }
        // 連続右入力の解除
        else if (Input.GetKeyUp(KeyCode.D))
        {
            timer.ContinuousLRKey = false; // キーの連続入力でない

            // if (!board.CheckPosition(spawner.activeMino))
            // {
            //     spawner.activeMino.MoveRight();
            // }
        }
        // 左入力された時
        else if (Input.GetKeyDown(KeyCode.A)) // Aキーに割り当て
        {
            timer.ContinuousLRKey = false; // キーの連続入力でない

            timer.UpdateLeftRightTimer();

            gameStatus.Reset_LastSRS();

            spawner.activeMino.MoveLeft();

            if (!board.CheckPosition(spawner.activeMino))
            {
                spawner.activeMino.MoveRight();
            }
            else
            {
                AudioManager.Instance.PlaySound("Move_Left_Right");

                spawner.AdjustGhostMinoPosition(); // ゴーストミノの位置調整

                gameStatus.Reset_LastSRS();

                //gameStatus.SpinActions = 7;
            }

            IncreaseBottomMoveCount();

            //BottomMove();
        }
        // 連続で左入力がされた時(左入力キーを長押しされている時)
        else if (Input.GetKey(KeyCode.A) && (Time.time > timer.NextKeyLeftRightTimer))
        {
            timer.ContinuousLRKey = true; // キーの連続入力がされた

            timer.UpdateLeftRightTimer();

            gameStatus.Reset_LastSRS();

            spawner.activeMino.MoveLeft();

            if (!board.CheckPosition(spawner.activeMino))
            {
                spawner.activeMino.MoveRight();
            }
            else
            {
                AudioManager.Instance.PlaySound("Move_Left_Right");

                spawner.AdjustGhostMinoPosition(); // ゴーストミノの位置調整

                gameStatus.Reset_LastSRS();

                //gameStatus.SpinActions = 7;
            }

            IncreaseBottomMoveCount();

            //BottomMove();
        }
        // 連続右入力の解除
        else if (Input.GetKeyUp(KeyCode.A))
        {
            timer.ContinuousLRKey = false; // キーの連続入力でない

            // if (!board.CheckPosition(spawner.activeMino))
            // {
            //     spawner.activeMino.MoveRight();
            // }
        }
        // 下入力された時
        else if (Input.GetKey(KeyCode.S) && (Time.time > timer.NextKeyDownTimer)) // Sキーに割り当て
        {
            timer.UpdateDownTimer();

            gameStatus.Reset_LastSRS();

            spawner.activeMino.MoveDown();

            if (!board.CheckPosition(spawner.activeMino))
            {

                spawner.activeMino.MoveUp();
                // spawner.activeMino.MoveUp();

                // gameStatus.Bottom = true;

                // BottomMove();

                //timer.keyReceptionTimer = Time.time + timer.keyReceptionInterval;

            }
            else
            {
                AudioManager.Instance.PlaySound("Move_Down");

                gameStatus.Reset_LastSRS();
            }

            //gameStatus.SpinActions = 7;
        }
        // 右回転入力された時
        else if (Input.GetKeyDown(KeyCode.P) && (Time.time > timer.NextKeyRotateTimer)) // Pキーに割り当て
        {
            //gameStatus.CanNotMove = true;

            timer.UpdateRotateTimer();

            gameStatus.Reset_LastSRS();

            spawner.activeMino.RotateRight();

            // 回転後の角度(minoAngleAfter)の調整
            //calculate.CalibrateMinoAngleAfter();

            if (!board.CheckPosition(spawner.activeMino)) // 通常回転ができなかった時
            {
                if (!mino.SuperRotationSystem()) // SRSもできなかった時
                {
                    Debug.Log("回転禁止");

                    gameStatus.Reset_MinoAngleAfter(); // MinoAngleAfterのリセット

                    AudioManager.Instance.PlaySound("Rotation");
                }
                else // SRSが成功した時
                {
                    Debug.Log("スーパーローテーション成功");

                    // ゴーストミノの向きを調整
                    //gameStatus.GhostMino.RotateRight(gameStatus.GhostMino);

                    SuccessRotateAction(); // 回転が成功した時の処理を実行
                }
            }
            else // 通常回転が成功した時
            {
                // ゴーストミノの向きを調整
                //gameStatus.GhostMino.RotateRight(gameStatus.GhostMino);

                SuccessRotateAction();
            }

            IncreaseBottomMoveCount();

            //BottomMove();

            //gameStatus.CanNotMove = false;
        }
        // 左回転入力された時
        else if (Input.GetKeyDown(KeyCode.L) && (Time.time > timer.NextKeyRotateTimer)) // Lキーに割り当て
        {
            // gameStatus.CanNotMove = true;

            timer.UpdateRotateTimer();

            gameStatus.Reset_LastSRS();

            spawner.activeMino.Rotateleft();

            // 回転後の角度(minoAngleAfter)の調整
            //calculate.CalibrateMinoAngleAfter();

            if (!board.CheckPosition(spawner.activeMino))
            {
                if (!mino.SuperRotationSystem())
                {
                    Debug.Log("回転禁止");

                    gameStatus.Reset_MinoAngleAfter();

                    AudioManager.Instance.PlaySound("Rotation");
                }
                else
                {
                    Debug.Log("スーパーローテーション成功");

                    // ゴーストミノの向きを調整
                    //gameStatus.GhostMino.Rotateleft(gameStatus.GhostMino);

                    SuccessRotateAction();
                }
            }
            else
            {
                SuccessRotateAction();
            }

            IncreaseBottomMoveCount();

            //BottomMove();

            ////gameStatus.CanNotMove = false;
        }
        // ハードドロップ入力された時
        else if (Input.GetKeyDown(KeyCode.Space)) // Spaceキーに割り当て
        {
            // gameStatus.Set_UseHardDrop(); // 

            AudioManager.Instance.PlaySound("Hard_Drop");

            int height = 20; // ゲームフィールドの高さの値

            for (int i = 0; i < height; i++) // heightの値分繰り返す
            {
                spawner.activeMino.MoveDown();

                if (!board.CheckPosition(spawner.activeMino)) // 底にぶつかった時
                {
                    spawner.activeMino.MoveUp(); // ミノを正常な位置に戻す

                    break; // For文を抜ける
                }

                gameStatus.Reset_LastSRS(); // 1マスでも下に移動した時、LastSRSの値を0にする

                //gameStatus.SpinActions = 7;
            }

            if (board.OverLimit(spawner.activeMino))
            {
                gameStatus.GameOverAction();

                sceneTransition.GameOver();
            }
            else
            {
                Reset_RockDown(); // RockDownに関する変数のリセット

                SetMinoFixed(); // 底に着いたときの処理
            }
        }
        // ホールド入力された時
        else if (Input.GetKeyDown(KeyCode.Return)) // Enter(Return)キーに割り当て
        {
            // Holdは1度使うと、ミノを設置するまで使えない
            // ミノを設置すると、useHold = false になる
            if (UseHold == false)
            {
                UseHold = true; // ホールドの使用

                AudioManager.Instance.PlaySound("Hold");

                Reset_RockDown(); // RockDownに関する変数のリセット

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

    // 時間経過で落ちる時の処理をする関数 //
    void AutoDown()
    {
        if (Time.time > timer.AutoDropTimer)
        {
            timer.UpdateDownTimer();

            gameStatus.Reset_LastSRS();

            spawner.activeMino.MoveDown();

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

    // ロックダウンの処理をする関数 //
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

                AudioManager.Instance.PlaySound("Normal_Drop");

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

        //             SetMinoFixed(); // ミノの設置判定
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
        //     SetMinoFixed();
        // }
    }

    // BottomMoveCountを進める関数 //
    private void IncreaseBottomMoveCount()
    {
        if (BottomMoveCount < BottomMoveCountLimit) // BottomMoveCount が15未満の時
        {
            BottomMoveCount++;

            // timer.BottomTimer = Time.time + timer.BottomTimerInterval;
        }
    }

    // 回転が成功した時の処理をする関数 //
    private void SuccessRotateAction()
    {
        spawner.AdjustGhostMinoPosition(); // ゴーストミノの位置調整

        gameStatus.UpdateMinoAngleBefore(); // MinoAngleBeforeのアップデート

        spinCheck.CheckSpinType(); // スピン判定のチェック

        if (spinCheck.spinTypeName != "None") // スピン判定がない場合
        {
            AudioManager.Instance.PlaySound("Spin");
        }
        else // スピン判定がある場合
        {
            AudioManager.Instance.PlaySound("Rotation");
        }
    }

    // ミノの設置場所が確定した時の処理をする関数 //
    void SetMinoFixed()
    {
        Reset_RockDown();
        UseHold = false;
        timer.ResetTimer();

        //ゴーストミノの削除
        //Destroy(gameStatus.GhostMino.gameObject);

        //初期化
        //gameStatus.ResetUseHold();

        //gameStatus.Bottom = false;

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

        gameStatus.Reset_LastSRS();

        spinCheck.Reset_SpinTypeName();

        gameStatus.Reset_Angle();

        // gameStatus.Reset_UseHardDrop();

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

        spawner.CreateNewNextMinos(MinoPopNumber);

        if (!board.CheckPosition(spawner.activeMino))
        {
            gameStatus.GameOverAction();

            sceneTransition.GameOver();
        }

        //spawner.SpawnNextMinos(); //Next表示
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

    // RockDownに関する変数のリセット //
    public void Reset_RockDown()
    {
        BottomBlockPosition_y = StartingBottomBlockPosition_y;
        BottomMoveCount = 0;
    }
}
