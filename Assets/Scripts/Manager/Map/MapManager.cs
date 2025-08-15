using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using System.Linq;

public class MapManager : ManagerBase<MapManager>
{
    public MapData MapData;

    public Tilemap Tilemap;
    public Tilemap ConfinerTilemap;

#if UNITY_EDITOR
    string SavePath => Path.Combine(Application.dataPath, MapData.ID + ".json");
#else
string SavePath => Path.Combine(Application.persistentDataPath, MapData.ID + ".json");
#endif

    [ContextMenu("Save")]
    public void Save()
    {
        // 맵 타일 저장
        MapData.Tiles = new ();

        BoundsInt bounds = Tilemap.cellBounds;
        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            TileBase tile = Tilemap.GetTile(pos);
            if (tile != null)
            {
                MapData.Tiles.Add(new TileData
                {
                    position = new Vector2Int(pos.x, pos.y),
                    tileName = tile.name
                });
            }
        }

        // 카메라 범위 제한 타일 저장
        MapData.ConfinerTiles = new ();
        bounds = ConfinerTilemap.cellBounds;
        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            TileBase tile = ConfinerTilemap.GetTile(pos);
            if (tile != null)
            {
                MapData.ConfinerTiles.Add(new TileData
                {
                    position = new Vector2Int(pos.x, pos.y),
                    tileName = ColorType.Red.ToString()
                });
            }
        }

        // 노트 저장
        MapData.NoteGroups = new ();

        var groups = transform.GetComponentsInChildren<NoteGroup>();
        foreach (NoteGroup group in groups)
        {
            MapData.NoteGroups.Add(new NoteData
            {
                position = group.transform.position,
                notes = group.NoteList.ToArray()
            });
        }

        string json = JsonUtility.ToJson(MapData, true);
        File.WriteAllText(SavePath, json);

        Debug.Log($"타일맵 저장 완료! 경로: {SavePath}");

        /*
        // Firebase 저장 로직 (나중에 사용)
        string json = JsonUtility.ToJson(mapData);
        dbRef.Child("maps").Child(mapId).SetRawJsonValueAsync(json)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                    Debug.Log("타일맵 Firebase 저장 완료!");
            });
        */
    }

    [ContextMenu("Load")]
    public void Load()
    {
        if (!File.Exists(SavePath))
        {
            Debug.LogWarning("저장된 맵이 없습니다.");
            return;
        }

        string json = File.ReadAllText(SavePath);
        JsonUtility.FromJsonOverwrite(json,MapData);

        // 맵 초기화
        Tilemap.ClearAllTiles();
        //ConfinerTilemap.ClearAllTiles();
        // 맵 타일 로드
        var tiles = Addressables.LoadAssetsAsync<TileBase>("Tiles").WaitForCompletion();
        if (tiles == null) return;
        Dictionary<string, TileBase> dic = new();
        foreach (var item in tiles)
        {
            dic[item.name] = item;
        }

        foreach (TileData info in MapData.Tiles)
        {
            if (dic.TryGetValue(info.tileName,out var tile))
            {
                Tilemap.SetTile(new Vector3Int(info.position.x, info.position.y, 0), tile);
            }
        }

        //foreach (TileData info in MapData.ConfinerTiles)
        //{
        //    if (dic.TryGetValue(info.tileName, out var tile))
        //    {
        //        ConfinerTilemap.SetTile(new Vector3Int(info.position.x, info.position.y, 0), tile);
        //    }
        //}
        //CompositeCollider2D composite = ConfinerTilemap.GetComponent<CompositeCollider2D>();
        //composite.GenerateGeometry();

        // 노트 로드
        var note = Addressables.LoadAssetAsync<GameObject>("NoteGroup").WaitForCompletion();
        foreach(NoteData data in MapData.NoteGroups)
        {

            var obj = Instantiate(note, data.position ,Quaternion.identity,transform);
            if (obj.TryGetComponent<NoteGroup>(out var group))
            {
                group.NoteList = data.notes.ToList();
                group.Init();
            }
        }

        Debug.Log("타일맵 로드 완료!");
        IsReady = true;
        /*
        // Firebase 로드 로직 (나중에 사용)
        dbRef.Child("maps").Child(mapId).GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    if (snapshot.Exists)
                    {
                        string json = snapshot.GetRawJsonValue();
                        TilemapData mapData = JsonUtility.FromJson<TilemapData>(json);
                        // ... 타일맵 적용 ...
                    }
                }
            });
        */
    }

    private void Start()
    {
        StartInit();
    }

    public override void StartInit()
    {
        base.StartInit();
        Load();
    }
}
