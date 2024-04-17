using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

//ゲーム画面のテキストに関するスクリプト
//表示できるテキストの一覧はData.csに記載
public class MainSceneText : MonoBehaviour
{
    //干渉するスクリプトの設定
    Data data;

    //表示できるテキストの一覧
    //変数宣言の文法上、実際にゲーム画面に表示するテキストと変数名が合致しない場合がある
    [SerializeField] private Text one_Line_Clear;
    [SerializeField] private Text two_Line_Clear;
    [SerializeField] private Text three_Line_Clear;
    [SerializeField] private Text Tetris;
    // [SerializeField] private Text Ispin;
    // [SerializeField] private Text Ispin_Single;
    // [SerializeField] private Text Ispin_Double;
    // [SerializeField] private Text Ispin_Triple;
    // [SerializeField] private Text Ispin_Quattro;
    // [SerializeField] private Text Jspin;
    // [SerializeField] private Text Jspin_Single;
    // [SerializeField] private Text Jspin_Double;
    // [SerializeField] private Text Jspin_Triple;
    // [SerializeField] private Text Lspin;
    // [SerializeField] private Text Lspin_Single;
    // [SerializeField] private Text Lspin_Double;
    // [SerializeField] private Text Lspin_Triple;
    // [SerializeField] private Text Ospin;
    // [SerializeField] private Text Ospin_Single;
    // [SerializeField] private Text Ospin_Double;
    // [SerializeField] private Text Ospin_Triple;
    // [SerializeField] private Text Sspin;
    // [SerializeField] private Text Sspin_Single;
    // [SerializeField] private Text Sspin_Double;
    // [SerializeField] private Text Sspin_Triple;
    [SerializeField] private Text Tspin;
    [SerializeField] private Text Tspin_Single;
    [SerializeField] private Text Tspin_Double;
    [SerializeField] private Text Tspin_Triple;
    [SerializeField] private Text Tspin_Mini;
    [SerializeField] private Text Tspin_Double_Mini;
    // [SerializeField] private Text Zspin;
    // [SerializeField] private Text Zspin_Single;
    // [SerializeField] private Text Zspin_Double;
    // [SerializeField] private Text Zspin_Triple;

    //フェードインとフェードアウトする時間
    float fadeInInterval = 0.2f;
    float fadeOutInterval = 1f;

    //テキストを表示させる時間
    float waitInterval = 2f;

    //透明度
    //0の時は透明、1の時は不透明
    int alpha = 1;

    //x軸とy軸それぞれのテキストを移動させる時間
    float moveInterval_x = 2f;
    float moveInterval_y = 1.2f;

    //テキストの移動量
    float moveDistance = 600f;

    //インスタンス化
    void Start()
    {
        data = FindObjectOfType<Data>();
    }

    //表示するテキストを判別して、実際に表示する関数
    public void TextDisplay(int text)
    {
        //textと合致するGameObjectのTextコンポーネントを取得
        if (text == data.one_Line_Clear)
        {
            //選ばれたテキストのテキストコンポーネントとトランスフォームコンポーネントを取得
            Text SelectText_Text = one_Line_Clear.GetComponent<Text>();
            Transform SelectText_Transform = one_Line_Clear.GetComponent<Transform>();

            //選ばれたテキストのフェードインとフェードアウトを行う
            TextFadeInAndOut(SelectText_Text);

            //選ばれたテキストの移動アニメーションを行う
            TextMove(SelectText_Transform);

            //↓以下同文
        }
        else if (text == data.two_Line_Clear)
        {
            Text SelectText_Text = two_Line_Clear.GetComponent<Text>();
            Transform SelectText_Transform = two_Line_Clear.GetComponent<Transform>();

            TextFadeInAndOut(SelectText_Text);
            TextMove(SelectText_Transform);
        }
        else if (text == data.three_Line_Clear)
        {
            Text SelectText_Text = three_Line_Clear.GetComponent<Text>();
            Transform SelectText_Transform = three_Line_Clear.GetComponent<Transform>();

            TextFadeInAndOut(SelectText_Text);
            TextMove(SelectText_Transform);
        }
        else if (text == data.Tetris)
        {
            Text SelectText_Text = Tetris.GetComponent<Text>();
            Transform SelectText_Transform = Tetris.GetComponent<Transform>();

            TextFadeInAndOut(SelectText_Text);
            TextMove(SelectText_Transform);
        }
        else if (text == data.Tspin)
        {
            Text SelectText_Text = Tspin.GetComponent<Text>();
            Transform SelectText_Transform = Tspin.GetComponent<Transform>();

            TextFadeInAndOut(SelectText_Text);
            TextMove(SelectText_Transform);
        }
        else if (text == data.Tspin_Single)
        {
            Text SelectText_Text = Tspin_Single.GetComponent<Text>();
            Transform SelectText_Transform = Tspin_Single.GetComponent<Transform>();

            TextFadeInAndOut(SelectText_Text);
            TextMove(SelectText_Transform);
        }
        else if (text == data.Tspin_Double)
        {
            Text SelectText_Text = Tspin_Double.GetComponent<Text>();
            Transform SelectText_Transform = Tspin_Double.GetComponent<Transform>();

            TextFadeInAndOut(SelectText_Text);
            TextMove(SelectText_Transform);
        }
        else if (text == data.Tspin_Triple)
        {
            Text SelectText_Text = Tspin_Triple.GetComponent<Text>();
            Transform SelectText_Transform = Tspin_Triple.GetComponent<Transform>();

            TextFadeInAndOut(SelectText_Text);
            TextMove(SelectText_Transform);
        }
        else if (text == data.Tspin_Mini)
        {
            Text SelectText_Text = Tspin_Mini.GetComponent<Text>();
            Transform SelectText_Transform = Tspin_Mini.GetComponent<Transform>();

            TextFadeInAndOut(SelectText_Text);
            TextMove(SelectText_Transform);
        }
        else if (text == data.Tspin_Double_Mini)
        {
            Text SelectText_Text = Tspin_Double_Mini.GetComponent<Text>();
            Transform SelectText_Transform = Tspin_Double_Mini.GetComponent<Transform>();

            TextFadeInAndOut(SelectText_Text);
            TextMove(SelectText_Transform);
        }
    }

    //選ばれたテキストのフェードインとフェードアウトを行う関数
    private void TextFadeInAndOut(Text selectText)
    {
        //Sequenceのインスタンス化
        var sequence = DOTween.Sequence();

        //0.3秒かけてアルファ値を256(=不透明)に変化させる
        sequence.Append(selectText.DOFade(alpha, fadeInInterval));

        //2秒表示
        sequence.AppendInterval(waitInterval);

        //透明度を0にする
        alpha = 0;

        //1秒かけてアルファ値を0(=透明)に変化させる
        sequence.Append(selectText.DOFade(alpha, fadeOutInterval));

        //透明度を1に戻す
        alpha = 1;
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
        //移動量が時間が経つにつれて小さくなるような動きに設定
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