using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureDataClass
{
    public BlockEnums blockType;
    public bool isFullBlock;
    public Vector2Int up, down, front, back, right, left;
    public Vector2Int[] textureLookupTable;

    public TextureDataClass(BlockEnums blockType, bool isFullBlock, Vector2Int up, Vector2Int down, Vector2Int front, Vector2Int back, Vector2Int right, Vector2Int left)
    {
        this.blockType = blockType;
        this.isFullBlock = isFullBlock;
        this.up = up;
        this.down = down;
        this.front = front;
        this.back = back;
        this.right = right;
        this.left = left;

        //textureLookupTable = new Vector2Int[9] {left, /**/up, down, back, /**/up, right, up, /**/up, front};
    }

    public Vector2Int FindTexturePosition(Vector3Int direction)
    {
        //return textureLookupTable[(direction.x | direction.y << 1 | direction.z << 2) + 4];
        
        if (direction == Vector3Int.up) {
            return this.up;
        }
        else if (direction == Vector3Int.forward) {
            return this.front;
        }
        else if (direction == Vector3Int.back) {
            return this.back;
        }
        else if (direction == Vector3Int.left) {
            return this.left;
        }
        else if (direction == Vector3Int.right) {
            return this.right;
        }
        else {
            return this.down;
        }
    }
}
