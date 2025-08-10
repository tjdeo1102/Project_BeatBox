using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == (int)LayerType.Ground)
        {
            foreach (var contact in collision.contacts)
            {
                var normal = contact.normal;
                // 벽이나 천장에 부딪히면 사망
                if (Vector2.Angle(normal, Vector2.down) < 45f ||
                    Vector2.Angle(normal, Vector2.left) < 45f ||
                    Vector2.Angle(normal, Vector2.right) < 45f)
                {
                    InGameLoop.Instance.PlayerDie();
                }
            }
        }
    }
}
