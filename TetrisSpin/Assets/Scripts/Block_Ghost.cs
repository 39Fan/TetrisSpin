using UnityEngine;

//ゴーストミノの移動、通常回転を扱うスクリプト

public class Block_Ghost : MonoBehaviour
{
    //干渉するスクリプトの設定
    Data data;

    //回転していいブロックかどうか
    //Oミノは回転しないので、falseに設定
    [SerializeField]
    private bool canRotate = true;

    //インスタンス化
    private void Awake()
    {
        data = FindObjectOfType<Data>();
    }

    //移動用
    public void Move(Vector3 moveDirection)
    {
        transform.position += moveDirection;
    }

    //移動関数を呼ぶ関数(4種類)
    public void MoveLeft()
    {
        Move(new Vector3(-1, 0, 0));

    }

    public void MoveRight()
    {
        Move(new Vector3(1, 0, 0));

    }

    public void MoveUp()
    {
        Move(new Vector3(0, 1, 0));

    }

    public void MoveDown()
    {
        Move(new Vector3(0, -1, 0));

    }

    //通常右回転
    public void RotateRight(Block_Ghost block)
    {
        //回転できるブロックかどうか
        //Oミノは回転できないので弾かれる
        if (canRotate)
        {
            //Z軸の回転量を格納
            int rotateAroundZ = -90;

            //Iミノ以外の回転
            if (!block.name.Contains("I"))
            {
                transform.Rotate(0, 0, rotateAroundZ);
            }
            //Iミノの回転
            //Iミノは軸が他のミノと違うため別の処理
            else
            {
                //Iミノの軸を取得する
                //Iミノのx, y座標を渡す
                Vector3 IminoAxis = data.AxisCheck
                    (Mathf.RoundToInt(block.transform.position.x), Mathf.RoundToInt(block.transform.position.y));

                //IminoAxisを中心に右回転する
                transform.RotateAround(IminoAxis, Vector3.forward, rotateAroundZ);
            }
        }
    }

    //通常左回転
    public void Rotateleft(Block_Ghost block)
    {
        //回転できるブロックかどうか
        //Oミノは回転できないので弾かれる
        if (canRotate)
        {
            //Z軸の回転量を格納
            int rotateAroundZ = 90;

            //Iミノ以外の回転
            if (!block.name.Contains("I"))
            {
                transform.Rotate(0, 0, rotateAroundZ);
            }
            //Iミノの回転
            //Iミノは軸が他のミノと違うため別の処理
            else
            {
                //Iミノの軸を取得する
                Vector3 IminoAxis = data.AxisCheck
                    (Mathf.RoundToInt(block.transform.position.x), Mathf.RoundToInt(block.transform.position.y));

                //IminoAxisを中心に右回転する
                transform.RotateAround(IminoAxis, Vector3.forward, rotateAroundZ);
            }
        }
    }
}
