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
    // 変数宣言の文法上、実際にゲーム画面に表示するテキストと変数名が合致しない場合がある
    [SerializeField] private TextMeshProUGUI Ready;
    [SerializeField] private TextMeshProUGUI Go;

    [SerializeField] private TextMeshProUGUI BackToBack;

    [SerializeField] private TextMeshProUGUI One_Line_Clear;
    [SerializeField] private TextMeshProUGUI Two_Line_Clear;
    [SerializeField] private TextMeshProUGUI Three_Line_Clear;
    [SerializeField] private TextMeshProUGUI Tetris;
    [SerializeField] private TextMeshProUGUI Ispin;
    [SerializeField] private TextMeshProUGUI Ispin_Single;
    [SerializeField] private TextMeshProUGUI Ispin_Double;
    [SerializeField] private TextMeshProUGUI Ispin_Triple;
    [SerializeField] private TextMeshProUGUI Ispin_Quattro;
    [SerializeField] private TextMeshProUGUI Jspin;
    [SerializeField] private TextMeshProUGUI Jspin_Single;
    [SerializeField] private TextMeshProUGUI Jspin_Double;
    [SerializeField] private TextMeshProUGUI Jspin_Triple;
    [SerializeField] private TextMeshProUGUI Lspin;
    [SerializeField] private TextMeshProUGUI Lspin_Single;
    [SerializeField] private TextMeshProUGUI Lspin_Double;
    [SerializeField] private TextMeshProUGUI Lspin_Triple;
    [SerializeField] private TextMeshProUGUI Ospin;
    [SerializeField] private TextMeshProUGUI Ospin_Single;
    [SerializeField] private TextMeshProUGUI Ospin_Double;
    [SerializeField] private TextMeshProUGUI Ospin_Triple;
    [SerializeField] private TextMeshProUGUI Sspin;
    [SerializeField] private TextMeshProUGUI Sspin_Single;
    [SerializeField] private TextMeshProUGUI Sspin_Double;
    [SerializeField] private TextMeshProUGUI Sspin_Triple;
    [SerializeField] private TextMeshProUGUI Tspin;
    [SerializeField] private TextMeshProUGUI Tspin_Single;
    [SerializeField] private TextMeshProUGUI Tspin_Double;
    [SerializeField] private TextMeshProUGUI Tspin_Triple;
    [SerializeField] private TextMeshProUGUI Tspin_Mini;
    [SerializeField] private TextMeshProUGUI Tspin_Double_Mini;
    [SerializeField] private TextMeshProUGUI Zspin;
    [SerializeField] private TextMeshProUGUI Zspin_Single;
    [SerializeField] private TextMeshProUGUI Zspin_Double;
    [SerializeField] private TextMeshProUGUI Zspin_Triple;

    // 透明度 //
    int Alpha_0 = 0; // 0の時は透明
    int Alpha_1 = 1; // 1の時は不透明

    // 攻撃ラインの値 //
    int TwoLineClearAttack = 1;
    int ThreeLineClearAttack = 2;
    int TetrisAttack = 4;
    int TspinSingleAttack = 2;
    int TspinDoubleAttack = 4;
    int TspinTripleAttack = 6;
    int TspinMiniAttack = 0;
    int TspinDoubleMiniAttack = 1;
    int BackToBackBonus = 1;
    int RenBonus_0or1 = 0;
    int RenBonus_2or3 = 1;
    int RenBonus_4or5 = 2;
    int RenBonus_6or7 = 3;
    int RenBonus_8or9or10 = 4;
    int RenBonus_over11 = 5;

    // 干渉するスクリプト //
    GameStatus gameStatus;
    SpinCheck spinCheck;

    // インスタンス化 //
    void Awake()
    {
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

                        TextAnimation(One_Line_Clear);

                        break;

                    case 2:
                        gameStatus.Reset_BackToBack();

                        RenAttackLines();

                        AudioManager.Instance.PlaySound("Normal_Destroy");

                        TextAnimation(Two_Line_Clear);

                        break;

                    case 3:
                        gameStatus.Reset_BackToBack();

                        RenAttackLines();

                        AudioManager.Instance.PlaySound("Normal_Destroy");

                        TextAnimation(Three_Line_Clear);

                        break;

                    case 4:
                        if (gameStatus.backToBack == true) // BackToBack 条件を満たしている時
                        {
                            gameStatus.IncreaseAttackLines(BackToBackBonus);

                            BackToBackAnimation();
                        }
                        else
                        {
                            gameStatus.Set_BackToBack(); // BackToBack 判定を付与
                        }

                        RenAttackLines();

                        gameStatus.IncreaseAttackLines(TetrisAttack);

                        AudioManager.Instance.PlaySound("Tetris");

                        TextAnimation(Tetris);

                        break;
                }
                break;

            case "T-Spin":
                if (gameStatus.backToBack == true) // BackToBack 条件を満たしている時
                {
                    gameStatus.IncreaseAttackLines(BackToBackBonus);

                    BackToBackAnimation();
                }
                else
                {
                    gameStatus.Set_BackToBack(); // BackToBack 判定を付与
                }
                switch (_LineClearCount)
                {
                    case 0:
                        AudioManager.Instance.PlaySound("Normal_Drop");

                        TextAnimation(Tspin);

                        break;

                    case 1:
                        RenAttackLines();

                        gameStatus.IncreaseAttackLines(TspinSingleAttack);

                        AudioManager.Instance.PlaySound("Spin_Destroy");

                        TextAnimation(Tspin_Single);

                        break;

                    case 2:
                        RenAttackLines();

                        gameStatus.IncreaseAttackLines(TspinDoubleAttack);

                        AudioManager.Instance.PlaySound("Spin_Destroy");

                        TextAnimation(Tspin_Double);

                        break;

                    case 3:
                        RenAttackLines();

                        gameStatus.IncreaseAttackLines(TspinTripleAttack);

                        AudioManager.Instance.PlaySound("Spin_Destroy");

                        TextAnimation(Tspin_Triple);

                        break;
                }
                break;

            case "T-Spin Mini":
                if (gameStatus.backToBack == true) // BackToBack 条件を満たしている時
                {
                    gameStatus.IncreaseAttackLines(BackToBackBonus);

                    BackToBackAnimation();
                }
                else
                {
                    gameStatus.Set_BackToBack(); // BackToBack 判定を付与
                }
                switch (_LineClearCount)
                {
                    case 0:
                        AudioManager.Instance.PlaySound("Normal_Drop");
                        TextAnimation(Tspin_Mini);
                        break;

                    case 1:
                        RenAttackLines();

                        gameStatus.IncreaseAttackLines(TspinMiniAttack);

                        AudioManager.Instance.PlaySound("Spin_Destroy");

                        TextAnimation(Tspin_Mini);

                        break;

                    case 2:
                        RenAttackLines();

                        gameStatus.IncreaseAttackLines(TspinDoubleMiniAttack);

                        AudioManager.Instance.PlaySound("Spin_Destroy");

                        TextAnimation(Tspin_Double_Mini);

                        break;
                }
                break;
        }
        // Compleate判定
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

    private void BackToBackAnimation()
    {
        TextMeshProUGUI InstantiatedText = Instantiate(BackToBack, Canvas);

        TextMeshProUGUI SelectText_Text = InstantiatedText.GetComponent<TextMeshProUGUI>();

        TextFadeInAndOut(SelectText_Text); // 選ばれたテキストのフェードインとフェードアウトを行う
    }

    public void ReadyGoAnimation()
    {
        // フェードインとフェードアウトする時間 //
        float fadeInInterval = 0.3f;
        float fadeOutInterval = 0f;

        // テキストを表示させる時間 //
        float waitInterval_ready = 3f;
        float waitInterval_go = 2f;

        TextMeshProUGUI ready = Instantiate(Ready, Canvas);
        TextMeshProUGUI go = Instantiate(Go, Canvas);

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
}