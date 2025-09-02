using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    public Vector2 Velocity;
    public float DestroyTime = 3f;

    private Rigidbody2D m_rb;
    private CancellationTokenSource m_cancelToken;
    void OnEnable()
    {
        m_rb = GetComponent<Rigidbody2D>();
        // 토큰 제어로 Task 중지 제어
        m_cancelToken = new CancellationTokenSource();
        RemoveTask(m_cancelToken.Token).Forget();
    }

    private void FixedUpdate()
    {
        m_rb.MovePosition(m_rb.position + Velocity * Time.fixedDeltaTime);
    }

    private async UniTaskVoid RemoveTask(CancellationToken token)
    {
        var pool = ObjectPoolManager.Instance;
        if (pool == null) Destroy(gameObject);
        else
        {
            await UniTask.WaitForSeconds(DestroyTime, cancellationToken: token);
            pool.ReturnObject(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (m_cancelToken != null)
        {
            m_cancelToken.Cancel();
            m_cancelToken.Dispose();
            m_cancelToken = null;
        }
    }
}
