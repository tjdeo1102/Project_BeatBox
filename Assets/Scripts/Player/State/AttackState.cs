using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "States/Attack")]
public class AttackState : State
{
    [Header("Require Setting")]
    public ColorType ColorType;
    public float AttackDistance;
    public override void Enter()
    {
        base.Enter();
#if UNITY_EDITOR
        Debug.DrawRay(ctrl.transform.position, Vector2.right * AttackDistance, Color.red, 10f);
#endif
        if (ctrl.CurrentComboNote == null)
        {
            var hit = Physics2D.BoxCast(ctrl.transform.position, Vector2.one, 0f, Vector2.right, AttackDistance, 1 << (int)LayerType.Note);
            if (hit.collider != null)
            {
                var noteGroup = hit.transform.GetComponentInParent<NoteGroup>();
                if (noteGroup != null
                    && noteGroup.TryHit(ColorType, hit.distance, AttackDistance, out ctrl.IsCombo))
                {
                    if (ctrl.CurrentComboNote == null && ctrl.IsCombo)
                    {
                        ComboHitEffect(true);
                        ctrl.CurrentComboNote = noteGroup;
                    }
                }
            }
        }
        else
        {
            ctrl.CurrentComboNote.TryHit(ColorType, AttackDistance, AttackDistance, out var isCombo);
        }
        
    }

    public override void Update()
    {
        base.Update();

        // 콤보 히트 상태라면 타이머 체크
        if (ctrl.CurrentComboNote != null)
        {
            var note = ctrl.CurrentComboNote;
            ctrl.ComboHitTimer += Time.unscaledDeltaTime; // timeScale = 0에서도 진행
            if (ctrl.ComboHitTimer >= MapManager.Instance.MapData.ComboHitTime * note.Notes.Count)
            {
                // 타이머 종료 시 상태 전환
                ComboHitEffect(false);
                // 다시 콤보 해제된 상태로 돌리기
                note.CurrentMode = HitJudgeMode.FirstHit;
                ctrl.CurrentComboNote = null;
                ctrl.ComboHitTimer = 0f;
            }
        }
        else
        {
            ctrl.Machine.ChangeState(StateType.Play);
        }
    }

    public void ComboHitEffect(bool isPlay)
    {
        if (isPlay)
        {
            ctrl.ComboHitTimer = 0f;

            // 슬로우 모션 (히트스톱)
            Time.timeScale = 0f;

            // TODO: 배경 어두워지는 이펙트 (DOTween)

            // TODO: 콤보 이펙트 생성

        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}
