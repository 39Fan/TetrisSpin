using UnityEngine;

public class Block_Ghost : MonoBehaviour
{
    //回転していいブロックかどうか
    [SerializeField]
    private bool canRotate = true;

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

    //回転用(2種類)
    public void RotateRight()
    {
        if (canRotate)
        {
            transform.Rotate(0, 0, -90);
        }
    }

    public void Rotateleft()
    {
        if (canRotate)
        {
            transform.Rotate(0, 0, 90);
        }
    }
}
