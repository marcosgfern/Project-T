using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Floors;

public class MinimapRoomLayout : MonoBehaviour {

    public GameObject tilePrefab;
    public float tileSeparationRatio = 1.25f;
    private Dictionary<Coordinate, MinimapRoomTile> tiles;


    void Start() {
        this.tiles = new Dictionary<Coordinate, MinimapRoomTile>();
    }

    public void GenerateTileLayout(Dictionary<Coordinate, Room> roomMap) {
        Clear();

        foreach(KeyValuePair<Coordinate, Room> room in roomMap) {
            GameObject tile = Instantiate(tilePrefab, this.transform);

            Rect tileRect = tile.GetComponent<RectTransform>().rect;
            Vector2 tilePosition = new Vector2(
                room.Key.x * tileRect.width * tileSeparationRatio,
                room.Key.y * tileRect.height * tileSeparationRatio);

            tile.name = room.Key.ToString();
            tile.transform.localPosition = tilePosition;

            MinimapRoomTile tileComponent = tile.GetComponent<MinimapRoomTile>();
            room.Value.SetMinimapTile(tileComponent);
            this.tiles.Add(room.Key, tileComponent);
        }
    }

    private void Clear() {
        this.tiles.Clear();
        foreach(Transform child in this.gameObject.transform) {
            Destroy(child.gameObject);
        }
    }

    public void MoveToRoom(Coordinate coordinate) {
        Vector2 tilePosition = tiles[coordinate].transform.localPosition;

        this.transform.localPosition = new Vector2(tilePosition.x * -1, tilePosition.y * -1);
    }


}
