using NUnit.Framework;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LobbyUIController : ManagerBase<LobbyUIController>
{
    [Header("Require Setting")]
    public InputActionAsset InputActions;
    public SemiCircleWheelUI WheelUI;

    private void Start()
    {
        Debug.Log("persistentDataPath: " + Application.persistentDataPath);
        SwipeInit();
    }

    #region Input: Swipe
    [Header("Input: Swipe")]
    [SerializeField] private float m_swipeDelay = 0.2f;
    private InputAction m_primaryContact;
    private InputAction m_primaryPosition;
    private Vector2 m_startPosition;
    private float m_startSwipeTime = 0f;
    private UnityAction m_upAction;
    private UnityAction m_downAction;

    private void SwipeInit()
    {
        try
        {
            if (InputActions == null || WheelUI == null) throw new NullReferenceException();
            var UIMap = InputActions.FindActionMap("UI", throwIfNotFound: true);
            m_primaryContact = UIMap.FindAction("PrimaryContact", throwIfNotFound: true);
            m_primaryPosition = UIMap.FindAction("PrimaryPosition", throwIfNotFound: true);

            m_upAction += WheelUI.ItemUp;
            m_downAction += WheelUI.ItemDown;

            m_primaryContact.performed += SwipeStart;
            m_primaryPosition.canceled += SwipeEnd;
        }
        catch (Exception)
        {
            Debug.Log("Swipe Init Error");
            throw;
        }
    }
    private void SwipeDestroy()
    {
        try
        {
            if (InputActions == null || WheelUI == null) throw new NullReferenceException();
            var UIMap = InputActions.FindActionMap("UI", throwIfNotFound: true);
            m_primaryContact = UIMap.FindAction("PrimaryContact", throwIfNotFound: true);
            m_primaryPosition = UIMap.FindAction("PrimaryPosition", throwIfNotFound: true);

            m_upAction -= WheelUI.ItemUp;
            m_downAction -= WheelUI.ItemDown;

            m_primaryContact.performed -= SwipeStart;
            m_primaryPosition.canceled -= SwipeEnd;
        }
        catch (Exception)
        {
            Debug.Log("Swipe Destroy Error");
            throw;
        }
        
    }
    private void SwipeStart(InputAction.CallbackContext context)
    {
        m_startPosition = m_primaryPosition.ReadValue<Vector2>();
        m_startSwipeTime = Time.time;
    }

    private void SwipeEnd(InputAction.CallbackContext context) 
    {
        var dir = m_primaryPosition.ReadValue<Vector2>() - m_startPosition;
        if (Time.time - m_startSwipeTime > m_swipeDelay) return;
        if (dir.y > 0)
            m_upAction?.Invoke();
        else
            m_downAction?.Invoke();
    }
    #endregion

    #region SongList Load
    private List<SongData> 
    private void LoadSongList()
    {
        // 로컬 저장 로직
        var path = Application.persistentDataPath;
        if (!Directory.Exists(path))
        {
            Debug.LogWarning($"폴더가 없습니다. {path}");
            throw new FileNotFoundException();
        }

        var jsonFiles = Directory.GetFiles(path, "*.json");

        if (jsonFiles.Length == 0)
        {
            Debug.LogWarning($"폴더 안에 JSON 파일이 없습니다: {path}");
            throw new FileNotFoundException();
        }

        foreach (var filePath in jsonFiles)
        {
            try
            {
                string json = File.ReadAllText(filePath);
                SongData song = JsonUtility.FromJson<SongData>(json);
                if (song != null)
                    songList.Add(song);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to load {filePath}: {e.Message}");
            }
        }
    }
    #endregion
}
