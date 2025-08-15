using UnityEngine;

public class Note : MonoBehaviour
{
    public ColorType ColorType;
    public SpriteRenderer Renderer;

    public void Init(int sortingOrder)
    {
        Renderer.sortingOrder = sortingOrder;
    }

    public void RemoveNote()
    {
        Destroy(gameObject);
    }
}
