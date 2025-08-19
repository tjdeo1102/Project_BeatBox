using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using System.Linq;
using Firebase.Extensions;
using Firebase.Database;
using Google.MiniJSON;
using System;

public class SongManager : ManagerBase<SongManager>
{
    public SongData SongData;

    public Tilemap Tilemap;
    public Tilemap ConfinerTilemap;

    private List<NoteGroup> m_groups;

    public void Save()
    {
        try
        {
            // 맵 타일 저장
            SongData.Tiles = new();

            BoundsInt bounds = Tilemap.cellBounds;
            foreach (Vector3Int pos in bounds.allPositionsWithin)
            {
                TileBase tile = Tilemap.GetTile(pos);
                if (tile != null)
                {
                    SongData.Tiles.Add(new TileData
                    {
                        position = new Vector2Int(pos.x, pos.y),
                        tileName = tile.name
                    });
                }
            }

            // 카메라 범위 제한 타일 저장
            SongData.ConfinerTiles = new();
            bounds = ConfinerTilemap.cellBounds;
            foreach (Vector3Int pos in bounds.allPositionsWithin)
            {
                TileBase tile = ConfinerTilemap.GetTile(pos);
                if (tile != null)
                {
                    SongData.ConfinerTiles.Add(new TileData
                    {
                        position = new Vector2Int(pos.x, pos.y),
                        tileName = ColorType.Red.ToString()
                    });
                }
            }

            // 노트 저장
            SongData.NoteGroups = new();

            var groups = transform.GetComponentsInChildren<NoteGroup>();
            foreach (NoteGroup group in groups)
            {
                SongData.NoteGroups.Add(new NoteData
                {
                    position = group.transform.position,
                    notes = group.NoteList.ToArray()
                });
            }

            string json = JsonUtility.ToJson(SongData, true);
            // 로컬 저장 로직
            var SavePath = Path.Combine(Application.persistentDataPath, $"{SongData.ID}_{SongData.SongTitle}.json");
            File.WriteAllText(SavePath, json);

            //// Firebase 저장 로직 (나중에 사용)
            //var manager = FirebaseManager.Instance;
            //if (manager != null
            //    && manager.CurrentUser != null)
            //{
            //    manager.DBRef.Child("users").Child(manager.CurrentUser.UserId).SetRawJsonValueAsync(json);
            //}
            //else throw new InvalidOperationException("Save Error");
            Debug.Log($"타일맵 저장 완료!");
        }
        catch (System.Exception)
        {
            SceneLoadManager.Instance.LoadScene((int)SceneIndex.Login);
            throw;
        }
        
    }

    [ContextMenu("Load")]
    public void Load(string title)
    {
        try
        {
            // 로컬 저장 로직
            var path = Path.Combine(Application.persistentDataPath, $"{title}.json");
            if (!File.Exists(path))
            {
                Debug.LogWarning($"저장된 맵이 없습니다. {path}");
                throw new FileNotFoundException();
            }
            string json = File.ReadAllText(path);
            JsonUtility.FromJsonOverwrite(json, SongData);
            MapTileLoad();

            //// Firebase 로드
            //var manager = FirebaseManager.Instance;
            //if (manager != null
            //    && manager.CurrentUser != null)
            //{
            //    manager.DBRef.Child("users").Child(manager.CurrentUser.UserId).GetValueAsync().ContinueWithOnMainThread(task =>
            //    {
            //        if (task.IsCompleted)
            //        {
            //            DataSnapshot snapshot = task.Result;
            //            if (snapshot.Exists)
            //            {
            //                JsonUtility.FromJsonOverwrite(snapshot.GetRawJsonValue(), SongData);
            //                MapTileLoad();
            //            }
            //            else
            //            {
            //                Debug.Log("데이터 없음");
            //            }
            //        }
            //        else
            //        {
            //            throw new System.Exception("Map Load Error");
            //        }
            //    });
            //}
        }
        catch (System.Exception)
        {
            SceneLoadManager.Instance.LoadScene((int)SceneIndex.Login);
        }

    }

    private void MapTileLoad()
    {
        // 맵 초기화
        if (SongData.Tiles.Count > 0
            && SongData.ConfinerTiles.Count > 0)
        {
            Tilemap.ClearAllTiles();
            ConfinerTilemap.ClearAllTiles();

            // 맵 타일 로드
            var tiles = Addressables.LoadAssetsAsync<TileBase>("Tiles").WaitForCompletion();
            if (tiles == null) return;
            Dictionary<string, TileBase> dic = new();
            foreach (var item in tiles)
            {
                dic[item.name] = item;
            }

            foreach (TileData info in SongData.Tiles)
            {
                if (dic.TryGetValue(info.tileName, out var tile))
                {
                    Tilemap.SetTile(new Vector3Int(info.position.x, info.position.y, 0), tile);
                }
            }
        }

        m_groups = new();
        if (SongData.NoteGroups.Count > 0)
        {
            // 노트 로드
            var note = Addressables.LoadAssetAsync<GameObject>("NoteGroup").WaitForCompletion();
            foreach (NoteData data in SongData.NoteGroups)
            {

                var obj = Instantiate(note, data.position, Quaternion.identity, transform);
                if (obj.TryGetComponent<NoteGroup>(out var group))
                {
                    group.NoteList = data.notes.ToList();
                    group.Init();
                    m_groups.Add(group);
                }

            }
        }

        Debug.Log("타일맵 로드 완료!");
        IsReady = true;
    }

    public void ResetMap()
    {
        if (m_groups != null)
        {
            foreach (var g in m_groups)
            {
                g.ResetNote();
            }
        }
    }
}
