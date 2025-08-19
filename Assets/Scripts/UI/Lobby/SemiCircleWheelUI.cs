using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(RectTransform))]
public class SemiCircleWheelUI : MonoBehaviour
{
    [Header("Require Setting")]
    public int SelectedIndex = 0;
    [SerializeField] private int m_visibleCount = 5;
    [SerializeField] private float m_radius = 150f;

    private List<RectTransform> m_itemElements;
    private RectTransform m_selfRect;
    private void Start()
    {
        m_itemElements = new();
        m_selfRect = GetComponent<RectTransform>();
        RenderWheel();
    }
    public void AddItem(List<RectTransform> list)
    {
        m_itemElements = list;
        RenderWheel();
    }

    public void ItemUp()
    {
        SelectedIndex = Mathf.Clamp(SelectedIndex - 1, 0, m_itemElements.Count);
        RenderWheel();
    }

    public void ItemDown()
    {
        SelectedIndex = Mathf.Clamp(SelectedIndex + 1, 0, m_itemElements.Count);
        RenderWheel();
    }

    void RenderWheel()
    {
        int half = m_visibleCount / 2;

        for (int i = 0; i < m_itemElements.Count; i++)
        {
            int offset = i - SelectedIndex;
            var item = m_itemElements[i];

            if (Mathf.Abs(offset) > half)
            {
                item.gameObject.SetActive(false);
            }
            else
            {
                float angle = (offset / (float)half) * Mathf.PI / 2f; // -90° ~ +90°
                float x = -m_radius * Mathf.Cos(angle); // 좌우
                float y = m_radius * Mathf.Sin(angle);  // 위/아래

                // 중앙 기준으로 위치 조정
                item.position = m_selfRect.position + new Vector3(x, y, 0);
                
                item.gameObject.SetActive(true);
            }
        }
    }
}
