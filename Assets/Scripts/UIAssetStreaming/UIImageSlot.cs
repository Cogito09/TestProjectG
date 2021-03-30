using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Sirenix.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class UIImageSlot : MonoBehaviour
{
    [SerializeField] private LayoutElement _layoutElement;
    [SerializeField] private Sprite _noImageSprite;
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _timeSinceCreationDate;

    public const float width = 600;
    private ImageData _config;
    private long? _creationTime;
    private long CreationTime => _creationTime ?? DateTimeUtils.ToEpoch(_config.CreationTime);
    private void Update()
    {
        SetupTime();
    }

    public void Setup(ImageData config,RectTransform viewPortRect)
    {
        ClearCache();
        
        _config = config;
        if (_config == null)
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
        var height = _config?.Sprite.texture.height ?? 100;
        var width = _config?.Sprite.texture.width ?? 100;


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
       
        _timeSinceCreationDate.text = $"Created : {DateTimeUtils.TimeWithSymbols((int)timePassed)} ago.";
    }

    private void SetupName()
    {
        _name.text = _config?.Name ?? String.Empty;
    }

    private void SetupImage()
    {
        _image.sprite = _config?.Sprite ?? _noImageSprite;
    }
}
