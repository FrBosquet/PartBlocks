using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputController : SpatialObject {
    public string mode = "create";

    public GameObject blockPrimitive;
    public GameObject blockPreview;
    public List<GameObject> previewParts;

    public BlockController blockController;
    public Transform partController;
    public PotentialBlockController potentialController;
    public DeletersController deleterController;

    public float panSensitivity;
    public float zoomSensitivity;

    private Transform cam;

    void Awake() {
        //For testing purpose, create a corner in the center of scene
        //Instantiate(blockPrimitive).transform.SetParent(blockController);
        DefaultPotential();
    }

    public void Start() {
        //Update tool to update potential blocks
        SetTool(blockPrimitive);

        cam = Camera.main.transform;
    }

    void Update() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        int layer;

        switch (mode) {
            case "create":
                layer = 1 << 8;

                bool hasHit = Physics.Raycast(ray, out hit, Mathf.Infinity, layer);

                if (Input.GetMouseButton(0) && hasHit)
                {//If mouse clicks a potential
                 //Retrieve potential object
                    GameObject newBlock;
                    newBlock = (GameObject)Instantiate(blockPrimitive, hit.transform.position, Quaternion.identity);
                    newBlock.transform.SetParent(blockController.transform);
                    deleterController.RequestDeleter(newBlock.GetComponent<Block>());
                    //Update potentials visibility
                    potentialController.UpdatePotentialBlocksVisibility(blockPrimitive.GetComponent<Block>());
                }
                else if (hasHit)
                {
                    //Preview
                    blockPreview.transform.position = hit.transform.position;
                    blockPreview.SetActive(true);
                }
                else {
                    blockPreview.SetActive(false);
                }
                break;
            case "delete":
                layer = 1 << 9;
                if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
                {//If mouse clicks a potential
                    Deleter targetDeleter = hit.transform.GetComponent<Deleter>();
                    Block targetBlock = targetDeleter.Target;
                    Vector3 gridPosition = targetBlock.GridPosition;
                    targetBlock.RequestDelete();
                    GameObject.Destroy(targetBlock.gameObject);
                    GameObject.Destroy(targetDeleter.gameObject);
                    
                    if (blockController.transform.childCount == 1)
                    {
                        Debug.Log("Default situation");
                        DefaultPotential();
                    }
                    else {
                        Debug.Log("Regular delete");
                        Block[] relatedBlocks = blockController.GetRelatedBlocks(gridPosition);
                        potentialController.RequestPotentialBlock(gridPosition, relatedBlocks);
                    }

                    potentialController.SetVisibility(false);
                }
                break;
        }

        CameraControl();
    }

    /*
        Creates a new potential at default position
        */
    public void DefaultPotential() {
        potentialController.RequestPotentialBlock(new Vector3(1, 0, 0));
    }

    /*
        Updates next model block to put in scene
        */
    public void SetTool(GameObject o) {
        blockPrimitive = o;
        Block mb = o.GetComponent<Block>();
        potentialController.UpdatePotentialBlocksVisibility(mb);
        //preview
        GameObject.Destroy(blockPreview);
        previewParts = blockPrimitive.GetComponent<Block>().parts;

        blockPreview = new GameObject("preview");
        Material previewMaterial = Resources.Load("previewMaterial") as Material;

        foreach (GameObject part in previewParts)
        {
            Debug.Log("Instantiate preview");
            GameObject newPart = Instantiate(part);
            newPart.name = "part";
            newPart.transform.SetParent(blockPreview.transform);
            newPart.transform.localScale = Vector3.one * scale;
            newPart.GetComponent<Renderer>().material = previewMaterial;
        }
    }

    public void SetMode(string mode) {
        this.mode = mode;
        switch (mode) {
            case "create":
                deleterController.ShowChilds(false);
                potentialController.UpdatePotentialBlocksVisibility(blockPrimitive.GetComponent<Block>());
                break;
            case "delete":
                deleterController.ShowChilds(true);
                potentialController.SetVisibility(false);
                break;
        }
        
    }

    public void CameraControl(){
        if (Input.GetMouseButton(1))
        {
            //Debug.Log("Camera rotate");
            Vector3 center = cam.position + cam.forward;
            Camera.main.transform.RotateAround(center, Vector3.up, Input.GetAxis("Mouse X"));
            Camera.main.transform.RotateAround(center, Camera.main.transform.right, -Input.GetAxis("Mouse Y"));

        }

        if (Input.GetMouseButton(2))
        {
            //Debug.Log("Camera pan");
            cam.Translate(cam.right * -1 * Input.GetAxis("Mouse X") * panSensitivity, Space.World);
            cam.Translate(cam.up * -1 * Input.GetAxis("Mouse Y") * panSensitivity, Space.World);
        }

        float mouseWheel = Input.GetAxis("Mouse ScrollWheel");
        if (mouseWheel != 0.0)
        {
            Debug.Log("Cam zoom");
            cam.Translate(cam.forward * mouseWheel * zoomSensitivity, Space.World);
        }
    }
}
