using UnityEngine;

///// ゲームマネージャー /////


// ↓このスクリプトで可能なこと↓ //

// ゲームの進行


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

    // 以下の関数で呼び出す
    // AudioManager.Instance.PlaySound("オーディオ名")


    // 干渉するスクリプト //
    Board board;
    GameStatus gameStatus;
    MainSceneText mainSceneText;
    Mino mino;
    SceneTransition sceneTransition;
    Spawner spawner;
    SpinCheck spinCheck;
    Timer timer;


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

        RockDown(); // ロックダウン判定

        PlayerInput(); // プレイヤーが制御できるコマンド

        AutoDown(); // 自動落下

        if (!board.CheckPosition(spawner.activeMino))
        {
            Debug.LogError("[GameManager Update()] ゲームボードからミノがはみ出した。または、ブロックに重なった。");
        }

        if (!board.CheckPosition(spawner.ghostMino))
        {
            Debug.LogError("[GameManager Update()] ゲームボードからゴーストミノがはみ出した。または、ブロックに重なった。");
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
            }

            IncreaseBottomMoveCount(); // BottomMoveCountの値を1増加
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
            }

            IncreaseBottomMoveCount();
        }

        // 連続右入力の解除
        else if (Input.GetKeyUp(KeyCode.D))
        {
            timer.ContinuousLRKey = false; // キーの連続入力でない
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
            }

            IncreaseBottomMoveCount();
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
            }

            IncreaseBottomMoveCount();
        }

        // 連続右入力の解除
        else if (Input.GetKeyUp(KeyCode.A))
        {
            timer.ContinuousLRKey = false; // キーの連続入力でない
        }

        // 下入力された時
        else if (Input.GetKey(KeyCode.S) && (Time.time > timer.NextKeyDownTimer)) // Sキーに割り当て
        {
            timer.UpdateDownTimer();

            spawner.activeMino.MoveDown();

            if (!board.CheckPosition(spawner.activeMino))
            {
                spawner.activeMino.MoveUp();
            }
            else
            {
                AudioManager.Instance.PlaySound("Move_Down");

                gameStatus.Reset_LastSRS();
            }
        }

        // 右回転入力された時
        else if (Input.GetKeyDown(KeyCode.P) && (Time.time > timer.NextKeyRotateTimer)) // Pキーに割り当て
        {
            timer.UpdateRotateTimer();

            gameStatus.Reset_LastSRS();

            spawner.activeMino.RotateRight();

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

                    SuccessRotateAction(); // 回転が成功した時の処理を実行
                }
            }
            else // 通常回転が成功した時
            {
                SuccessRotateAction();
            }

            IncreaseBottomMoveCount();
        }

        // 左回転入力された時
        else if (Input.GetKeyDown(KeyCode.L) && (Time.time > timer.NextKeyRotateTimer)) // Lキーに割り当て
        {
            timer.UpdateRotateTimer();

            gameStatus.Reset_LastSRS();

            spawner.activeMino.Rotateleft();

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

                    SuccessRotateAction();
                }
            }
            else
            {
                SuccessRotateAction();
            }

            IncreaseBottomMoveCount();
        }

        // ハードドロップ入力された時
        else if (Input.GetKeyDown(KeyCode.Space)) // Spaceキーに割り当て
        {
            AudioManager.Instance.PlaySound("Hard_Drop");

            int height = 20; // ゲームボードの高さの値

            for (int i = 0; i < height; i++) // heightの値分繰り返す
            {
                spawner.activeMino.MoveDown();

                if (!board.CheckPosition(spawner.activeMino)) // 底にぶつかった時
                {
                    spawner.activeMino.MoveUp(); // ミノを正常な位置に戻す

                    break; // For文を抜ける
                }

                gameStatus.Reset_LastSRS(); // 1マスでも下に移動した時、LastSRSの値を0にする
            }

            Reset_RockDown(); // RockDownに関する変数のリセット

            SetMinoFixed(); // 底に着いたときの処理
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

            spawner.activeMino.MoveDown();

            if (!board.CheckPosition(spawner.activeMino))
            {
                spawner.activeMino.MoveUp(); // ミノを正常な位置に戻す
            }
            else
            {
                gameStatus.Reset_LastSRS();
            }
        }
    }

    // ロックダウンの処理をする関数 //
    private void RockDown()
    {
        int newBottomBlockPosition_y = board.CheckActiveMinoBottomBlockPosition_y(spawner.activeMino); // ActiveMino の1番下のブロックのy座標を取得

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
        }
    }

    // BottomMoveCountを進める関数 //
    private void IncreaseBottomMoveCount()
    {
        if (BottomMoveCount < BottomMoveCountLimit) // BottomMoveCount が15未満の時
        {
            BottomMoveCount++;
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
        if (board.CheckGameOver(spawner.activeMino)) // ミノの設置時にゲームオーバーの条件を満たした場合
        {
            gameStatus.Set_GameOver();

            sceneTransition.GameOver();

            return;
        }

        // 各種変数のリセット
        Reset_RockDown();
        UseHold = false;
        timer.ResetTimer();

        board.SaveBlockInGrid(spawner.activeMino); //mino.activeMinoをセーブ

        gameStatus.AddLineClearCountHistory(board.ClearAllRows(), MinoPutNumber); // 横列が埋まっていれば消去し、消去数を記録する

        mainSceneText.TextDisplay(gameStatus.lineClearCountHistory[MinoPutNumber]); // 消去数、Spinに対応したテキストを表示し、それに対応したSEも鳴らす

        // 各種変数のリセット
        gameStatus.Reset_LastSRS();
        spinCheck.Reset_SpinTypeName();
        gameStatus.Reset_Angle();

        // Numberを1進める
        MinoPutNumber++;
        MinoPopNumber++;

        spawner.CreateNewActiveMino(MinoPopNumber);

        spawner.CreateNewNextMinos(MinoPopNumber);

        if (!board.CheckPosition(spawner.activeMino)) // ミノを生成した際に、ブロックと重なってしまった場合
        {
            gameStatus.Set_GameOver();

            sceneTransition.GameOver();

            return;
        }
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
