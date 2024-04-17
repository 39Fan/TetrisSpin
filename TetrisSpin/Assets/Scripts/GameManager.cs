using System.Collections.Generic;
using UnityEngine;

//GameManager
//ゲームの進行を制御するスクリプト

public class GameManager : MonoBehaviour
{
    //各種干渉するスクリプトの設定
    Board board;
    Data data;
    Rotation rotation;
    SceneTransition sceneTransition;
    SE se;
    Spawner spawner;

    //操作中のミノ
    public Block ActiveMino;

    //ゴーストミノ
    Block_Ghost ActiveMino_Ghost;

    //ホールドミノ
    public Block HoldMino;

    [SerializeField]
    float dropInteaval; //次にブロックが落ちるまでのインターバル時間
    float nextdropTimer;  //次にブロックが落ちるまでの時間
    float nextKeyDownTimer, nextKeyLeftRightTimer, nextKeyRotateTimer; //入力受付タイマー(3種類)

    [SerializeField]
    float keyReceptionTimer;

    [SerializeField]
    private float nextKeyDownInterval, nextKeyLeftRightInterval, nextKeyRotateInterval, keyReceptionInterval; //入力インターバル(4種類)

    //ゲームオーバー判定
    bool gameOver;

    [SerializeField]
    int SpinActions = 7;

    //底についた判定
    bool Bottom = false;

    bool CanNotMove = false;

    [SerializeField]
    List<int> ClearRowHistory = new List<int>();

    int ClearRowHistoryCount = 0;

    //インスタンス化
    private void Awake()
    {
        spawner = FindObjectOfType<Spawner>();
        board = FindObjectOfType<Board>();
        data = FindObjectOfType<Data>();
        rotation = FindObjectOfType<Rotation>();
        ActiveMino = FindObjectOfType<Block>();
        se = FindObjectOfType<SE>();
        sceneTransition = FindObjectOfType<SceneTransition>();
    }

    private void Start()
    {
        //スポーン位置の数値を丸める
        //spawner.transform.position = Rounding.Round(spawner.transform.position);

        //タイマーの初期設定
        nextKeyDownTimer = Time.time + nextKeyDownInterval;
        nextKeyLeftRightTimer = Time.time + nextKeyLeftRightInterval;
        nextKeyRotateTimer = Time.time + nextKeyRotateInterval;
        keyReceptionTimer = Time.time + keyReceptionInterval;

        if (!ActiveMino)
        {
            //2回繰り返す
            int length = 2;

            for (int i = 0; i < length; i++)
            {
                //ゲーム開始時、0から13番目のミノの順番を決める
                data.DecideSpawnMinoOrder();
            }

            //新しいActiveMinoの生成
            ActiveMino = spawner.SpawnMino(data.spawnMinoOrder[data.count]);

            //activeMinoの種類を判別
            data.CheckActiveMinoShape();

            //ActiveMinoのゴーストミノの生成
            ActiveMino_Ghost = spawner.SpawnMino_Ghost(ActiveMino);

            //Nextの表示
            spawner.SpawnNextBlocks();
        }
    }

    private void Update()
    {
        if (gameOver)
        {
            return;
        }

        /*if (ActiveMino_Ghost)
        {
            board.DestroyBlock_Ghost(ActiveMino_Ghost);
        }*/

        if (Time.time > keyReceptionTimer && Bottom == true)
        {
            ActiveMino.MoveDown();
            BottomBoard();
        }

        PlayerInput();

        Down();
    }

    //キーの入力を検知してブロックを動かす関数
    void PlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.D) && CanNotMove == false) //右入力
        {
            nextKeyLeftRightInterval = 0.20f;

            ActiveMino.MoveRight();

            nextKeyLeftRightTimer = Time.time + nextKeyLeftRightInterval;

            if (!board.CheckPosition(ActiveMino))
            {
                ActiveMino.MoveLeft();
            }
            else
            {
                se.CallSE(2);

                //ゴーストミノの位置調整を実行
                ActiveMino_Ghost.transform.position = data.PositionAdjustmentActiveMino_Ghost(ActiveMino);

                data.SpinReset();

                SpinActions = 7;
            }

            BottomMove();
        }
        else if (Input.GetKey(KeyCode.D) && (Time.time > nextKeyLeftRightTimer) && CanNotMove == false) //連続右入力
        {
            nextKeyLeftRightInterval = 0.05f;

            ActiveMino.MoveRight();

            nextKeyLeftRightTimer = Time.time + nextKeyLeftRightInterval;

            if (!board.CheckPosition(ActiveMino))
            {
                ActiveMino.MoveLeft();
            }
            else
            {
                se.CallSE(2);

                //ゴーストミノの位置調整を実行
                ActiveMino_Ghost.transform.position = data.PositionAdjustmentActiveMino_Ghost(ActiveMino);

                data.SpinReset();

                SpinActions = 7;
            }

            BottomMove();
        }
        else if (Input.GetKeyUp(KeyCode.D)) //連続右入力の解除
        {
            nextKeyLeftRightInterval = 0.20f;

            if (!board.CheckPosition(ActiveMino))
            {
                ActiveMino.MoveRight();
            }
        }
        else if (Input.GetKeyDown(KeyCode.A) && CanNotMove == false) //左入力
        {
            nextKeyLeftRightInterval = 0.20f;

            ActiveMino.MoveLeft();

            nextKeyLeftRightTimer = Time.time + nextKeyLeftRightInterval;

            if (!board.CheckPosition(ActiveMino))
            {
                ActiveMino.MoveRight();
            }
            else
            {
                se.CallSE(2);

                //ゴーストミノの位置調整を実行
                ActiveMino_Ghost.transform.position = data.PositionAdjustmentActiveMino_Ghost(ActiveMino);

                data.SpinReset();

                SpinActions = 7;
            }

            BottomMove();
        }
        else if (Input.GetKey(KeyCode.A) && (Time.time > nextKeyLeftRightTimer) && CanNotMove == false) //連続左入力
        {
            nextKeyLeftRightInterval = 0.05f;

            ActiveMino.MoveLeft();

            nextKeyLeftRightTimer = Time.time + nextKeyLeftRightInterval;

            if (!board.CheckPosition(ActiveMino))
            {
                ActiveMino.MoveRight();
            }
            else
            {
                se.CallSE(2);

                //ゴーストミノの位置調整を実行
                ActiveMino_Ghost.transform.position = data.PositionAdjustmentActiveMino_Ghost(ActiveMino);

                data.SpinReset();

                SpinActions = 7;
            }

            BottomMove();
        }
        else if (Input.GetKeyUp(KeyCode.A)) //連続右入力の解除
        {
            nextKeyLeftRightInterval = 0.20f;

            if (!board.CheckPosition(ActiveMino))
            {
                ActiveMino.MoveRight();
            }
        }
        else if (Input.GetKeyDown(KeyCode.P) && (Time.time > nextKeyRotateTimer)) //右回転
        {
            CanNotMove = true;

            data.useSpin = true;

            data.spinMini = false;

            data.lastSRS = 0;

            SpinActions = 7;

            ActiveMino.RotateRight(ActiveMino);

            //回転後の角度(minoAngleAfter)の調整
            data.CalibrateMinoAngleAfter(ActiveMino);

            nextKeyRotateTimer = Time.time + nextKeyRotateInterval;

            if (!board.CheckPosition(ActiveMino))
            {
                if (!rotation.MinoSuperRotation(ActiveMino))
                {
                    Debug.Log("回転禁止");

                    //4 Rotation
                    se.CallSE(4);
                }
                else
                {
                    Debug.Log("スーパーローテーション成功");

                    //ゴーストミノの向きを調整
                    ActiveMino_Ghost.RotateRight(ActiveMino_Ghost);

                    //ゴーストミノの位置を調整
                    ActiveMino_Ghost.transform.position = data.PositionAdjustmentActiveMino_Ghost(ActiveMino);

                    data.minoAngleBefore = data.minoAngleAfter;

                    SpinActions = rotation.SpinTerminal(data.order);

                    if (SpinActions == 4)
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
                //ゴーストミノの向きを調整
                ActiveMino_Ghost.RotateRight(ActiveMino_Ghost);

                //ゴーストミノの位置を調整
                ActiveMino_Ghost.transform.position = data.PositionAdjustmentActiveMino_Ghost(ActiveMino);

                data.minoAngleBefore = data.minoAngleAfter;

                SpinActions = rotation.SpinTerminal(data.order);

                if (SpinActions == 4)
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

            CanNotMove = false;
        }
        else if (Input.GetKeyDown(KeyCode.L) && (Time.time > nextKeyRotateTimer)) //左回転
        {
            CanNotMove = true;

            data.useSpin = true;

            data.spinMini = false;

            data.lastSRS = 0;

            SpinActions = 7;

            ActiveMino.Rotateleft(ActiveMino);

            //回転後の角度(minoAngleAfter)の調整
            data.CalibrateMinoAngleAfter(ActiveMino);

            nextKeyRotateTimer = Time.time + nextKeyRotateInterval;

            if (!board.CheckPosition(ActiveMino))
            {
                if (!rotation.MinoSuperRotation(ActiveMino))
                {
                    Debug.Log("回転禁止");

                    //4 Rotation
                    se.CallSE(4);
                }
                else
                {
                    Debug.Log("スーパーローテーション成功");

                    //ゴーストミノの向きを調整
                    ActiveMino_Ghost.Rotateleft(ActiveMino_Ghost);

                    //ゴーストミノの位置を調整
                    ActiveMino_Ghost.transform.position = data.PositionAdjustmentActiveMino_Ghost(ActiveMino);

                    data.minoAngleBefore = data.minoAngleAfter;

                    SpinActions = rotation.SpinTerminal(data.order);

                    if (SpinActions == 4)
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
                //ゴーストミノの向きを調整
                ActiveMino_Ghost.Rotateleft(ActiveMino_Ghost);

                //ゴーストミノの位置を調整
                ActiveMino_Ghost.transform.position = data.PositionAdjustmentActiveMino_Ghost(ActiveMino);

                data.minoAngleBefore = data.minoAngleAfter;

                SpinActions = rotation.SpinTerminal(data.order);

                if (SpinActions == 4)
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

            CanNotMove = false;
        }
        else if (Input.GetKeyDown(KeyCode.Space)) //ハードドロップ
        {
            data.hardDrop = true;

            for (int i = 0; i < 30; i++)
            {
                ActiveMino.MoveDown();

                if (!board.CheckPosition(ActiveMino))
                {
                    break;
                }

                data.SpinReset(); //1マスでも落ちたらspin判定は消える。
                SpinActions = 7;
            }

            if (board.OverLimit(ActiveMino))
            {
                gameOver = true;
                sceneTransition.GameOver();
            }
            else
            {
                //底に着いたときの処理
                BottomBoard();
            }
        }
        //ホールド
        //Enterキーに割り当て
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            //Holdは1度使うと、ミノを設置するまで使えない
            //ミノを設置すると、useHold = false になる
            if (data.useHold == false)
            {
                //HoldのSEを鳴らす
                se.CallSE(se.Hold);

                //ゴーストミノの削除
                Destroy(ActiveMino_Ghost.gameObject);

                //ホールドの処理
                //ActiveMinoをホールドに移動して、新しいミノを生成する
                data.Hold();

                //ActiveMinoのゴーストミノを生成
                ActiveMino_Ghost = spawner.SpawnMino_Ghost(ActiveMino);

                data.useHold = true;
            }
        }
    }

    void Down()
    {
        if (Input.GetKey(KeyCode.S) && (Time.time > nextKeyDownTimer) && CanNotMove == false || (Time.time > nextdropTimer)) //下入力、または時間経過で落ちる時
        {
            ActiveMino.MoveDown();

            nextKeyDownTimer = Time.time + nextKeyDownInterval;

            if (!board.CheckPosition(ActiveMino))
            {
                if (board.OverLimit(ActiveMino))
                {
                    //ゲームオーバー
                    gameOver = true;
                    sceneTransition.GameOver();
                }
                else
                {
                    ActiveMino.MoveUp();
                    Bottom = true;
                    BottomMove();
                    keyReceptionTimer = Time.time + keyReceptionInterval;
                }
            }
            else
            {
                //下入力をした際はSEの3を鳴らす
                if (Time.time <= nextdropTimer)
                {
                    se.CallSE(3);
                }

                data.SpinReset();
                SpinActions = 7;
            }
            nextdropTimer = Time.time + dropInteaval;
        }
    }

    //Bottomで移動、回転したとき
    void BottomMove()
    {
        if (keyReceptionTimer > Time.time && Bottom == true)
        {
            keyReceptionInterval = keyReceptionInterval - (keyReceptionTimer - Time.time) / 10;
            keyReceptionTimer = Time.time + keyReceptionInterval;
        }
    }

    void BottomBoard() //底に着いたときの処理
    {
        Debug.Log("BottomBoard");

        //ゴーストミノの削除
        Destroy(ActiveMino_Ghost.gameObject);

        //初期化
        Bottom = false;
        nextKeyDownTimer = Time.time;
        nextKeyLeftRightTimer = Time.time;
        nextKeyRotateTimer = Time.time;
        keyReceptionTimer = Time.time;
        keyReceptionInterval = 1;

        ActiveMino.MoveUp(); //ミノを正常な位置に戻す

        board.SaveBlockInGrid(ActiveMino); //ActiveMinoをセーブ

        ClearRowHistory.Add(board.ClearAllRows()); //埋まっていれば削除し、ClearRowCountに消去ライン数を追加していく

        //Tspin判定(SpinActionsが4の時)
        if (SpinActions == 4)
        {
            //Tspinで1列も消去していない時
            //1列も消していなくてもTspin判定は行われる
            if (ClearRowHistory[ClearRowHistoryCount] == 0)
            {
                //1列も消していない時のSE
                if (data.hardDrop == true)
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
                if (data.spinMini == true)
                {
                    Debug.Log("TspinMini!");
                }
                else
                {
                    Debug.Log("Tspin!");
                }
            }
            //Tspinで1ライン消去した時
            else if (ClearRowHistory[ClearRowHistoryCount] == 1)
            {
                //9 Spin Destroy
                se.CallSE(9);

                //TspinMini判定
                if (data.spinMini == true)
                {
                    Debug.Log("TspinMini!");
                }
                else
                {
                    Debug.Log("TspinSingle!");
                }
            }
            //Tspinで2ライン消去した時
            else if (ClearRowHistory[ClearRowHistoryCount] == 2)
            {
                //9 Spin Destroy
                se.CallSE(9);

                //TspinMini判定
                if (data.spinMini == true)
                {
                    Debug.Log("TspinDoubleMini!");
                }
                else
                {
                    Debug.Log("TspinDouble!");
                }
            }
            //Tspinで3ライン消去した時(TspinTripleMiniは存在しない)
            else
            {
                //9 Spin Destroy
                se.CallSE(9);

                Debug.Log("TspinTriple!");
            }
        }
        //4列消えた時(Tetris!)
        else if (ClearRowHistory[ClearRowHistoryCount] == 4)
        {
            //10 Tetris!
            se.CallSE(10);
        }
        //1〜3列消えた時
        else if (ClearRowHistory[ClearRowHistoryCount] >= 1 && ClearRowHistory[ClearRowHistoryCount] <= 3)
        {
            //8 Normal Destroy
            se.CallSE(8);
        }
        //ハードドロップで1列も消していない時
        else if (data.hardDrop == true)
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

        SpinActions = 7;

        data.spinMini = false;

        ClearRowHistoryCount++;

        data.AllReset();

        //count進行
        data.count++;

        //countが7の倍数の時
        if (data.count % 7 == 0)
        {
            //ミノの配列の補充
            data.DecideSpawnMinoOrder();

            //次のActiveMinoの生成
            ActiveMino = spawner.SpawnMino(data.spawnMinoOrder[data.count]);
        }
        else
        {
            //次のActiveMinoの生成
            ActiveMino = spawner.SpawnMino(data.spawnMinoOrder[data.count]);
        }

        //activeMinoの種類を判別
        data.CheckActiveMinoShape();

        //ActiveMinoのゴーストミノを生成
        ActiveMino_Ghost = spawner.SpawnMino_Ghost(ActiveMino);

        if (!board.CheckPosition(ActiveMino))
        {
            gameOver = true;

            sceneTransition.GameOver();
        }

        spawner.SpawnNextBlocks(); //Next表示
    }


    /*void SpinEffect(int i)
    {

    }*/
}
