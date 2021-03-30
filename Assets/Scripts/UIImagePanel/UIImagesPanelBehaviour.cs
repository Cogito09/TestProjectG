using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

public class UIImagesPanelBehaviour : MonoBehaviour
{
    private Images _imagesLogic;
    private Images ImagesLogic => _imagesLogic ??= ProgramManager.Images;
    
    [FormerlySerializedAs("_root")] [SerializeField] private RectTransform _viewPortRect; 
    [SerializeField] private Dictionary<string,UIImageSlotBehaviour> _slots = new Dictionary<string,UIImageSlotBehaviour>();
    [SerializeField] private UIImageSlotBehaviour _prefab;
    [SerializeField] private Transform _content;
    [SerializeField] private GameObject _refreshAnimation;
    private bool _isRefreshing;
    
    private void Awake()
    {
        SetupRefreshPanelBlocker( _isRefreshing = false);
        EventManager.OnImageAdded += OnImageAdded;
        EventManager.OnImageRemoved += OnImageRemoved;
    }

    private void Start()
    {
        Refresh();
    }
    
    private void OnDestroy()
    {
        EventManager.OnImageAdded -= OnImageAdded;
        EventManager.OnImageRemoved -= OnImageRemoved;
    }

    private void OnImageRemoved(string fileName)
    {
        _slots.TryGetValue(fileName, out var imageSlot);
        if (imageSlot == null)
        {
            Debug.Log($"Tried to remove deleted image slot but its already delated.");
            return;
        }
        
        Destroy(imageSlot.gameObject);
        _slots.Remove(fileName);
    }

    private void OnImageAdded(string fileName)
    {
        var imageData = ImagesLogic.ImagesDictionary[fileName];
        
        _slots.TryGetValue(fileName, out var imageSlot);
        if (imageSlot == null)
        {
            imageSlot = Instantiate(_prefab, _content);
            _slots.Add(fileName,imageSlot);
        }
        
        imageSlot.Setup(imageData,_viewPortRect);
    }
    
    public void OnClickRefresh()
    {
        Refresh();
    }

    public void OnClickSync()
    {
        Refresh(true);
    }
    
    private void Refresh(bool sync = false)
    {
        if (_isRefreshing)
        {
            Debug.Log("Refreshing already in progress.");
            return;
        }
        
        SetupRefreshPanelBlocker(_isRefreshing = true);
        StartCoroutine(ProgramManager.Images.RefreshAllImages(() =>
        {
            SetupRefreshPanelBlocker(_isRefreshing = false);
        },sync));
    }

    private void SetupRefreshPanelBlocker(bool state)
    {
        _refreshAnimation.SetActive(state);
    }
}
