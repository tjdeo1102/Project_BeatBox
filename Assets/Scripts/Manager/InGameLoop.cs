using UnityEngine;

public class InGameLoop : ManagerBase<InGameLoop>
{
    public MapManager MapManager;
    public PlayerController Player;

    public void PlayerDie()
    {
        Debug.Log("Player Die");
    }
}
