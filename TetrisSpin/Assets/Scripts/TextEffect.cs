using UnityEngine;
using TMPro;
using DG.Tweening;

///// ゲーム画面のテキストに関するスクリプト /////


// ↓このスクリプトで可能なこと↓ //

// テキストの表示
// テキストのアニメーション
// 攻撃ラインの値を計算

public class TextEffect : MonoBehaviour
{
    // Canvas //
    [SerializeField] private RectTransform Canvas;

    // 表示できるテキスト //
    [SerializeField] private TextMeshProUGUI Ready_Text;
    [SerializeField] private TextMeshProUGUI Go_Text;

    [SerializeField] private TextMeshProUGUI BackToBack_Text;
    [SerializeField] private TextMeshProUGUI PerfectClear_Text;
    [SerializeField] private TextMeshProUGUI Rens_Text;

    [SerializeField] private TextMeshProUGUI OneLineClear_Text;
    [SerializeField] private TextMeshProUGUI TwoLineClear_Text;
    [SerializeField] private TextMeshProUGUI ThreeLineClear_Text;
    [SerializeField] private TextMeshProUGUI Tetris_Text;
    [SerializeField] private TextMeshProUGUI Ispin_Text;
    [SerializeField] private TextMeshProUGUI IspinSingle_Text;
    [SerializeField] private TextMeshProUGUI IspinDouble_Text;
    [SerializeField] private TextMeshProUGUI IspinTriple_Text;
    [SerializeField] private TextMeshProUGUI IspinQuattro_Text;
    [SerializeField] private TextMeshProUGUI IspinMini_Text;
    [SerializeField] private TextMeshProUGUI Jspin_Text;
    [SerializeField] private TextMeshProUGUI JspinSingle_Text;
    [SerializeField] private TextMeshProUGUI JspinDouble_Text;
    [SerializeField] private TextMeshProUGUI JspinTriple_Text;
    [SerializeField] private TextMeshProUGUI Lspin_Text;
    [SerializeField] private TextMeshProUGUI LspinSingle_Text;
    [SerializeField] private TextMeshProUGUI LspinDouble_Text;
    [SerializeField] private TextMeshProUGUI LspinTriple_Text;
    [SerializeField] private TextMeshProUGUI Ospin_Text;
    [SerializeField] private TextMeshProUGUI OspinSingle_Text;
    [SerializeField] private TextMeshProUGUI OspinDouble_Text;
    [SerializeField] private TextMeshProUGUI OspinTriple_Text;
    [SerializeField] private TextMeshProUGUI Sspin_Text;
    [SerializeField] private TextMeshProUGUI SspinSingle_Text;
    [SerializeField] private TextMeshProUGUI SspinDouble_Text;
    [SerializeField] private TextMeshProUGUI SspinTriple_Text;
    [SerializeField] private TextMeshProUGUI Tspin_Text;
    [SerializeField] private TextMeshProUGUI TspinSingle_Text;
    [SerializeField] private TextMeshProUGUI TspinDouble_Text;
    [SerializeField] private TextMeshProUGUI TspinTriple_Text;
    [SerializeField] private TextMeshProUGUI TspinMini_Text;
    [SerializeField] private TextMeshProUGUI TspinDoubleMini_Text;
    [SerializeField] private TextMeshProUGUI Zspin_Text;
    [SerializeField] private TextMeshProUGUI ZspinSingle_Text;
    [SerializeField] private TextMeshProUGUI ZspinDouble_Text;
    [SerializeField] private TextMeshProUGUI ZspinTriple_Text;


    // 透明度 //
    int Alpha_0 = 0; // 0の時は透明
    int Alpha_1 = 1; // 1の時は不透明

    // 攻撃ラインの値 //
    int TwoLineClearAttack = 1;
    int ThreeLineClearAttack = 2;
    int TetrisAttack = 4;
    int IspinSingleAttack = 2;
    int IspinDoubleAttack = 4;
    int IspinTripleAttack = 6;
    int IspinQuattroAttack = 8;
    int IspinMiniAttack = 1;
    int TspinSingleAttack = 2;
    int TspinDoubleAttack = 4;
    int TspinTripleAttack = 6;
    int TspinMiniAttack = 0;
    int TspinDoubleMiniAttack = 1;
    int BackToBackBonus = 1;
    int PerfectClearBonus = 10;
    int RenBonus_0or1 = 0;
    int RenBonus_2or3 = 1;
    int RenBonus_4or5 = 2;
    int RenBonus_6or7 = 3;
    int RenBonus_8or9or10 = 4;
    int RenBonus_over11 = 5;

    // 干渉するスクリプト //
    Board board;
    GameStatus gameStatus;
    SpinCheck spinCheck;

    // インスタンス化 //
    void Awake()
    {
        board = FindObjectOfType<Board>();
        gameStatus = FindObjectOfType<GameStatus>();
        spinCheck = FindObjectOfType<SpinCheck>();
    }

    // 表示するテキストを判別して、実際に表示する関数 //
    public void TextDisplay(int _LineClearCount)
    {
        switch (spinCheck.spinTypeName)
        {
            case "None": // Spin判定がない時
                switch (_LineClearCount) // 消去数で表示するテキストが変わる
                {
                    case 0:
                        gameStatus.Reset_Ren();
                        break;

                    case 1:
                        gameStatus.Reset_BackToBack();

                        RenAttackLines();

                        AudioManager.Instance.PlaySound("Normal_Destroy");

                        TextAnimation(OneLineClear_Text);

                        break;

                    case 2:
                        gameStatus.Reset_BackToBack();

                        gameStatus.IncreaseAttackLines(TwoLineClearAttack);

                        RenAttackLines();

                        AudioManager.Instance.PlaySound("Normal_Destroy");

                        TextAnimation(TwoLineClear_Text);

                        break;

                    case 3:
                        gameStatus.Reset_BackToBack();

                        gameStatus.IncreaseAttackLines(ThreeLineClearAttack);

                        RenAttackLines();

                        AudioManager.Instance.PlaySound("Normal_Destroy");

                        TextAnimation(ThreeLineClear_Text);

                        break;

                    case 4:
                        CheckBackToBack();

                        RenAttackLines();

                        gameStatus.IncreaseAttackLines(TetrisAttack);

                        AudioManager.Instance.PlaySound("Tetris");

                        TextAnimation(Tetris_Text);

                        break;
                }
                break;

            case "I-Spin Mini":
                CheckBackToBack();

                switch (_LineClearCount)
                {
                    case 0:
                        AudioManager.Instance.PlaySound("Normal_Drop");

                        TextAnimation(IspinMini_Text);

                        break;

                    case 1:
                        RenAttackLines();

                        gameStatus.IncreaseAttackLines(IspinMiniAttack);

                        AudioManager.Instance.PlaySound("Spin_Destroy");

                        TextAnimation(IspinMini_Text);

                        break;
                }
                break;

            case "I-Spin":
                CheckBackToBack();

                switch (_LineClearCount)
                {
                    case 0:
                        AudioManager.Instance.PlaySound("Normal_Drop");

                        TextAnimation(Ispin_Text);

                        break;

                    case 1:
                        RenAttackLines();

                        gameStatus.IncreaseAttackLines(IspinSingleAttack);

                        AudioManager.Instance.PlaySound("Spin_Destroy");

                        TextAnimation(IspinSingle_Text);

                        break;

                    case 2:
                        RenAttackLines();

                        gameStatus.IncreaseAttackLines(IspinDoubleAttack);

                        AudioManager.Instance.PlaySound("Spin_Destroy");

                        TextAnimation(IspinDouble_Text);

                        break;

                    case 3:
                        RenAttackLines();

                        gameStatus.IncreaseAttackLines(IspinTripleAttack);

                        AudioManager.Instance.PlaySound("Spin_Destroy");

                        TextAnimation(IspinTriple_Text);

                        break;
                    case 4:
                        RenAttackLines();

                        gameStatus.IncreaseAttackLines(IspinQuattroAttack);

                        AudioManager.Instance.PlaySound("Spin_Destroy");

                        TextAnimation(IspinQuattro_Text);

                        break;
                }
                break;

            case "T-Spin":
                CheckBackToBack();

                switch (_LineClearCount)
                {
                    case 0:
                        AudioManager.Instance.PlaySound("Normal_Drop");

                        TextAnimation(Tspin_Text);

                        break;

                    case 1:
                        RenAttackLines();

                        gameStatus.IncreaseAttackLines(TspinSingleAttack);

                        AudioManager.Instance.PlaySound("Spin_Destroy");

                        TextAnimation(TspinSingle_Text);

                        break;

                    case 2:
                        RenAttackLines();

                        gameStatus.IncreaseAttackLines(TspinDoubleAttack);

                        AudioManager.Instance.PlaySound("Spin_Destroy");

                        TextAnimation(TspinDouble_Text);

                        break;

                    case 3:
                        RenAttackLines();

                        gameStatus.IncreaseAttackLines(TspinTripleAttack);

                        AudioManager.Instance.PlaySound("Spin_Destroy");

                        TextAnimation(TspinTriple_Text);

                        break;
                }
                break;

            case "T-Spin Mini":
                CheckBackToBack();

                switch (_LineClearCount)
                {
                    case 0:
                        AudioManager.Instance.PlaySound("Normal_Drop");

                        TextAnimation(TspinMini_Text);

                        break;

                    case 1:
                        RenAttackLines();

                        gameStatus.IncreaseAttackLines(TspinMiniAttack);

                        AudioManager.Instance.PlaySound("Spin_Destroy");

                        TextAnimation(TspinMini_Text);

                        break;

                    case 2:
                        RenAttackLines();

                        gameStatus.IncreaseAttackLines(TspinDoubleMiniAttack);

                        AudioManager.Instance.PlaySound("Spin_Destroy");

                        TextAnimation(TspinDoubleMini_Text);

                        break;
                }
                break;
        }

        // PerfectClearか判定する
        if (board.CheckPerfectClear())
        {
            gameStatus.IncreaseAttackLines(PerfectClearBonus);

            // AudioManager.Instance.PlaySound("");

            PerfectClearAnimation();
        }
    }

    // BackToBackの判定をチェックする関数
    private void CheckBackToBack()
    {
        if (gameStatus.backToBack == true) // BackToBack 条件をすでに満たしている時
        {
            gameStatus.IncreaseAttackLines(BackToBackBonus);

            BackToBackAnimation();
        }
        else
        {
            gameStatus.Set_BackToBack(); // BackToBack 判定を付与
        }
    }

    // Renの攻撃ラインを計算する関数 //
    private void RenAttackLines()
    {
        gameStatus.IncreaseRen(); // Renのカウントを1あげる

        switch (gameStatus.ren)
        {
            case 0:
            case 1:
                gameStatus.IncreaseAttackLines(RenBonus_0or1);
                break;

            case 2:
            case 3:
                gameStatus.IncreaseAttackLines(RenBonus_2or3);
                break;

            case 4:
            case 5:
                gameStatus.IncreaseAttackLines(RenBonus_4or5);
                break;

            case 6:
            case 7:
                gameStatus.IncreaseAttackLines(RenBonus_6or7);
                break;

            case 8:
            case 9:
            case 10:
                gameStatus.IncreaseAttackLines(RenBonus_8or9or10);
                break;

            default:
                gameStatus.IncreaseAttackLines(RenBonus_over11);
                break;
        }
    }

    // テキストのアニメーションを行う関数を呼ぶ関数 //
    private void TextAnimation(TextMeshProUGUI _SelectText)
    {
        TextMeshProUGUI InstantiatedText = Instantiate(_SelectText, Canvas);

        // 選ばれたテキストのテキストコンポーネントとトランスフォームコンポーネントを取得
        TextMeshProUGUI SelectText_Text = InstantiatedText.GetComponent<TextMeshProUGUI>();
        Transform SelectText_Transform = InstantiatedText.GetComponent<Transform>();

        TextFadeInAndOut(SelectText_Text); // 選ばれたテキストのフェードインとフェードアウトを行う

        TextMove(SelectText_Transform); // 選ばれたテキストの移動アニメーションを行う
    }

    //選ばれたテキストのフェードインとフェードアウトを行う関数
    private void TextFadeInAndOut(TextMeshProUGUI selectText)
    {
        // フェードインとフェードアウトする時間 //
        float fadeInInterval = 0.3f;
        float fadeOutInterval = 1f;

        // テキストを表示させる時間 //
        float waitInterval = 2f;

        // Sequenceのインスタンス化
        var sequence = DOTween.Sequence();

        // 0.3秒かけてアルファ値を1(=不透明)に変化させる
        // その後、2秒表示
        // 最後に、1秒かけてアルファ値を0(=透明)に変化させる
        sequence
            .Append(selectText.DOFade(Alpha_1, fadeInInterval))
            .AppendInterval(waitInterval)
            .Append(selectText.DOFade(Alpha_0, fadeOutInterval));
    }


    //選ばれたテキストの移動アニメーションを行う関数
    private void TextMove(Transform selectText)
    {
        // テキストを移動させる時間 //
        float moveInterval_x = 2f; // x軸
        float moveInterval_y = 1.2f; // y軸

        // テキストの移動量 //
        float moveDistance = 600f;

        // selectTextの各座標を格納する変数を宣言
        float selectText_x = Mathf.RoundToInt(selectText.transform.position.x);
        float selectText_y = Mathf.RoundToInt(selectText.transform.position.y);
        float selectText_z = Mathf.RoundToInt(selectText.transform.position.z);

        // Sequenceのインスタンス化
        var sequence = DOTween.Sequence();

        // 現在位置から3秒で、Y座標を600移動する
        // 始点と終点の繋ぎ方はOutSineで設定
        // その後、現在位置から1秒で、X座標を-600移動する(移動中にフェードアウトする)
        // 始点と終点の繋ぎ方はInQuintで設定
        sequence
            .Append(selectText.transform.DOMoveY(selectText_y + moveDistance, moveInterval_x).SetEase(Ease.OutSine))
            .Append(selectText.transform.DOMoveX(selectText_x - moveDistance, moveInterval_y).SetEase(Ease.InQuint))
            //.Join(selectText.transform.DOScale(new Vector3(0, 1f, 0), moveInterval_y))
            .OnComplete(() =>
            {
                // アニメーションが完了したら元の位置に戻す
                selectText.transform.position = new Vector3(selectText_x, selectText_y, selectText_z);
            });
    }

    // BackToBackの表示をする関数を呼ぶ関数 //
    private void BackToBackAnimation()
    {
        TextMeshProUGUI InstantiatedText = Instantiate(BackToBack_Text, Canvas);

        TextMeshProUGUI SelectText_Text = InstantiatedText.GetComponent<TextMeshProUGUI>();

        TextFadeInAndOut(SelectText_Text); // 選ばれたテキストのフェードインとフェードアウトを行う
    }

    // PerfectClearの表示をする関数を呼ぶ関数 //
    private void PerfectClearAnimation()
    {
        TextMeshProUGUI InstantiatedText = Instantiate(PerfectClear_Text, Canvas);

        TextMeshProUGUI SelectText_Text = InstantiatedText.GetComponent<TextMeshProUGUI>();

        TextFadeInAndOut(SelectText_Text); // 選ばれたテキストのフェードインとフェードアウトを行う
    }

    // Ready Go の表示をする関数 //
    public void ReadyGoAnimation()
    {
        // フェードインとフェードアウトする時間 //
        float fadeInInterval = 0.3f;
        float fadeOutInterval = 0f;

        // テキストを表示させる時間 //
        float waitInterval_ready = 3f;
        float waitInterval_go = 2f;

        TextMeshProUGUI ready = Instantiate(Ready_Text, Canvas);
        TextMeshProUGUI go = Instantiate(Go_Text, Canvas);

        Sequence sequence_ready = DOTween.Sequence();
        Sequence sequence_go = DOTween.Sequence(); // Sequenceの作成

        sequence_ready
            .Append(ready.DOFade(Alpha_1, fadeInInterval))
            .AppendInterval(waitInterval_ready)
            .Append(ready.DOFade(Alpha_0, fadeOutInterval))
            .OnComplete(() =>
            {
                // Readyアニメーションが完了したらGoアニメーションを開始
                sequence_go
                    .Append(go.DOFade(Alpha_1, fadeInInterval))
                    // .Join(go.DOScale(Vector3.one, 0.2f))
                    .AppendInterval(waitInterval_go)
                    .Append(go.DOFade(Alpha_0, fadeOutInterval));
            });

        // // フェードイン＆拡大
        // sequence.Append(textTransform.DOScale(Vector3.one, 0.2f)); // 0.2秒で拡大
        // sequence.Join(textTransform.DOFade(1, 0.2f)); // 同時にフェードイン

        // // フェードアウト
        // sequence.AppendInterval(1f); // 1秒待機
        // sequence.Append(textTransform.DOFade(0, 0.5f)); // 0.5秒でフェードアウト
    }

    // アニメーションを停止させる関数 //
    public void StopAnimation()
    {
        DOTween.KillAll();
    }
}