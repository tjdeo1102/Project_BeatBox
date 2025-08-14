using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundShift : MonoBehaviour
{
    public SpriteRenderer background;
    public float colorDuration = 5f;

    private Sequence m_seq;
    void Start()
    {
        background.material = new Material(background.material);
        StartBackgroundColorSequence();
    }

    private void StartBackgroundColorSequence()
    {
        m_seq = DOTween.Sequence();

        Color[] colors = new Color[]
        {
            new Color(0x85 / 255f, 0xDA / 255f, 0xEB / 255f, 1f), // #85daeb
            new Color(0x5F / 255f, 0xC9 / 255f, 0xE7 / 255f, 1f), // #5fc9e7
            new Color(0x5F / 255f, 0xA1 / 255f, 0xE7 / 255f, 1f), // #5fa1e7
            new Color(0x5F / 255f, 0x6E / 255f, 0xE7 / 255f, 1f), // #5f6ee7
            new Color(0x4C / 255f, 0x60 / 255f, 0xAA / 255f, 1f), // #4c60aa
            new Color(0x44 / 255f, 0x47 / 255f, 0x74 / 255f, 1f), // #444774
            new Color(0x32 / 255f, 0x31 / 255f, 0x3B / 255f, 1f), // #32313b
            new Color(0x46 / 255f, 0x3C / 255f, 0x5E / 255f, 1f), // #463c5e
            new Color(0x5D / 255f, 0x47 / 255f, 0x76 / 255f, 1f), // #5d4776
            new Color(0x85 / 255f, 0x53 / 255f, 0x95 / 255f, 1f), // #855395
            new Color(0xAB / 255f, 0x58 / 255f, 0xA8 / 255f, 1f), // #ab58a8
            new Color(0xCA / 255f, 0x60 / 255f, 0xAE / 255f, 1f), // #ca60ae
            new Color(0xF3 / 255f, 0xA7 / 255f, 0x87 / 255f, 1f), // #f3a787
            new Color(0xF5 / 255f, 0xDA / 255f, 0xA7 / 255f, 1f), // #f5daa7
            new Color(0x8D / 255f, 0xD8 / 255f, 0x94 / 255f, 1f), // #8dd894
            new Color(0x5D / 255f, 0xC1 / 255f, 0x90 / 255f, 1f), // #5dc190
            new Color(0x4A / 255f, 0xB9 / 255f, 0xA3 / 255f, 1f), // #4ab9a3
            new Color(0x45 / 255f, 0x93 / 255f, 0xA5 / 255f, 1f), // #4593a5
            new Color(0x5E / 255f, 0xFD / 255f, 0xF7 / 255f, 1f), // #5efdf7
            new Color(0xFF / 255f, 0x5D / 255f, 0xCC / 255f, 1f), // #ff5dcc
            new Color(0xFD / 255f, 0xFE / 255f, 0x89 / 255f, 1f), // #fdfe89
            new Color(0xFF / 255f, 0xFF / 255f, 0xFF / 255f, 1f), // #ffffff
};


        for (int i = 0; i < colors.Length; i++)
        {
            Color from = colors[i];
            Color to = colors[(i + 1) % colors.Length];

            m_seq.AppendCallback(() =>
            {
                background.color = from;
            });

            m_seq.Join(DOTween.To(
                () => background.color,
                x => background.color = x,
                to,
                colorDuration
            ));

        }

        m_seq.SetLoops(-1); // 무한 반복
    }

    private void OnDestroy()
    {
        if (m_seq != null && m_seq.IsActive())
            m_seq.Kill();
    }
}


