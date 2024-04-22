using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ScriptableObjectを継承したTetrisSpinDataクラス //

//[CreateAssetMenu(menuName = "ScriptableObjects/TetrisSpinData")]

public class TetrisSpinData : MonoBehaviour //不変のデータを扱う
{
    // ミノの種類
    public string[] MINOTYPES { get; } = new string[]
    {
        "I_mino", "J_mino", "L_mino", "O_mino", "S_mino", "T_mino", "Z_mino"
    };

    //ミノのPrefabs
    //順番は(I, J, L, O, S, T, Z)
    [SerializeField] public Mino[] MINOS;

    //ゴーストミノのPrefabs
    //順番は(I, J, L, O, S, T, Z)
    [SerializeField] public Mino_Ghost[] MINOS_GHOST;

    //ミノの向きについて//

    //GameManagerとRotationで用いる
    //初期(未回転)状態をnorthとして、
    //右回転後の向きをeast
    //左回転後の向きをwest
    //2回右回転または左回転した時の向きをsouthとする
    public int NORTH { get; } = 0;
    public int EAST { get; } = 90;
    public int SOUTH { get; } = 180;
    public int WEST { get; } = 270;

    //新しくミノが降ってくる時の初期座標
    public Vector3 SPAWNMINOPOSITION { get; } = new Vector3(4, 20, 0);

    //Holdされたミノの座標(画面左上に配置)
    public Vector3 HOLDMINOPOSITION { get; } = new Vector3(-3, 17, 0);

    //テトリスのゲームフィールドについて//

    //ゲームフィールドは高さ20マス、幅10マスに設定
    //headerは、ゲームオーバーの判定に必要
    //height - header で高さを表現
    public int HEIGHT { get; } = 40;
    public int WIDTH { get; } = 10;
    public int HEADER { get; } = 20;

    public static TetrisSpinData tetrisSpinData;

    private void Awake()
    {
        tetrisSpinData = this;
    }

}
