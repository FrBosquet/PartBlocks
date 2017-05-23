using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Block : SpatialObject {
    
    #region PROPERTIES
    public PartController partController;
    public BlockController blockController;
    public PotentialBlockController potentialController;

    [Header("Prefabs")]
    public GameObject potentialPrimitive;
    public Material previewMaterial;

    [Header("State")]    
    public PotentialBlock[] relativePotentials;
    public Block[] relativeBlocks;
    public List<GameObject> associatedParts;
    
    #region config
    [Header("Configuration")]
    public string[] interfaces; //Left, Right, Down, Up, Back, Front
    public List<GameObject> parts;
    #endregion
    #endregion

    #region FUNCTIONS
    #region mono
    void Awake() {
        partController = GameObject.Find("partController").GetComponent<PartController>();
        blockController = GameObject.FindObjectOfType<BlockController>();
        potentialController = GameObject.FindObjectOfType<PotentialBlockController>();
        relativePotentials = new PotentialBlock[6];
        relativeBlocks = new Block[6];

        gridPosition = PositionToGrid(transform.position);
        potentialController.DeletePotentialBlock(gridPosition);

        foreach (GameObject o in parts)
        {
            associatedParts.Add(partController.ProvidePart(o, gridPosition));
        }

        for (int i = 0; i < 6; i++)
        {
            if (interfaces[i].Length != 0)
            {
                CreatePotentialBlock(i);
            }
        }
    }

    /*
    When starts:
    1º Create every part and file it in associated parts
    2º Create and configure potentials:
        2.1 Check if any potential in every place
            2.1.1 If yes, configure input interface for a given face
            2.1.2 If not, create potential 
        
        */
    #endregion
    #region class
    public void CreatePotentialBlock(int face) {
        if (!blockController.IsOccupied(FaceToGridPosition(face))){
            relativePotentials[face] = potentialController.RequestPotentialBlock(gridPosition + directions[face], this, face);
        }
    }

    public void CloneAs(PotentialBlock potential) {
        Instantiate(gameObject, potential.Position, Quaternion.identity);
    }

    //
    public void RequestDelete() {
        //Delete parts and symmetrycs
        for (int i = associatedParts.Count - 1; i >= 0; i--) {
            associatedParts[i].GetComponent<Part>().RequestDelete();
            GameObject.Destroy(associatedParts[i].gameObject);
        }

        //Break potential block connections
        for (int face = 0; face < 6; face ++) {
            if (relativePotentials[face] != null) {
                bool lastBlock = relativePotentials[face].LoseBlock(Complement(face));
                if(lastBlock) GameObject.Destroy(relativePotentials[face].gameObject);
            }
        }
    }
    #endregion
    #endregion
}
