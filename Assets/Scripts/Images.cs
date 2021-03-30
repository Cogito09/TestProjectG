using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Images
{
    private List<ImageData> AllImages = new List<ImageData>();

    public List<ImageData> GetAllImages()
    {
        return AllImages;
    }
    
    public IEnumerator RefreshImages(Action onFinish)
    {
        yield return ProgramManager.AssetManager.SyncImages(images =>
        {
            AllImages = images;
            onFinish?.Invoke();
        });
    }
}