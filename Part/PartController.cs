using UnityEngine;
using System.Collections;

public class PartController : SpatialObject {

    public PartController SymmetricPart;

    /*
        Creates a new part and checks for symmetry
        */
    public GameObject ProvidePart(GameObject o, Vector3 gridPosition)
    {
        GameObject newPart = CreatePart(o, gridPosition, Vector3.one);
        Debug.Log("Creating new part");

        if (SymmetricPart != null) {
            //Debug.Log("Symmetricing");
            float newX = (2 * symmetryAxis.x) - gridPosition.x;
            Vector3 mirroredPosition = new Vector3(newX, gridPosition.y, gridPosition.z);
            GameObject newSymmetricPart = SymmetricPart.CreatePart(o, mirroredPosition, new Vector3(-1, 1, 1));
            newPart.GetComponent<Part>().Symmetry = newSymmetricPart.GetComponent<Part>();
        }

        return newPart;
    }

    /*
        Creates a new part as sibling
    */
    public GameObject CreatePart(GameObject primitive, Vector3 gridPosition, Vector3 symScale) {
        GameObject newPart = Instantiate(primitive);
        newPart.transform.SetParent(transform);
        newPart.transform.position = gridPosition * scale;
        newPart.transform.localScale = symScale * scale;
        return newPart;
    }

    /*
        Deletes the part and the symmetry if found
    */
    public void RequestDelete() {

    }
}
