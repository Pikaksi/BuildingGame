using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BlockStruct
{
    public BlockEnums blockType;
    public int direction; // 0 = top, 1 = bottom, 2 = forward, 3 = backward, 4 = right, 5 = left

    public BlockStruct(BlockEnums blockType, int direction)
    {
        this.blockType = blockType;
        this.direction = direction;
    }
}
