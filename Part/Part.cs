using UnityEngine;
using System.Collections;

public class Part : MonoBehaviour {
    public Part symmetry;

    public void RequestDelete() {
        if (symmetry) {
            GameObject.Destroy(symmetry.gameObject);
        }
    }

    public Part Symmetry
    {
        get
        {
            return symmetry;
        }

        set
        {
            symmetry = value;
        }
    }
}
