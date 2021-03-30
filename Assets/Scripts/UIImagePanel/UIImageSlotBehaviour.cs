using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Sirenix.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class UIImageSlotBehaviour : MonoBehaviour
{
    [SerializeField] private LayoutElement _layoutElement;
    [SerializeField] private Sprite _noImageSprite;
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _timeSinceCreationDate;
    
    private ImageData _imageData;
    private long? _creationTime;
    private long CreationTime => _creationTime ?? DateTimeUtils.ToEpoch(_imageData.CreationTime);
    private void Update()
    {
        if (_imageData == null)
        {
            return;
        }
        
        SetupTime();
    }

    private void OnDestroy()
    {
        _imageData = null;
    }

    public void Setup(ImageData config,RectTransform viewPortRect)
    {
        ClearCache();
        
        _imageData = config;
        if (_imageData == null)
        {
            Debug.Log("Image slot config is null");
        }

        SetupImage();
        SetupName();
        SetupTime();
        ScaleLayoutElement(viewPortRect);
    }

    private void ClearCache()
    {
        _creationTime = null;
    }

    private void ScaleLayoutElement(RectTransform viewPortRect)
    { 
        var height = _imageData?.Sprite.texture.height ?? 100;
        var width = _imageData?.Sprite.texture.width ?? 100;


        float slotHeight;
        var slotWidth = viewPortRect.rect.width;
        if (slotWidth < width)
        {
            slotHeight = height-((float)(1 - ((float)slotWidth / (float)width) ) * (float)height );
        }
        else
        {
            slotHeight = height;
        }
        
        _layoutElement.minHeight = slotHeight;
        _layoutElement.minWidth = slotWidth; 
    }

    private void SetupTime()
    {
        var now = DateTimeUtils.ToEpoch(DateTime.Now);
        var timePassed = now - CreationTime;
       
        _timeSinceCreationDate.text = $"Time since creation: {DateTimeUtils.TimeWithSymbols((int)timePassed)}";
    }

    private void SetupName()
    {
        _name.text = _imageData?.Name ?? String.Empty;
    }

    private void SetupImage()
    {
        _image.sprite = _imageData?.Sprite ?? _noImageSprite;
    }
}
