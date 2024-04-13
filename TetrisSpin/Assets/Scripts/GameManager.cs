using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //シーン遷移のライブラリ


public class GameManager : MonoBehaviour
{
    Spawner spawner;
    Rotation rotation;
    SE se;

    SceneTransition sceneTransition;
    int Count = 0; //順番
    Block ActiveBlock; //生成されたブロック格納

    [SerializeField]
    private float dropInteaval; //次にブロックが落ちるまでのインターバル時間
    float NextdropTimer;  //次にブロックが落ちるまでの時間
    Board board;
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

    //回転の向き(0 = 右回転、1　= 左回転)
    int Turn;

    // 回転使用フラグ
    bool UseSpin = false;

    [SerializeField]
    int LastSRS; // 最後に行ったスーパーローテーション(SR)パターン(0-4)

    [SerializeField]
    int SpinActions = 7;
    public bool SpinMini = false;

    //ミノの向き(°)
    int MinoAngleBefore = 0;

    //ホールド機能に必要なもの
    bool FirstHold = true;
    bool Hold = false;
    Block NewHoldmino;

    //GhostBlickの機能に必要なもの
    Block_Ghost ActiveBlock_Ghost;

    //底についた判定
    bool Bottom = false;

    bool CanNotMove = false;

    List<int> ClearRowHistory = new List<int>();

    int ClearRowHistoryCount = 0;

    bool HardDrop = false;


    private void Start()
    {
        //スポナーオブジェクトをスポナー変数に格納する
        spawner = GameObject.FindObjectOfType<Spawner>();
        //ボードを変数に格納する
        board = GameObject.FindObjectOfType<Board>();

        rotation = GameObject.FindObjectOfType<Rotation>();

        ActiveBlock = GameObject.FindObjectOfType<Block>();

        se = GameObject.FindObjectOfType<SE>();

        sceneTransition = GameObject.FindObjectOfType<SceneTransition>();

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
                se.CallSE(1);
            }

            BottomMove();

            UseSpin = false;
            SpinActions = 7;
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
                se.CallSE(1);
            }

            BottomMove();

            UseSpin = false;
            SpinActions = 7;
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
                se.CallSE(1);
            }

            BottomMove();

            UseSpin = false;
            SpinActions = 7;
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
                se.CallSE(1);
            }

            BottomMove();

            UseSpin = false;
            SpinActions = 7;
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

            LastSRS = 0;

            UseSpin = true;
            SpinMini = false;
            SpinActions = 7;

            ActiveBlock.RotateRight(MinoAngleBefore, ActiveBlock);

            nextKeyRotateTimer = Time.time + nextKeyRotateInterval;

            Turn = 0; //右

            if (!board.CheckPosition(ActiveBlock))
            {
                if (!rotation.MinoSuperRotation(MinoAngleBefore, LastSRS, ActiveBlock))
                {
                    //回転できなかったら逆回転して無かったことにする。
                    Debug.Log("回転禁止");
                    ActiveBlock.Rotateleft(Mathf.RoundToInt(ActiveBlock.transform.rotation.eulerAngles.z), ActiveBlock);
                }
                else
                {
                    Debug.Log("スーパーローテーション成功");
                    MinoAngleBefore = Mathf.RoundToInt(ActiveBlock.transform.rotation.eulerAngles.z);

                    SpinActions = rotation.SpinTerminal(UseSpin, LastSRS, ActiveBlock);

                    if (SpinActions == 4)
                    {
                        se.CallSE(16);
                    }
                    else
                    {
                        se.CallSE(2);
                    }
                }
            }
            else
            {
                //回転できたらミノの向きを更新する。
                MinoAngleBefore = Mathf.RoundToInt(ActiveBlock.transform.rotation.eulerAngles.z);

                SpinActions = rotation.SpinTerminal(UseSpin, LastSRS, ActiveBlock);

                if (SpinActions == 4)
                {
                    se.CallSE(16);
                }
                else
                {
                    se.CallSE(2);
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
            SpinActions = 7;

            ActiveBlock.Rotateleft(MinoAngleBefore, ActiveBlock);

            nextKeyRotateTimer = Time.time + nextKeyRotateInterval;

            Turn = 1; //左

            if (!board.CheckPosition(ActiveBlock))
            {
                if (!rotation.MinoSuperRotation(MinoAngleBefore, LastSRS, ActiveBlock))
                {
                    //回転できなかったら逆回転して無かったことにする。
                    Debug.Log("回転禁止");
                    ActiveBlock.RotateRight(Mathf.RoundToInt(ActiveBlock.transform.rotation.eulerAngles.z), ActiveBlock);
                }
                else
                {
                    Debug.Log("スーパーローテーション成功");

                    MinoAngleBefore = Mathf.RoundToInt(ActiveBlock.transform.rotation.eulerAngles.z);

                    SpinActions = rotation.SpinTerminal(UseSpin, LastSRS, ActiveBlock);

                    if (SpinActions == 4)
                    {
                        se.CallSE(16);
                    }
                    else
                    {
                        se.CallSE(2);
                    }
                }
            }
            else
            {
                //回転できたらミノの向きを更新する。
                MinoAngleBefore = Mathf.RoundToInt(ActiveBlock.transform.rotation.eulerAngles.z);

                SpinActions = rotation.SpinTerminal(UseSpin, LastSRS, ActiveBlock);

                if (SpinActions == 4)
                {
                    se.CallSE(16);
                }
                else
                {
                    se.CallSE(2);
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
            NextdropTimer = Time.time + dropInteaval;

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
                UseSpin = false;
                SpinActions = 7;
            }
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
        MinoAngleBefore = 0;

        ActiveBlock.MoveUp(); //ミノを正常な位置に戻す

        board.SaveBlockInGrid(ActiveBlock); //ActiveBlockをセーブ

        ClearRowHistory.Add(board.ClearAllRows()); //埋まっていれば削除し、ClearRowCountに消去ライン数を追加していく

        if (ClearRowHistory[ClearRowHistoryCount] >= 1 && ClearRowHistory[ClearRowHistoryCount] <= 3)
        {
            if (SpinActions == 4)
            {
                se.CallSE(3);

                if (SpinMini == true) //TspinMini判定
                {
                    Debug.Log("TspinMiniでーす");
                }
            }
            else if (HardDrop == true)
            {
                se.CallSE(8);
                se.CallSE(17);

                HardDrop = false;
            }
            else
            {
                se.CallSE(17);
            }
        }
        else if (ClearRowHistory[ClearRowHistoryCount] == 4)
        {
            if (HardDrop == true)
            {
                se.CallSE(8);
                se.CallSE(20);

                HardDrop = false;
            }
            else
            {
                se.CallSE(20);
            }
        }
        else if (HardDrop == true)
        {
            se.CallSE(8);

            if (SpinMini == true) //TspinMini判定
            {
                Debug.Log("TspinMiniでーす");
            }

            HardDrop = false;
        }
        else
        {
            se.CallSE(9);

            if (SpinMini == true) //TspinMini判定
            {
                Debug.Log("TspinMiniでーす");
            }
        }

        SpinActions = 7;

        SpinMini = false;

        ClearRowHistoryCount++;

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

    void SpinEffect(int i)
    {

    }
}
