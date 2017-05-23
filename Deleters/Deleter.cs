using UnityEngine;
using System.Collections;

public class Deleter : SpatialObject {
    public Block target;

    void Awake() {
        transform.localScale = Vector3.one * scale;
        transform.position = gridPosition * scale;
    }

    #region GETTER SETTER
    public Block Target
    {
        get
        {
            return target;
        }

        set
        {
            target = value;
        }
    }
    #endregion
}
