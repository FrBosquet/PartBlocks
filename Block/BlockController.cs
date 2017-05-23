using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockController : SpatialObject {

    public bool IsOccupied(SpatialObject so) {
        return IsOccupied(so.GridPosition);
    }//Returns true if the position is already 

    public bool IsOccupied(Vector3 so)
    {
        bool occupied = false;
        Block[] everyBlock = GetEveryBlock();

        foreach (Block mb in everyBlock)
        {
            if (mb.IsSamePlace(so)) occupied = true;
        }

        return occupied;
    }//Returns true if the position is already occupied

    /*
        Returns a block given a grid position
        */
    public Block GetBlockAt(Vector3 gridPoint) {
        Block[] everyBlock = GetEveryBlock();

        foreach (Block mb in everyBlock)
        {
            if (mb.IsSamePlace(gridPoint)) return mb;
        }

        return null;
    }

    public Block[] GetEveryBlock(){
        return transform.GetComponentsInChildren<Block>();
    }

    /*
        Return the blocks related for a given grid point
    */
    public Block[] GetRelatedBlocks(Vector3 gridPosition) {
        Block[] relatedBlocks = new Block[6];

        for (int i = 0; i < 5; i++) {
            Vector3 position = gridPosition + directions[i];
            relatedBlocks[i] = GetBlockAt(position);
        }
        return relatedBlocks;
    }
}
