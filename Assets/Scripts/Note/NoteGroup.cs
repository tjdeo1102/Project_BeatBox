using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public class NoteGroup : MonoBehaviour
{
    public Queue<Note> Notes;
    public Vector2 MinNoteSize = Vector2.one;
    public Vector2 NoteDistance = Vector2.one;
    public MapManager Manager;
    public HitJudgeMode CurrentMode;

    private float m_lastHitTime = 0f;
    private void Start()
    {
        Init();
        Manager = MapManager.Instance;
    }

    public void Init()
    {
        var notes = GetComponentsInChildren<Note>();

        transform.position += (Vector3)NoteDistance * (notes.Length - 1);

        for (int i = 0; i < notes.Length; i++)
        {
            var n = notes[i];
            n.transform.localScale = (Vector3)(MinNoteSize + NoteDistance * i);
            n.Init(-i);
        }

        Notes = new();
        for (int i = notes.Length - 1; i >= 0; i--)
        {
            Notes.Enqueue(notes[i]);
        }
    }

    public bool TryHit(ColorType type, float hitDistance, float attackDistance, out bool IsCombo)
    {
        IsCombo = false;
        if (Manager == null || Manager.MapData == null) return false;

        if (Notes.TryPeek(out var note) && note.ColorType == type)
        {
            string result = "";

            if (CurrentMode == HitJudgeMode.FirstHit)
            {
                result = JudgeByDistance(hitDistance, attackDistance);
                m_lastHitTime = Time.unscaledTime;
            }
            else if (CurrentMode == HitJudgeMode.ComboHIt)
            {
                float currentTime = Time.unscaledTime;
                if (m_lastHitTime > 0f)
                    result = JudgeByTime(currentTime, m_lastHitTime);
                else
                {
                    result = "Miss";
                }
                m_lastHitTime = currentTime; // 다음 콤보 판정 대비
            }

            Debug.Log($"Hit Result: {result}");
            if (result == "Miss") Destroy(gameObject);

            note.RemoveNote();
            Notes.Dequeue();
            if (Notes.Count > 0 && CurrentMode == HitJudgeMode.FirstHit)
            {
                CurrentMode = HitJudgeMode.ComboHIt;
                IsCombo = true;
            }
            return true;
        }
        else return false;
    }

    private string JudgeByDistance(float hitDistance, float attackDistance)
    {
        var data = Manager.MapData;
        float distance = Mathf.Max(0f, hitDistance - MinNoteSize.x / 2f);
        var attackRange = attackDistance - MinNoteSize.x / 2;

        var perfect = attackRange * data.HitWindow.perfect;
        var great = attackRange * data.HitWindow.great;
        var good = attackRange * data.HitWindow.good;
        //Debug.Log($"{distance} {good}");
        if (distance <= perfect)
            return $"Perfect {data.ScorePerPerfect * data.ScoreMultiplier.perfect}";
        else if (distance <= great)
            return $"Great {data.ScorePerPerfect * data.ScoreMultiplier.great}";
        else if (distance <= good)
            return $"Good {data.ScorePerPerfect * data.ScoreMultiplier.good}";
        else
            return "Miss";
    }

    private string JudgeByTime(float currentTime, float m_lastHitTime)
    {
        var data = Manager.MapData;
        var deltaTime = Mathf.Abs(currentTime - m_lastHitTime);

        var perfect = data.ComboHitTime * data.HitWindow.perfect;
        var great = data.ComboHitTime * data.HitWindow.great;
        var good = data.ComboHitTime * data.HitWindow.good;

        if (deltaTime <= perfect) return $"Perfect {data.ScorePerPerfect * data.ScoreMultiplier.perfect}";
        if (deltaTime <= great) return $"Great {data.ScorePerPerfect * data.ScoreMultiplier.great}";
        if (deltaTime <= good) return $"Good {data.ScorePerPerfect * data.ScoreMultiplier.good}";

        return "Miss";
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == (int)LayerType.Player)
        {
            InGameLoop.Instance.PlayerDie();
        }
    }
}
