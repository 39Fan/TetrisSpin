using UnityEngine;

//複数のスクリプトに干渉するデータをまとめて扱うスクリプト
public class Data : MonoBehaviour
{
    //GameManagerとRotationで用いる
    //初期(未回転)状態をNorthとして、
    //右回転後の向きをEast
    //左回転後の向きをWest
    //2回右回転または左回転した時の向きをSouthとする
    public int North = 0;
    public int East = 90;
    public int South = 180;
    public int West = 270;
}
