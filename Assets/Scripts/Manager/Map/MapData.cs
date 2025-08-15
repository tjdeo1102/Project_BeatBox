using System;
using System.Collections.Generic;
using UnityEngine;

// HitWindow는 (1f ~ 공격범위) 사이에서 공격 인정되는 비율
// 공격범위 (1.5f) 가정 ==> 0.5f: 1.25f 지점아내 히트 1f: 1.5f지점이내 히트 
[Serializable]
public class HitWindow
{
    public float perfect = 0.25f;
    public float great = 0.5f;
    public float good = 1f;
}

[Serializable]
public class ScoreMultiplier
{
    public float perfect = 1.0f;
    public float great = 0.8f;
    public float good = 0.5f;
    public float miss = 0f;
}

[Serializable]
public class TileData
{
    public Vector2Int position;
    public string tileName;
}

[Serializable]
public class NoteData
{
    public Vector2 position;
    public ColorType[] notes;
}

[CreateAssetMenu(fileName = "MapData", menuName = "Scriptable Objects/MapData")]
public class MapData : ScriptableObject
{
    // 추후, RealtimeDB에 올라가며 수정될 내용들

    public int ID;
    public string SongTitle;
    public float ScorePerPerfect;
    // 연속 히트로 없애는 박스인 경우, 연속 히트 인정 시간
    // HitWindow영향받음
    public float ComboHitTime = 0.2f;
    public HitWindow HitWindow;
    public ScoreMultiplier ScoreMultiplier;
    public List<TileData> Tiles;
    public List<TileData> ConfinerTiles;
    public List<NoteData> NoteGroups;
}
