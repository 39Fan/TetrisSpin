using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
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
    public void RotateRight(int minoAngleBefore, Block block)
    {
        if (canRotate && !block.name.Contains("I"))
        {
            transform.Rotate(0, 0, -90);
        }
        else
        {
            switch (minoAngleBefore)
            {
                case 0:
                    transform.RotateAround(new Vector3(block.transform.position.x + 0.5f, block.transform.position.y - 0.5f, 0), Vector3.forward, -90);
                    break;
                case 270:
                    transform.RotateAround(new Vector3(block.transform.position.x - 0.5f, block.transform.position.y - 0.5f, 0), Vector3.forward, -90);
                    break;
                case 180:
                    transform.RotateAround(new Vector3(block.transform.position.x - 0.5f, block.transform.position.y + 0.5f, 0), Vector3.forward, -90);
                    break;
                case 90:
                    transform.RotateAround(new Vector3(block.transform.position.x + 0.5f, block.transform.position.y + 0.5f, 0), Vector3.forward, -90);
                    break;
            }
        }
    }

    public void Rotateleft(int minoAngleBefore, Block block)
    {
        if (canRotate && !block.name.Contains("I"))
        {
            transform.Rotate(0, 0, 90);
        }
        else
        {
            switch (minoAngleBefore)
            {
                case 0:
                    transform.RotateAround(new Vector3(block.transform.position.x + 0.5f, block.transform.position.y - 0.5f, 0), Vector3.forward, 90);
                    break;
                case 270:
                    transform.RotateAround(new Vector3(block.transform.position.x - 0.5f, block.transform.position.y - 0.5f, 0), Vector3.forward, 90);
                    break;
                case 180:
                    transform.RotateAround(new Vector3(block.transform.position.x - 0.5f, block.transform.position.y + 0.5f, 0), Vector3.forward, 90);
                    break;
                case 90:
                    transform.RotateAround(new Vector3(block.transform.position.x + 0.5f, block.transform.position.y + 0.5f, 0), Vector3.forward, 90);
                    break;
            }
        }
    }
}
