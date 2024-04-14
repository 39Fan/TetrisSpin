using System.Collections.Generic;
using System.Globalization;
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

    int Count = 0; //順番
    Block ActiveBlock; //生成されたブロック格納

    [SerializeField]
    private float dropInteaval; //次にブロックが落ちるまでのインターバル時間
    float NextdropTimer;  //次にブロックが落ちるまでの時間
    float nextKeyDownTimer, nextKeyLeftRightTimer, nextKeyRotateTimer; //入力受付タイマー(4種類)

    [SerializeField]
    float keyReceptionTimer;

    [SerializeField]
    private float nextKeyDownInterval, nextKeyLeftRightInterval, nextKeyRotateInterval, keyReceptionInterval; //入力インターバル(4種類)

    //ゲームオーバー判定
    bool gameOver;

    //ランダムミノの生成に必要な配列
    List<int> numbers = new List<int>();
    public List<int> MinoOrder = new List<int>();

    // 回転使用フラグ
    bool UseSpin = false;

    public int LastSRS; // 最後に行ったスーパーローテーション(SR)パターン(0-4)

    [SerializeField]
    int SpinActions = 7;
    public bool SpinMini = false;

    //ホールド機能に必要なもの
    bool FirstHold = true;
    bool Hold = false;
    Block NewHoldmino;

    //GhostBlickの機能に必要なもの
    Block_Ghost ActiveBlock_Ghost;

    //底についた判定
    bool Bottom = false;

    bool CanNotMove = false;

    [SerializeField]
    List<int> ClearRowHistory = new List<int>();

    int ClearRowHistoryCount = 0;

    bool HardDrop = false;


    private void Start()
    {
        //スポナーオブジェクトをスポナー変数に格納する
        spawner = FindObjectOfType<Spawner>();
        //ボードを変数に格納する
        board = FindObjectOfType<Board>();

        data = FindObjectOfType<Data>();

        rotation = FindObjectOfType<Rotation>();

        ActiveBlock = FindObjectOfType<Block>();

        se = FindObjectOfType<SE>();

        sceneTransition = FindObjectOfType<SceneTransition>();

        //スポーン位置の数値を丸める
        spawner.transform.position = Rounding.Round(spawner.transform.position);

        //タイマーの初期設定
        nextKeyDownTimer = Time.time + nextKeyDownInterval;
        nextKeyLeftRightTimer = Time.time + nextKeyLeftRightInterval;
        nextKeyRotateTimer = Time.time + nextKeyRotateInterval;
        keyReceptionTimer = Time.time + keyReceptionInterval;

        //スポナークラスからブロック生成関数を呼んで変数に格納する
        if (!ActiveBlock)
        {
            for (int i = 0; i < 2; i++) //0から13番目のミノの順番を決める
            {
                for (int j = 0; j <= 6; j++)
                {
                    numbers.Add(j);
                }

                while (numbers.Count > 0)
                {
                    int index = Random.Range(0, numbers.Count);

                    int ransu = numbers[index];

                    MinoOrder.Add(ransu);

                    numbers.RemoveAt(index);
                }
            }

            ActiveBlock = spawner.SpawnBlock(MinoOrder[Count]); //activeBlockの生成

            spawner.SpawnNextBlocks(Count + 1, MinoOrder.ToArray()); //Next表示

            Count++;
        }
    }

    private void Update()
    {
        if (gameOver)
        {
            return;
        }

        if (ActiveBlock_Ghost)
        {
            board.DestroyBlock_Ghost(ActiveBlock_Ghost);
        }

        if (Time.time > keyReceptionTimer && Bottom == true)
        {
            ActiveBlock.MoveDown();
            BottomBoard();
        }

        ActiveBlock_Ghost = spawner.SpawnBlock_Ghost(ActiveBlock);

        PlayerInput();

        Down();
    }

    //キーの入力を検知してブロックを動かす関数
    void PlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.D) && CanNotMove == false) //右入力
        {
            nextKeyLeftRightInterval = 0.20f;

            ActiveBlock.MoveRight();

            nextKeyLeftRightTimer = Time.time + nextKeyLeftRightInterval;

            if (!board.CheckPosition(ActiveBlock))
            {
                ActiveBlock.MoveLeft();
            }
            else
            {
                se.CallSE(2);

                UseSpin = false;
                LastSRS = 0;
                SpinActions = 7;
            }

            BottomMove();
        }
        else if (Input.GetKey(KeyCode.D) && (Time.time > nextKeyLeftRightTimer) && CanNotMove == false) //連続右入力
        {
            nextKeyLeftRightInterval = 0.05f;

            ActiveBlock.MoveRight();

            nextKeyLeftRightTimer = Time.time + nextKeyLeftRightInterval;

            if (!board.CheckPosition(ActiveBlock))
            {
                ActiveBlock.MoveLeft();
            }
            else
            {
                se.CallSE(2);

                UseSpin = false;
                LastSRS = 0;
                SpinActions = 7;
            }

            BottomMove();
        }
        else if (Input.GetKeyUp(KeyCode.D)) //連続右入力の解除
        {
            nextKeyLeftRightInterval = 0.20f;

            if (!board.CheckPosition(ActiveBlock))
            {
                ActiveBlock.MoveRight();
            }
        }
        else if (Input.GetKeyDown(KeyCode.A) && CanNotMove == false) //左入力
        {
            nextKeyLeftRightInterval = 0.20f;

            ActiveBlock.MoveLeft();

            nextKeyLeftRightTimer = Time.time + nextKeyLeftRightInterval;

            if (!board.CheckPosition(ActiveBlock))
            {
                ActiveBlock.MoveRight();
            }
            else
            {
                se.CallSE(2);

                UseSpin = false;
                LastSRS = 0;
                SpinActions = 7;
            }

            BottomMove();
        }
        else if (Input.GetKey(KeyCode.A) && (Time.time > nextKeyLeftRightTimer) && CanNotMove == false) //連続左入力
        {
            nextKeyLeftRightInterval = 0.05f;

            ActiveBlock.MoveLeft();

            nextKeyLeftRightTimer = Time.time + nextKeyLeftRightInterval;

            if (!board.CheckPosition(ActiveBlock))
            {
                ActiveBlock.MoveRight();
            }
            else
            {
                se.CallSE(2);

                UseSpin = false;
                LastSRS = 0;
                SpinActions = 7;
            }

            BottomMove();
        }
        else if (Input.GetKeyUp(KeyCode.A)) //連続右入力の解除
        {
            nextKeyLeftRightInterval = 0.20f;

            if (!board.CheckPosition(ActiveBlock))
            {
                ActiveBlock.MoveRight();
            }
        }
        else if (Input.GetKeyDown(KeyCode.P) && (Time.time > nextKeyRotateTimer)) //右回転
        {
            CanNotMove = true;

            UseSpin = true;
            SpinMini = false;
            LastSRS = 0;
            SpinActions = 7;

            ActiveBlock.RotateRight(ActiveBlock);

            //回転後の角度(MinoAngleAfter)の調整
            data.CalibrateMinoAngleAfter(ActiveBlock);

            nextKeyRotateTimer = Time.time + nextKeyRotateInterval;

            if (!board.CheckPosition(ActiveBlock))
            {
                if (!rotation.MinoSuperRotation(ActiveBlock))
                {
                    Debug.Log("回転禁止");

                    //4 Rotation
                    se.CallSE(4);
                }
                else
                {
                    Debug.Log("スーパーローテーション成功");

                    data.MinoAngleBefore = data.MinoAngleAfter;

                    SpinActions = rotation.SpinTerminal(UseSpin, ActiveBlock);

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
                data.MinoAngleBefore = data.MinoAngleAfter;

                SpinActions = rotation.SpinTerminal(UseSpin, ActiveBlock);

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

            UseSpin = true;
            SpinMini = false;
            LastSRS = 0;
            SpinActions = 7;

            ActiveBlock.Rotateleft(ActiveBlock);

            //回転後の角度(MinoAngleAfter)の調整
            data.CalibrateMinoAngleAfter(ActiveBlock);

            nextKeyRotateTimer = Time.time + nextKeyRotateInterval;

            if (!board.CheckPosition(ActiveBlock))
            {
                if (!rotation.MinoSuperRotation(ActiveBlock))
                {
                    Debug.Log("回転禁止");

                    //4 Rotation
                    se.CallSE(4);
                }
                else
                {
                    Debug.Log("スーパーローテーション成功");

                    data.MinoAngleBefore = data.MinoAngleAfter;

                    SpinActions = rotation.SpinTerminal(UseSpin, ActiveBlock);

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
                data.MinoAngleBefore = data.MinoAngleAfter;

                SpinActions = rotation.SpinTerminal(UseSpin, ActiveBlock);

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
            HardDrop = true;

            for (int i = 0; i < 30; i++)
            {
                ActiveBlock.MoveDown();

                if (!board.CheckPosition(ActiveBlock))
                {
                    break;
                }

                Debug.Log("ハードドロップで1マス以上動いた");
                UseSpin = false; //1マスでも落ちたらspin判定は消える。
                SpinActions = 7;
            }

            if (board.OverLimit(ActiveBlock))
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
        else if (Input.GetKeyDown(KeyCode.Return)) //ホールド
        {
            if (Hold == false)
            {
                se.CallSE(11);

                HoldBlock();
            }
        }
    }

    void Down()
    {
        if (Input.GetKey(KeyCode.S) && (Time.time > nextKeyDownTimer) && CanNotMove == false || (Time.time > NextdropTimer)) //下入力、または時間経過で落ちる時
        {
            ActiveBlock.MoveDown();

            nextKeyDownTimer = Time.time + nextKeyDownInterval;

            if (!board.CheckPosition(ActiveBlock))
            {
                if (board.OverLimit(ActiveBlock))
                {
                    //ゲームオーバー
                    gameOver = true;
                    sceneTransition.GameOver();
                }
                else
                {
                    ActiveBlock.MoveUp();
                    Bottom = true;
                    BottomMove();
                    keyReceptionTimer = Time.time + keyReceptionInterval;
                }
            }
            else
            {
                //下入力をした際はSEの3を鳴らす
                if (Time.time <= NextdropTimer)
                {
                    se.CallSE(3);
                }

                UseSpin = false;
                SpinActions = 7;
            }
            NextdropTimer = Time.time + dropInteaval;
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

        //初期化
        Hold = false;
        Bottom = false;
        UseSpin = false;
        nextKeyDownTimer = Time.time;
        nextKeyLeftRightTimer = Time.time;
        nextKeyRotateTimer = Time.time;
        keyReceptionTimer = Time.time;
        keyReceptionInterval = 1;

        ActiveBlock.MoveUp(); //ミノを正常な位置に戻す

        board.SaveBlockInGrid(ActiveBlock); //ActiveBlockをセーブ

        ClearRowHistory.Add(board.ClearAllRows()); //埋まっていれば削除し、ClearRowCountに消去ライン数を追加していく

        //Tspin判定(SpinActionsが4の時)
        if (SpinActions == 4)
        {
            //Tspinで1列も消去していない時
            //1列も消していなくてもTspin判定は行われる
            if (ClearRowHistory[ClearRowHistoryCount] == 0)
            {
                //1列も消していない時のSE
                if (HardDrop == true)
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
                if (SpinMini == true)
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
                if (SpinMini == true)
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
                if (SpinMini == true)
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
        else if (HardDrop == true)
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

        SpinMini = false;

        HardDrop = false;

        ClearRowHistoryCount++;

        data.Reset();

        MinoSpawn(FirstHold, Hold); //次のactiveBlockの生成

        if (!board.CheckPosition(ActiveBlock))
        {
            gameOver = true;

            sceneTransition.GameOver();
        }

        spawner.SpawnNextBlocks(Count, MinoOrder.ToArray()); //Next表示
    }

    void HoldBlock() //Holdした時の処理
    {
        Hold = true;
        ActiveBlock.transform.position = new Vector3(-3, 17, 0);
        NewHoldmino = ActiveBlock;
        board.DestroyBlock(ActiveBlock); //ActiveBlockを消す
        MinoSpawn(FirstHold, Hold); //新たなActiveBlockの表示
        spawner.SpawnHoldBlock(FirstHold, NewHoldmino); //Hold画面に表示

        if (FirstHold == true) //最初のHoldの時
        {
            spawner.SpawnNextBlocks(Count, MinoOrder.ToArray());
            FirstHold = false;
        }
        data.Reset();
    }

    void MinoSpawn(bool FirstHold, bool Hold) //ミノを呼び出す関数(順番決定も含む)
    {
        //Debug.Log("====this is MinoSpawn in GameManager====");

        if (FirstHold == true && Hold == true || Hold == false) //最初のホールドと、NEXT処理
        {
            if (Count % 7 == 0) //7の倍数の時
            {
                for (int j = 0; j <= 6; j++) //次のミノの順番を決める
                {
                    numbers.Add(j);
                }

                while (numbers.Count > 0)
                {
                    int index = Random.Range(0, numbers.Count);

                    int ransu = numbers[index];

                    MinoOrder.Add(ransu);

                    numbers.RemoveAt(index);
                }

                ActiveBlock = spawner.SpawnBlock(MinoOrder[Count]);
                Count++;
            }
            else
            {
                ActiveBlock = spawner.SpawnBlock(MinoOrder[Count]);
                Count++;
            }
        }
        else //2回目以降のホールド
        {
            ActiveBlock = spawner.HoldChange();
        }
    }

    /*void SpinEffect(int i)
    {

    }*/
}
