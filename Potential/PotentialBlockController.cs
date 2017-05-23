using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PotentialBlockController : SpatialObject {

    public List<PotentialBlock> potentialBlocks;
    public GameObject potentialPrimitive;

    /*
        Request a new potential block. If position is occupied, it assign the new interface.

        Returns: The Potential block
        */
    public PotentialBlock RequestPotentialBlock(Vector3 gridPosition, Block invoker, int face) {
        if (gridPosition.x < symmetryAxis.x) return null;
        if (gridPosition.y < symmetryAxis.y) return null;

        foreach (PotentialBlock p in potentialBlocks) {
            if (p.GridPosition == gridPosition) {
                Debug.Log("Found one");
                p.relativeBlocks[Complement(face)] = invoker;
                p.Interfaces[Complement(face)] = invoker.interfaces[face];
                return p;
            }
        };

        PotentialBlock newPotential = Instantiate(potentialPrimitive).GetComponent<PotentialBlock>();
        newPotential.transform.SetParent(transform);
        newPotential.Position = (gridPosition) * 0.16f;
        newPotential.relativeBlocks[Complement(face)] = invoker;
        newPotential.Interfaces[Complement(face)] = invoker.interfaces[face];
        potentialBlocks.Add(newPotential);
        return newPotential;
    }

    public PotentialBlock RequestPotentialBlock(Vector3 gridPosition) {
        PotentialBlock newPotential = Instantiate(potentialPrimitive).GetComponent<PotentialBlock>();
        newPotential.transform.SetParent(transform);
        newPotential.Position = (gridPosition) * 0.16f;
        potentialBlocks.Add(newPotential);
        return newPotential;
    }

    public PotentialBlock RequestPotentialBlock(Vector3 gridPosition, Block[] relatedBlocks) {
        PotentialBlock newPotential = RequestPotentialBlock(gridPosition);
        for (int i = 0; i < 6; i++) {
            newPotential.relativeBlocks[i] = relatedBlocks[i];
            if (relatedBlocks[i] != null) {
                newPotential.interfaces[i] = relatedBlocks[i].interfaces[Complement(i)];
                relatedBlocks[i].relativePotentials[Complement(i)] = newPotential;
            }
        }
        
        return newPotential;
    }

    /*
        Request for destrucction of a potential block, due to the space to be occupied
        */
    public void DeletePotentialBlock(Vector3 gridPosition) {
        foreach (PotentialBlock p in potentialBlocks) {
            if (p.GridPosition == gridPosition) {
                Debug.Log("Remove one");
                potentialBlocks.Remove(p);
                GameObject.Destroy(p.gameObject);
                return;
            }
        }
    }

    /*
        Updates potential blocks visibility due to compatibility with next block
        */
    public void UpdatePotentialBlocksVisibility(Block nextBlock) {
        foreach (PotentialBlock pb in potentialBlocks) {
            //Check every potential B. and hide if not compatible
            pb.CheckCompatibility(nextBlock);
        }
    }

    /*
        Hides or unhides every potential block
    */
    public void SetVisibility(bool state) {
        foreach (PotentialBlock pb in potentialBlocks)
        {
            pb.gameObject.SetActive(state);
        }
    }
}
