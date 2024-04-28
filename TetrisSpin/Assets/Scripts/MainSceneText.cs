using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

// ゲーム画面のテキストに関するスクリプト //

public class MainSceneText : MonoBehaviour
{
    // 干渉するスクリプト //
    SpinCheck spinCheck;

    // Canvas //
    [SerializeField] private RectTransform Canvas;

    // 表示できるテキスト //
    // 変数宣言の文法上、実際にゲーム画面に表示するテキストと変数名が合致しない場合がある
    [SerializeField] private Text One_Line_Clear;
    [SerializeField] private Text Two_Line_Clear;
    [SerializeField] private Text Three_Line_Clear;
    [SerializeField] private Text Tetris;
    [SerializeField] private Text Ispin;
    [SerializeField] private Text Ispin_Single;
    [SerializeField] private Text Ispin_Double;
    [SerializeField] private Text Ispin_Triple;
    [SerializeField] private Text Ispin_Quattro;
    [SerializeField] private Text Jspin;
    [SerializeField] private Text Jspin_Single;
    [SerializeField] private Text Jspin_Double;
    [SerializeField] private Text Jspin_Triple;
    [SerializeField] private Text Lspin;
    [SerializeField] private Text Lspin_Single;
    [SerializeField] private Text Lspin_Double;
    [SerializeField] private Text Lspin_Triple;
    [SerializeField] private Text Ospin;
    [SerializeField] private Text Ospin_Single;
    [SerializeField] private Text Ospin_Double;
    [SerializeField] private Text Ospin_Triple;
    [SerializeField] private Text Sspin;
    [SerializeField] private Text Sspin_Single;
    [SerializeField] private Text Sspin_Double;
    [SerializeField] private Text Sspin_Triple;
    [SerializeField] private Text Tspin;
    [SerializeField] private Text Tspin_Single;
    [SerializeField] private Text Tspin_Double;
    [SerializeField] private Text Tspin_Triple;
    [SerializeField] private Text Tspin_Mini;
    [SerializeField] private Text Tspin_Double_Mini;
    [SerializeField] private Text Zspin;
    [SerializeField] private Text Zspin_Single;
    [SerializeField] private Text Zspin_Double;
    [SerializeField] private Text Zspin_Triple;


    // private string One_Line_Clear = "One_Line_Clear";
    // private string Two_Line_Clear = "Two_Line_Clear";
    // private string Three_Line_Clear = "Three_Line_Clear";
    // private string Tetris = "Tetris";
    // private string Ispin = "Ispin";
    // private string Ispin_Single = "Ispin_Single";
    // private string Ispin_Double = "Ispin_Double";
    // private string Ispin_Triple = "Ispin_Triple";
    // private string Ispin_Quattro = "Ispin_Quattro";
    // private string Jspin = "Jspin";
    // private string Jspin_Single = "Jspin_Single";
    // private string Jspin_Double = "Jspin_Double";
    // private string Jspin_Triple = "Jspin_Triple";
    // private string Lspin = "Lspin";
    // private string Lspin_Single = "Lspin_Single";
    // private string Lspin_Double = "Lspin_Double";
    // private string Lspin_Triple = "Lspin_Triple";
    // private string Ospin = "Ospin";
    // private string Ospin_Single = "Ospin_Single";
    // private string Ospin_Double = "Ospin_Double";
    // private string Ospin_Triple = "Ospin_Triple";
    // private string Sspin = "Sspin";
    // private string Sspin_Single = "Sspin_Single";
    // private string Sspin_Double = "Sspin_Double";
    // private string Sspin_Triple = "Sspin_Triple";
    // private string Tspin = "Tspin";
    // private string Tspin_Single = "Tspin_Single";
    // private string Tspin_Double = "Tspin_Double";
    // private string Tspin_Triple = "Tspin_Triple";
    // private string Tspin_Mini = "Tspin_Mini";
    // private string Tspin_Double_Mini = "Tspin_Double_Mini";
    // private string Zspin = "Zspin";
    // private string Zspin_Single = "Zspin_Single";
    // private string Zspin_Double = "Zspin_Double";
    // private string Zspin_Triple = "Zspin_Triple";


    // フェードインとフェードアウトする時間 //
    float FadeInInterval = 0.2f;
    float FadeOutInterval = 1f;

    // テキストを表示させる時間 //
    float waitInterval = 2f;

    // 透明度 //
    int alpha_0 = 0; // 0の時は透明
    int alpha_1 = 1; // 1の時は不透明

    // テキストを移動させる時間 //
    float moveInterval_x = 2f; // x軸
    float moveInterval_y = 1.2f; // y軸

    // テキストの移動量 //
    float moveDistance = 600f;

    // アニメーションの合計時間 //
    float waitTime = 3.2f;

    // インスタンス化 //
    void Awake()
    {
        spinCheck = FindObjectOfType<SpinCheck>();

        // Dictionary<string, Text> textDictionary = new Dictionary<string, Text> // String と Text の Dictionary を作成
        // {
        //     { "One_Line_Clear", One_Line_Clear },
        //     { "Two_Line_Clear", Two_Line_Clear },
        //     { "Three_Line_Clear", Three_Line_Clear },
        //     { "Tetris", Tetris },
        //     { "Ispin", Ispin },
        //     { "Ispin_Single", Ispin_Single },
        //     { "Ispin_Double", Ispin_Double },
        //     { "Ispin_Triple", Ispin_Triple },
        //     { "Ispin_Quattro", Ispin_Quattro },
        //     { "Jspin", Jspin },
        //     { "Jspin_Single", Jspin_Single },
        //     { "Jspin_Double", Jspin_Double },
        //     { "Jspin_Triple", Jspin_Triple },
        //     { "Lspin", Lspin },
        //     { "Lspin_Single", Lspin_Single },
        //     { "Lspin_Double", Lspin_Double },
        //     { "Lspin_Triple", Lspin_Triple },
        //     { "Ospin", Ospin },
        //     { "Ospin_Single", Ospin_Single },
        //     { "Ospin_Double", Ospin_Double },
        //     { "Ospin_Triple", Ospin_Triple },
        //     { "Sspin", Sspin },
        //     { "Sspin_Single", Sspin_Single },
        //     { "Sspin_Double", Sspin_Double },
        //     { "Sspin_Triple", Sspin_Triple },
        //     { "Tspin", Tspin },
        //     { "Tspin_Single", Tspin_Single },
        //     { "Tspin_Double", Tspin_Double },
        //     { "Tspin_Triple", Tspin_Triple },
        //     { "Tspin_Mini", Tspin_Mini },
        //     { "Tspin_Double_Mini", Tspin_Double_Mini },
        //     { "Zspin", Zspin },
        //     { "Zspin_Single", Zspin_Single },
        //     { "Zspin_Double", Zspin_Double },
        //     { "Zspin_Triple", Zspin_Triple }
        // };
    }

    // 表示するテキストを判別して、実際に表示する関数 //
    public void TextDisplay(int _LineClearCount)
    {
        switch (spinCheck.spinTypeName)
        {
            case "None": // Spin判定がない時
                switch (_LineClearCount) // 消去数で表示するテキストが変わる
                {
                    case 1:
                        Debug.Log("1ライン消去");
                        One_Line_Clear.gameObject.SetActive(true);
                        TextAnimation(One_Line_Clear);
                        break;
                    case 2:
                        TextAnimation(Two_Line_Clear);
                        break;
                    case 3:
                        TextAnimation(Three_Line_Clear);
                        break;
                    case 4:
                        TextAnimation(Tetris);
                        break;
                }
                break;

            case "T-Spin":
                switch (_LineClearCount)
                {
                    case 0:
                        TextAnimation(Tspin);
                        break;
                    case 1:
                        TextAnimation(Tspin_Single);
                        break;
                    case 2:
                        Tspin_Double.gameObject.SetActive(true);
                        TextAnimation(Tspin_Double);
                        break;
                    case 3:
                        TextAnimation(Tspin_Triple);
                        break;
                }
                break;
            case "T-Spin Mini":
                switch (_LineClearCount)
                {
                    case 0:
                        TextAnimation(Tspin_Mini);
                        break;
                    case 1:
                        TextAnimation(Tspin_Mini);
                        break;
                    case 2:
                        TextAnimation(Tspin_Double_Mini);
                        break;
                }
                break;
        }
    }


    //     if (_LineClearCount == 0)
    //     {
    //         if ()
    //         {

    //         }
    //     }
    //     else if (_LineClearCount == 1)
    //     {
    //         switch (spinCheck.spinTypeName)
    //         {
    //             case "None":

    //                 break;
    //         }
    //     }

    //     //textと合致するGameObjectのTextコンポーネントを取得
    //     if (spinCheck.spinTypeName == calculate.one_Line_Clear)
    //     {
    //         //one_Line_Clear_Arrayの数だけ繰り返す
    //         for (int count = 0; count < one_Line_Clears.Length; count++)
    //         {
    //             //count番目のone_Line_Clearsのテキストが未使用なら
    //             if (one_Line_Clears[count].gameObject.activeSelf == false)
    //             {
    //                 //アニメーションを行う
    //                 //コルーチンで演出が終わるまで待機し、完全にアニメーションが終了したら未使用に戻す
    //                 TextAnimation(one_Line_Clears[count]));

    //                 //このfor文を抜ける
    //                 break;
    //             }
    //         }
    //         //↓以下同文
    //     }
    //     else if (text == calculate.two_Line_Clear)
    //     {
    //         for (int count = 0; count < two_Line_Clears.Length; count++)
    //         {
    //             if (two_Line_Clears[count].gameObject.activeSelf == false)
    //             {
    //                 TextAnimation(two_Line_Clears[count]));

    //                 break;
    //             }
    //         }
    //     }
    //     else if (text == calculate.three_Line_Clear)
    //     {
    //         for (int count = 0; count < three_Line_Clears.Length; count++)
    //         {
    //             if (three_Line_Clears[count].gameObject.activeSelf == false)
    //             {
    //                 TextAnimation(three_Line_Clears[count]));

    //                 break;
    //             }
    //         }
    //     }
    //     else if (text == calculate.Tetris)
    //     {
    //         for (int count = 0; count < Tetrises.Length; count++)
    //         {
    //             if (Tetrises[count].gameObject.activeSelf == false)
    //             {
    //                 TextAnimation(Tetrises[count]));

    //                 break;
    //             }
    //         }
    //     }
    //     else if (text == calculate.Tspin)
    //     {
    //         for (int count = 0; count < Tspins.Length; count++)
    //         {
    //             if (Tspins[count].gameObject.activeSelf == false)
    //             {
    //                 TextAnimation(Tspins[count]));

    //                 break;
    //             }
    //         }
    //     }
    //     else if (text == calculate.Tspin_Single)
    //     {
    //         for (int count = 0; count < Tspin_Singles.Length; count++)
    //         {
    //             if (Tspin_Singles[count].gameObject.activeSelf == false)
    //             {
    //                 TextAnimation(Tspin_Singles[count]));

    //                 break;
    //             }
    //         }
    //     }
    //     else if (text == calculate.Tspin_Double)
    //     {
    //         for (int count = 0; count < Tspin_Doubles.Length; count++)
    //         {
    //             if (Tspin_Doubles[count].gameObject.activeSelf == false)
    //             {
    //                 TextAnimation(Tspin_Doubles[count]));

    //                 break;
    //             }
    //         }
    //     }
    //     else if (text == calculate.Tspin_Triple)
    //     {
    //         for (int count = 0; count < Tspin_Triples.Length; count++)
    //         {
    //             if (Tspin_Triples[count].gameObject.activeSelf == false)
    //             {
    //                 TextAnimation(Tspin_Triples[count]));

    //                 break;
    //             }
    //         }
    //     }
    //     else if (text == calculate.Tspin_Mini)
    //     {
    //         for (int count = 0; count < Tspin_Minis.Length; count++)
    //         {
    //             if (Tspin_Minis[count].gameObject.activeSelf == false)
    //             {
    //                 TextAnimation(Tspin_Minis[count]));

    //                 break;
    //             }
    //         }
    //     }
    //     else if (text == calculate.Tspin_Double_Mini)
    //     {
    //         for (int count = 0; count < Tspin_Double_Minis.Length; count++)
    //         {
    //             if (Tspin_Double_Minis[count].gameObject.activeSelf == false)
    //             {
    //                 TextAnimation(Tspin_Double_Minis[count]));

    //                 break;
    //             }
    //         }
    //     }
    // }

    private void TextAnimation(Text _SelectText)
    {
        // //アクティブにする
        // _SelectText.gameObject.SetActive(true);

        Debug.Log(_SelectText);

        Text InstantiatedText = Instantiate(_SelectText, Canvas);

        //選ばれたテキストのテキストコンポーネントとトランスフォームコンポーネントを取得
        Text SelectText_Text = InstantiatedText.GetComponent<Text>();
        Transform SelectText_Transform = InstantiatedText.GetComponent<Transform>();

        //選ばれたテキストのフェードインとフェードアウトを行う
        TextFadeInAndOut(SelectText_Text);

        //選ばれたテキストの移動アニメーションを行う
        TextMove(SelectText_Transform);

        //3.2秒待つ
        //yield return new WaitForSeconds(waitTime);

        // //アニメーションが完了したら非表示に戻す
        // _SelectText.gameObject.SetActive(false);
    }

    //選ばれたテキストのフェードインとフェードアウトを行う関数
    private void TextFadeInAndOut(Text selectText)
    {
        //Sequenceのインスタンス化
        var sequence = DOTween.Sequence();

        //0.3秒かけてアルファ値を1(=不透明)に変化させる
        //その後、2秒表示
        //最後に、1秒かけてアルファ値を0(=透明)に変化させる
        sequence.Append(selectText.DOFade(alpha_1, FadeInInterval))
                .AppendInterval(waitInterval)
                .Append(selectText.DOFade(alpha_0, FadeOutInterval));
    }


    //選ばれたテキストの移動アニメーションを行う関数
    private void TextMove(Transform selectText)
    {
        //selectTextの各座標を格納する変数を宣言
        float selectText_x = Mathf.RoundToInt(selectText.transform.position.x);
        float selectText_y = Mathf.RoundToInt(selectText.transform.position.y);
        float selectText_z = Mathf.RoundToInt(selectText.transform.position.z);

        //Sequenceのインスタンス化
        var sequence = DOTween.Sequence();

        //現在位置から3秒で、Y座標を600移動する
        //始点と終点の繋ぎ方はOutSineで設定
        //その後、現在位置から1秒で、X座標を-600移動する(移動中にフェードアウトする)
        //始点と終点の繋ぎ方はInQuintで設定
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
}