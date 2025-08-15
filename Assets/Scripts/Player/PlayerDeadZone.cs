using UnityEngine;

public class PlayerDeadZone : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (MapManager.Instance == null || MapManager.Instance.IsReady == false) return;

        if (collision.gameObject.layer == (int)LayerType.Player)
        {
            InGameLoop.Instance.PlayerDie();
        }
    }
}
