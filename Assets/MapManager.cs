using DG.Tweening;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : ManagerBase<MapManager>
{
    public MapData MapData;

    public Tilemap tilemap;

    // 저장: Tilemap → ScriptableObject
    [ContextMenu("Save Tiles")]
    public void SaveToSO()
    {
        MapData.Tiles = new ();
        var tiles = MapData.Tiles;
        var bounds = tilemap.cellBounds;
        foreach (var pos in bounds.allPositionsWithin)
        {
            TileBase tile = tilemap.GetTile(pos);
            if (tile != null)
            {
                Vector3 worldPos = tilemap.CellToWorld(pos) + tilemap.tileAnchor;

                tiles.Add(new TileData
                {
                    position = new Vector2(worldPos.x, worldPos.y),
                    tileName = tile.name
                });
            }
        }

        Debug.Log($"Saved {tiles.Count} tiles to SO.");
    }

    [ContextMenu("Load Tiles")]
    public void LoadFromSO()
    {
        tilemap.ClearAllTiles();
        var tiles = MapData.Tiles;

        foreach (var tileData in tiles)
        {
            Vector3Int cellPos = tilemap.WorldToCell(new Vector3(tileData.position.x, tileData.position.y, 0));
            TileBase tile = Resources.Load<TileBase>($"Tiles/{tileData.tileName}");
            if (tile != null)
            {
                tilemap.SetTile(cellPos, tile);
            }
            else
            {
                Debug.LogWarning($"Tile '{tileData.tileName}' not found in Resources/Tiles/");
            }
        }

        Debug.Log($"Loaded {tiles.Count} tiles from SO.");
    }
}
