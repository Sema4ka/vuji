using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Generator : MonoBehaviour
{
    public Room[] RoomPrefabs;
    public Room StartingRoom;
    public int Width;
    public int Height; 

    private Room[,] spawnedRooms;
    //IEnumerator
    private void Start()
    {
        spawnedRooms = new Room[Width, Height];
        spawnedRooms[Width / 2, 0] = StartingRoom;
        Debug.Log("" +Width / 2 + " " + 0);


        for (int i = 0; i < Width * Height - 1; i++)
        {
            //yield return new WaitForSecondsRealtime(0.5f);
            PlaceOneRoom();
        }
    }

    private void PlaceOneRoom()
    {
        HashSet<Vector2Int> vacantPlaces = new HashSet<Vector2Int>();
        for (int x = 0; x < spawnedRooms.GetLength(0); x++)
        {
            for (int y = 0; y < spawnedRooms.GetLength(1); y++)
            {
                if (spawnedRooms[x, y] == null) continue;

                int maxX = spawnedRooms.GetLength(0) - 1;
                int maxY = spawnedRooms.GetLength(1) - 1;

                if (x > 0 && spawnedRooms[x - 1, y] == null) vacantPlaces.Add(new Vector2Int(x - 1, y));
                if (y > 0 && spawnedRooms[x, y - 1] == null) vacantPlaces.Add(new Vector2Int(x, y - 1));
                if (x < maxX && spawnedRooms[x + 1, y] == null) vacantPlaces.Add(new Vector2Int(x + 1, y));
                if (y < maxY && spawnedRooms[x, y + 1] == null) vacantPlaces.Add(new Vector2Int(x, y + 1));
            }
        }
        Room newRoom = Instantiate(RoomPrefabs[Random.Range(0, RoomPrefabs.Length)]);

        Vector2Int position = vacantPlaces.ElementAt(Random.Range(0, vacantPlaces.Count));
        newRoom.transform.position = new Vector3(position.x +1, position.y+1, 0) * Random.Range(12, 20);
        spawnedRooms[position.x, position.y] = newRoom;
        Debug.Log(""+ position.x+" " + position.y);


    }

    //private bool ConnectToSomething(Room room, Vector2Int p)
    //{
    //    int maxX = spawnedRooms.GetLength(0) - 1;
    //    int maxY = spawnedRooms.GetLength(1) - 1;

    //    List<Vector2Int> neighbours = new List<Vector2Int>();

    //    if (room.DoorU != null && p.y < maxY && spawnedRooms[p.x, p.y + 1]?.DoorD != null) neighbours.Add(Vector2Int.up);
    //    if (room.DoorD != null && p.y > 0 && spawnedRooms[p.x, p.y - 1]?.DoorU != null) neighbours.Add(Vector2Int.down);
    //    if (room.DoorR != null && p.x < maxX && spawnedRooms[p.x + 1, p.y]?.DoorL != null) neighbours.Add(Vector2Int.right);
    //    if (room.DoorL != null && p.x > 0 && spawnedRooms[p.x - 1, p.y]?.DoorR != null) neighbours.Add(Vector2Int.left);

    //    if (neighbours.Count == 0) return false;

    //    Vector2Int selectedDirection = neighbours[Random.Range(0, neighbours.Count)];
    //    Room selectedRoom = spawnedRooms[p.x + selectedDirection.x, p.y + selectedDirection.y];

    //    if (selectedDirection == Vector2Int.up)
    //    {
    //        room.DoorU.SetActive(false);
    //        selectedRoom.DoorD.SetActive(false);
    //    }
    //    else if (selectedDirection == Vector2Int.down)
    //    {
    //        room.DoorD.SetActive(false);
    //        selectedRoom.DoorU.SetActive(false);
    //    }
    //    else if (selectedDirection == Vector2Int.right)
    //    {
    //        room.DoorR.SetActive(false);
    //        selectedRoom.DoorL.SetActive(false);
    //    }
    //    else if (selectedDirection == Vector2Int.left)
    //    {
    //        room.DoorL.SetActive(false);
    //        selectedRoom.DoorR.SetActive(false);
    //    }

    //    return true;
    //}
}