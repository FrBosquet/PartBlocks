using UnityEngine;
using System.Collections;

public class DeletersController : SpatialObject {
    public GameObject deleterPrimitive;

    public void RequestDeleter(Block target) {
        GameObject newDeleter = Instantiate(deleterPrimitive);
        newDeleter.transform.SetParent(transform);
        newDeleter.transform.localScale = Vector3.one * scale;
        newDeleter.GetComponent<Deleter>().GridPosition = target.GridPosition;
        newDeleter.GetComponent<Deleter>().Target = target;
        newDeleter.SetActive(false);
    }

    public void ShowChilds(bool state) {
        foreach (Transform child in transform) {
            child.gameObject.SetActive(state);
        }
    }
}
