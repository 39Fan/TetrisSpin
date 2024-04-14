using UnityEngine;

//複数のスクリプトに干渉するデータをまとめて扱うスクリプト
public class Data : MonoBehaviour
{
    //干渉するスクリプトの設定

    //GameManagerとRotationで用いる
    //初期(未回転)状態をNorthとして、
    //右回転後の向きをEast
    //左回転後の向きをWest
    //2回右回転または左回転した時の向きをSouthとする
    public int North = 0;
    public int East = 90;
    public int South = 180;
    public int West = 270;

    //ミノが回転した時、回転前の向きを保存する変数
    //初期値はNorthの状態
    public int MinoAngleBefore = 0;

    public int MinoAngleAfter = 0;

    private void Start()
    {

    }

    public void Reset()
    {
        MinoAngleBefore = 0;
        MinoAngleAfter = 0;
    }

    //Iミノの軸を計算し、Vectoe3で返す関数
    public Vector3 AxisCheck(Block block)
    {
        Debug.Log("AxisCheck");
        //xとyのオフセットを宣言
        //zは関係ない
        float xOffset = 0.5f;
        float yOffset = 0.5f;

        //Iミノの座標を格納
        //xとyで分ける
        //zは関係ない
        int Imino_x = Mathf.RoundToInt(block.transform.position.x);
        int Imino_y = Mathf.RoundToInt(block.transform.position.y);

        //回転軸は現在位置から、x軸をxOffset動かし、y軸をyOffset動かした座標にある
        //xOffsetとyOffsetの正負は回転前の向きによって変化する

        //回転前の向きがNorthの時
        if (MinoAngleBefore == North)
        {
            return new Vector3(Imino_x + xOffset, Imino_y - yOffset, 0);
        }
        //回転前の向きがEastの時
        else if (MinoAngleBefore == East)
        {
            return new Vector3(Imino_x - xOffset, Imino_y - yOffset, 0);
        }
        //回転前の向きがSouthの時
        else if (MinoAngleBefore == South)
        {
            return new Vector3(Imino_x - xOffset, Imino_y + yOffset, 0);
        }
        //回転前の向きがWestの時
        //MinoAngleBefore == West
        else
        {
            return new Vector3(Imino_x + xOffset, Imino_y + yOffset, 0);
        }
    }

    //回転後の角度(MinoAngleAfter)の調整
    //Z軸で回転を行っているため、90°(East)と270°(West)はプログラム上 270, 90 と表記されているため(左右反転している)
    public void CalibrateMinoAngleAfter(Block block)
    {
        //block.transform.rotation.eulerAngles.zはZ軸の回転角度
        //eulerAnglesはオイラー角の意
        //調整前の角度
        int OriginalAngle = Mathf.RoundToInt(block.transform.rotation.eulerAngles.z);

        if (OriginalAngle == West)
        {
            MinoAngleAfter = East;
        }
        else if (OriginalAngle == East)
        {
            MinoAngleAfter = West;
        }
        else
        {
            //修正の必要なし
            MinoAngleAfter = OriginalAngle;
        }
    }

    //通常回転のリセットをする関数
    public void RotateReset(Block block, int minoAngleAfter)
    {
        //通常回転が右回転だった時
        if ((MinoAngleBefore == North && minoAngleAfter == East) ||
        (MinoAngleBefore == East && minoAngleAfter == South) ||
        (MinoAngleBefore == South && minoAngleAfter == West) ||
        (MinoAngleBefore == West && minoAngleAfter == North))
        {
            //左回転で回転前の状態に戻す
            block.Rotateleft(block);
        }
        //通常回転が左回転だった時
        else
        {
            //右回転で回転前の状態に戻す
            block.RotateRight(block);
        }
    }
}
