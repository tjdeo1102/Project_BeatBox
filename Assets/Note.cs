using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Note : MonoBehaviour
{
    public ColorType ColorType;
    public SpriteRenderer Renderer;
    private void Awake()
    {
        Renderer = GetComponent<SpriteRenderer>();
    }

    public void Init(int sortingOrder)
    {
        Renderer.sortingOrder = sortingOrder;
    }

    public void RemoveNote()
    {
        Destroy(gameObject);
    }
}
