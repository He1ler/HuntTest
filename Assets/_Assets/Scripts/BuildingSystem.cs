using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem instance;
    public GridLayout gridLayout;
    Grid grid;
    [SerializeField] Tilemap mainTilemap;
    public DragAndSnapObject objectToPlace;
    public List<DragAndSnapObject> gunParts = new List<DragAndSnapObject>();
    public GameObject[] disableOnStartGame;
    public GameObject[] enableOnStartGame;
    //Vector3 position;
    public bool isStarted = false;
    [SerializeField] CinemachineVirtualCamera main_cam;
    //private void Awake()
    //{
    //    instance = this;
    //    grid = gridLayout.gameObject.GetComponent<Grid>();
    //}
    //public void PlaceGunPart(Vector3 position, DragAndSnapObject gunPart)
    //{
    //    objectToPlace = gunPart;
    //    Vector3 pos = SnapCoordinateToGrid(position);
    //    objectToPlace.transform.position = pos;
    //    gunParts.Add(gunPart);
    //    isStarted = true;
    //    UIManager.instance.ChangeButton(isStarted);
    //}
    //public Vector3 SnapCoordinateToGrid(Vector3 pos)
    //{
    //    Vector3Int cellPos = gridLayout.WorldToCell(pos);
    //    pos = grid.GetCellCenterWorld(cellPos);
    //    pos.z -= .5f;
    //    return pos;
    //}
    //public void DiselectGunPart(DragAndSnapObject gunPart)
    //{
    //    gunParts.Remove(gunPart);
    //    if(gunParts.Count == 0)
    //    {
    //        isStarted = false;
    //        UIManager.instance.ChangeButton(isStarted);
    //    }
    //    objectToPlace = null;
    //}
    #region Old
    /*public static Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            return raycastHit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }*/

    /*public void InitializeWithObject(GameObject gunPart)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit2, Mathf.Infinity))
        {
            position = hit2.point;
        }
        Vector3 pos = SnapCoordinateToGrid(position);
        GameObject go = Instantiate(gunPart, position, Quaternion.identity);
        objectToPlace = go.GetComponent<GunPart>();
        
    }*/

    /*private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
{
    TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
    int counter = 0;

    foreach(Vector3Int vector3Int in area.allPositionsWithin)
    {
        Vector3Int pos = new Vector3Int(vector3Int.x, vector3Int.y, 0);
        array[counter] = tilemap.GetTile(pos);
        counter++;
    }
    return array;
}   */

    /*private bool CanBePlaced(GunPart gunPart)
    {
        BoundsInt area = new BoundsInt();
        area.position = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
        area.size = gunPart.vectorSize;
        
        TileBase[] baseArray = GetTilesBlock(area, mainTilemap);
        
        foreach(TileBase b in baseArray)
        {
            if(b == whiteTile)
            {
                return false;
            }
        }
        return true;
    }*/

    //public void TakeArea(Vector3Int start, Vector3Int size)
    //{
    //    mainTilemap.BoxFill(start, whiteTile, start.x, start.y, start.x + size.x, start.y + size.y);
    //}
    #endregion
}