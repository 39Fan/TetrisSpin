// using UnityEngine;

// //ミノの移動、通常回転を扱うスクリプト

// public class Mino : MonoBehaviour
// {
//     //干渉するスクリプトの設定
//     Calculate calculate;

//     //回転していいミノかどうか
//     //Oミノは回転しないので、falseに設定
//     [SerializeField]
//     private bool can_rotate = true;

//     //インスタンス化
//     private void Awake()
//     {
//         calculate = FindObjectOfType<Calculate>();
//     }

//     //移動用
//     public void Move(Vector3 moveDirection)
//     {
//         transform.position += moveDirection;
//     }

//     //移動関数を呼ぶ関数(4種類)
//     public void MoveLeft()
//     {
//         Move(new Vector3(-1, 0, 0));

//     }

//     public void MoveRight()
//     {
//         Move(new Vector3(1, 0, 0));

//     }

//     public void MoveUp()
//     {
//         Move(new Vector3(0, 1, 0));

//     }

//     public void MoveDown()
//     {
//         Move(new Vector3(0, -1, 0));

//     }

//     //通常右回転
//     public void RotateRight(Mino _ActiveMino)
//     {
//         //回転できるブロックかどうか
//         //Oミノは回転できないので弾かれる
//         if (can_rotate)
//         {
//             //Z軸の回転量を格納
//             int rotateAroundZ = -90;

//             //Iミノ以外の回転
//             if (!_ActiveMino.name.Contains("I"))
//             {
//                 transform.Rotate(0, 0, rotateAroundZ);
//             }
//             //Iミノの回転
//             //Iミノは軸が他のミノと違うため別の処理
//             else
//             {
//                 //Iミノの軸を取得する
//                 Vector3 IminoAxis = calculate.AxisCheck
//                     (Mathf.RoundToInt(_ActiveMino.transform.position.x), Mathf.RoundToInt(_ActiveMino.transform.position.y));

//                 //IminoAxisを中心に右回転する
//                 transform.RotateAround(IminoAxis, Vector3.forward, rotateAroundZ);
//             }
//         }
//     }

//     //通常左回転
//     public void Rotateleft(Mino _ActiveMino)
//     {
//         //回転できるブロックかどうか
//         //Oミノは回転できないので弾かれる
//         if (can_rotate)
//         {
//             //Z軸の回転量を格納
//             int rotateAroundZ = 90;

//             //Iミノ以外の回転
//             if (!_ActiveMino.name.Contains("I"))
//             {
//                 transform.Rotate(0, 0, rotateAroundZ);
//             }
//             //Iミノの回転
//             //Iミノは軸が他のミノと違うため別の処理
//             else
//             {
//                 //Iミノの軸を取得する
//                 Vector3 IminoAxis = calculate.AxisCheck
//                     (Mathf.RoundToInt(_ActiveMino.transform.position.x), Mathf.RoundToInt(_ActiveMino.transform.position.y));

//                 //IminoAxisを中心に右回転する
//                 transform.RotateAround(IminoAxis, Vector3.forward, rotateAroundZ);
//             }
//         }
//     }
// }