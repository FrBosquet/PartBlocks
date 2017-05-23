using UnityEngine;
using System.Collections;

public class PotentialBlock : SpatialObject {
    public PotentialBlockController potentialController;

    public Block[] relativeBlocks = new Block[6];
    #region config
    [Header("State")]
    public string[] interfaces; //Left, Right, Down, Up, Back, Front
    #endregion

    #region FUNCTIONS
    void Awake() {
        interfaces = new string[6];
        potentialController = GameObject.FindObjectOfType<PotentialBlockController>();
    }

    public bool CheckCompatibility(Block nextBlock) {
        bool compatible = true;

        for (int i = 0; i < 6; i++) {
            if (interfaces[i] != null && interfaces[i] != nextBlock.interfaces[i]) {
                compatible = false;
                break;
            }
        }

        gameObject.SetActive( compatible );

        return compatible;
    }

    /*
        Deletes a faced block. If no relative block left, returns true so the block can delete it
        */
    public bool LoseBlock(int face)
    {
        interfaces[face] = null;
        relativeBlocks[face] = null;

        bool noBlocksLeft = true;

        for (int i = 0; i < 6; i++) {
            if (relativeBlocks[i] != null)
            {
                noBlocksLeft = false;
                break;
            }
        }

        //If is goin to get destroyed, remove from the potentials controller list
        if (noBlocksLeft) potentialController.DeletePotentialBlock(gridPosition);

        return noBlocksLeft;
    }
    #endregion

    #region GETTER SETTER
    public Vector3 Position {
        get {
            return transform.position;
        }
        set {
            transform.position = value;
            gridPosition = PositionToGrid(value);
        }
    }

    public string[] Interfaces
    {
        get
        {
            return interfaces;
        }

        set
        {
            interfaces = value;
        }
    }
    #endregion
}
