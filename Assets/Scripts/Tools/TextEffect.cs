using DG.Tweening;
using TMPro;
using UnityEngine;

public class TextEffect
{
    /// <summary>
    /// 전체 텍스트를 처음부터 타자기 효과
    /// </summary>
    public static void TypeWriteEffect(string fullText, TextMeshProUGUI textMesh, float duration)
    {
        textMesh.maxVisibleCharacters = 0;
        textMesh.text = fullText;

        DOTween.To(
            () => textMesh.maxVisibleCharacters,
            x => textMesh.maxVisibleCharacters = x,
            fullText.Length,
            duration
        ).SetLink(textMesh.gameObject);
    }

    /// <summary>
    /// 기존 텍스트는 유지하고, 추가된 부분만 타자기 효과
    /// </summary>
    public static void AppendTypeWriteEffect(string addedText, TextMeshProUGUI textMesh, float duration)
    {
        int currentLength = textMesh.text.Length;
        string currentText = textMesh.text;

        // 최종 텍스트 = 기존 + 추가
        string fullText = currentText + addedText;
        textMesh.text = fullText;

        // 기존 글자까지는 이미 보이도록 하고
        textMesh.maxVisibleCharacters = currentLength;

        // 추가된 부분만 효과 적용
        DOTween.To(
            () => textMesh.maxVisibleCharacters,
            x => textMesh.maxVisibleCharacters = x,
            fullText.Length,
            duration
        ).SetLink(textMesh.gameObject);
    }
}
