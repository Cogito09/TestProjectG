using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class Images
{
    public Dictionary<string, ImageData> ImagesDictionary = new Dictionary<string, ImageData>();
    
    public IEnumerator RefreshAllImages(Action onFinish,bool TryRemoveNonExisting = false)
    {
        var allFiles = ProgramManager.AssetManager.GetAllImagesFilesPaths();
        if (TryRemoveNonExisting)
        {
            yield return TryRemoveNotExisting(allFiles);
        }
        
        for (int i = 0; i < allFiles.Length; i++)
        {
            var filePath = allFiles[i];
            var name = Path.GetFileName(filePath);
            if(ImagesDictionary.ContainsKey(name))
            {
                continue;
            }

            yield return ProgramManager.Instance.StartCoroutine(ProgramManager.AssetManager.GetImage(filePath, image =>
            {
                if (image == null)
                {
                    Debug.LogError("Loaded image is null , skiping");
                    return;
                }
                
                ImagesDictionary.Add(image.Name, image);
                EventManager.OnImageAdded(image.Name);
            }));
        }
        
        onFinish?.Invoke();
    }

    private IEnumerator TryRemoveNotExisting(string[] allFiles)
    {
        var keysToBeRemoved = new List<string>();
        foreach (var kV in ImagesDictionary)
        {
            var hasBeenDeleted = true;
            var storedName = kV.Key;
            for (int i = 0; i < allFiles.Length; i++)
            {
                var filePath = allFiles[i];
                var name = Path.GetFileName(filePath);
                if (storedName == name)
                {
                    hasBeenDeleted = false;
                    break;
                }
            }

            if (hasBeenDeleted == false)
            {
                continue;
            }
            
            keysToBeRemoved.Add(storedName);
            yield return null;
        }

        for (int i = 0; i < keysToBeRemoved.Count; i++)
        {
            ImagesDictionary.Remove(keysToBeRemoved[i]);
            EventManager.OnImageRemoved?.Invoke(keysToBeRemoved[i]);
        }
    }
}