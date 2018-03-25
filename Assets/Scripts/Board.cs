using UnityEngine;
using System;

public class Board : MonoBehaviour
{
	#region Singleton
	public static Board Instance;
	private void Awake() {
		Instance = this;
	}
	#endregion

    public string prefabPlacementObject = "Tower";
    public GameObject prefabOK;
    public GameObject prefabFail;
	public LayerMask layerMask;
	public int towerCost = 250;

    // Store which grids are in use
    int[,] usedSpace;
    float gridSize = 1.0f;

    GameObject placementObject = null;
    GameObject areaObject = null;

    bool mouseClick = false;
    Vector3 lastPos;


	void Start() {
		// initialize 2D grid array with grid size
        Vector3 slots = GetComponent<Renderer>().bounds.size / gridSize;
		usedSpace = new int[Mathf.CeilToInt(slots.x), Mathf.CeilToInt(slots.z)];
        for (var x = 0; x < Mathf.CeilToInt(slots.x); x++) {
            for (var z = 0; z < Mathf.CeilToInt(slots.z); z++) {
                usedSpace[x, z] = 0;
            }
        }
    }

    void Update() {
        Vector3 point;

        // Get target under mouse
        if (GetObjectAtMouse(out point)) {
            Vector3 halfSlots = GetComponent<Renderer>().bounds.size / 2.0f;

            // get grid position at mouse position
            int x = (int)Math.Round(Math.Round(point.x - transform.position.x + halfSlots.x - gridSize / 2.0f) / gridSize);
            int z = (int)Math.Round(Math.Round(point.z - transform.position.z + halfSlots.z - gridSize / 2.0f) / gridSize);

            // Calculate the quantized world coordinates to place the object
            point.x = (float)(x) * gridSize - halfSlots.x + transform.position.x + gridSize / 2.0f;
            point.z = (float)(z) * gridSize - halfSlots.z + transform.position.z + gridSize / 2.0f;

            // Create an object to see if this area is available for building
            // Re-instantiate only when the slot has changed or the object not instantiated at all
            if (lastPos.x != x || lastPos.z != z || areaObject == null){
                lastPos.x = x;
                lastPos.z = z;
                if (areaObject != null){
                    Destroy(areaObject);
                }
                areaObject = (GameObject)Instantiate(usedSpace[x, z] == 0 ? prefabOK : prefabFail, point, Quaternion.identity);
				areaObject.SetActive(true);
            }

            // On left click, insert the object to the area and mark it as "used"
            if (Input.GetMouseButtonDown(0) && mouseClick == false) {
                mouseClick = true;
                // Place the object
                if (usedSpace[x, z] == 0) {
					if (GameData.Gold >= towerCost) {
						// Buy a tower and place it
						usedSpace[x, z] = 1;
						GameData.Gold -= towerCost;
						ObjectPooler.Instance.SpawnFromPool(prefabPlacementObject, point, Quaternion.identity).GetComponent<Tower>().SetGridPos(new Vector2(x, z));
						areaObject.SetActive(false);
					}
					else // not enough gold
						UI_Gold.Instance.Flash();
                }
            }
            else if (!Input.GetMouseButtonDown(0)) {
                mouseClick = false;
            }

        }
		// else no valid target
		else {
            if (placementObject) {
                Destroy(placementObject);
                placementObject = null;
            }
            if (areaObject) {
                Destroy(areaObject);
                areaObject = null;
            }
        }
    }

	// Get first nearest object under mouse
	bool GetObjectAtMouse(out Vector3 point) {
		if (GameData.isGameOver) {
			point = Vector3.zero;
			return false;
		}
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo = new RaycastHit();
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, layerMask)) {
            if (hitInfo.collider == GetComponent<Collider>()) {
                point = hitInfo.point;
                return true;
            }
        }
        point = Vector3.zero;
        return false;
    }

	// free up previously occupied grid
	public void FreeGrid(Vector2 pos) {
		usedSpace[(int)pos.x, (int)pos.y] = 0;
	}
}