using UnityEngine;
using System.Collections;

public abstract class SpatialObject : MonoBehaviour {
    protected Vector3[] directions = new Vector3[] { Vector3.left, Vector3.right, Vector3.down, Vector3.up, Vector3.back, Vector3.forward};
    protected static float scale = 0.16f; //Medida del bloque en metros
    protected static Vector3 symmetryAxis = new Vector3(0.5f, 0, 0);
    protected Vector3 gridPosition;

    public static Vector3 PositionToGrid(Vector3 position) {
        int x, y, z;

        x = Mathf.RoundToInt(position.x / scale);
        y = Mathf.RoundToInt(position.y / scale);
        z = Mathf.RoundToInt(position.z / scale);

        return new Vector3(x, y, z);
    }

    public static int Complement(int a) {
        int b = -1;
        switch (a) {
            case 0:
                b = 1;
                break;
            case 1:
                b = 0;
                break;
            case 2:
                b = 3;
                break;
            case 3:
                b = 2;
                break;
            case 4:
                b = 5;
                break;
            case 5:
                b = 4;
                break;
        }
        return b;
    }
    public Vector3 FaceToGridPosition(int face) {
        return gridPosition + directions[face];
    }

    public bool IsSamePlace(SpatialObject so) {
        return IsSamePlace(so.GridPosition);
    }

    public bool IsSamePlace(Vector3 so)
    {
        if (this.gridPosition == so)
        {
            return true;
        }
        else {
            return false;
        }
    }

    #region GETTER SETTER
    public Vector3 GridPosition {
        get {
            return gridPosition;
        }

        set {
            gridPosition = value;
            transform.position = gridPosition * scale;
        }
    }
    #endregion
}
