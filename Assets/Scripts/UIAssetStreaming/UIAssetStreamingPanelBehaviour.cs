using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

public class UIAssetStreamingPanelBehaviour : MonoBehaviour
{
    [FormerlySerializedAs("_root")] [SerializeField] private RectTransform _viewPortRect; 
    [SerializeField] private List<UIImageSlot> _slots = new List<UIImageSlot>();
    
    [SerializeField] private UIImageSlot _prefab;
    [SerializeField] private Transform _content;
    [SerializeField] private GameObject _refreshAnimation;
    private bool _isRefreshing;
    
    private void Awake()
    {
        _isRefreshing = false;
        SetupRefreshPanelBlocker(_isRefreshing);
    }

    private void Refresh()
    {
        if (_isRefreshing)
        {
            Debug.Log("Refreshing already in progress.");
            return;
        }
        
        SetupRefreshPanelBlocker(_isRefreshing = true);
        StartCoroutine(ProgramManager.Images.RefreshImages(() => SetupImages()));
    }

    private void SetupImages()
    {
        StartCoroutine(SetupImages(() =>  SetupRefreshPanelBlocker(_isRefreshing = false)));
    }

    private IEnumerator SetupImages(Action onFinish)
    {
        List<ImageData> getAllImages = ProgramManager.Images.GetAllImages();
        
        for (var i = 0; i < _slots.Count; i++)
        {
            _slots[i].gameObject.SetActive(false);
            yield return null;
        }

        for (int i = 0; i < getAllImages.Count; i++)
        {
            yield return null;
            var imageData = getAllImages[i];
            if (_slots.Count < i)
            {
                _slots[i].Setup(imageData,_viewPortRect);
                continue;
            }

            var newSlot = Instantiate(_prefab, _content);
            newSlot.Setup(imageData,_viewPortRect);
            _slots.Add(newSlot);
        }
        
        onFinish?.Invoke();
    }

    private void SetupRefreshPanelBlocker(bool state)
    {
        _refreshAnimation.SetActive(state);
    }
    
    public void OnClickRefresh()
    {
        Refresh();
    }
}
