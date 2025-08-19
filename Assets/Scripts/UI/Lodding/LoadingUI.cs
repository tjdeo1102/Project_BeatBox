using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{
    public Slider Slider;

    private SceneLoadManager m_manager;

    private void Start()
    {
        if (SceneLoadManager.Instance != null)
            m_manager = SceneLoadManager.Instance;
        
    }

    private void Update()
    {
        if (Slider == null || m_manager == null) return;
        Slider.value = m_manager.SceneLoadProgress;
    }
}
