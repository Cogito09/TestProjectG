
using System.Collections;
using System.Collections.Generic;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.iOS;
using UnityEngine.Networking;

public class AdressablesManager
{
    private AddressableAssetGroup ImageGroup ;
    
    public void RefreshImages()
    {
        var streamingAssetSubFolder = Addressables.StreamingAssetsSubFolder();
        AddressableAssetGroup.
        var runtimePath =  Addressables.RuntimePath;
        Debug.Log($"runtimePath : {runtimePath}");
        Addressables.LoadContentCatalogAsync("Asset/Images");

        using (fsd =  UnityWebRequestTexture(streamingAssetSubFolder).)
        {
            
        }
        
        group.GatherAllAssets();
        
        
        

    }    
    
    private IEnumerator ReadFromStreamingAssets()
    {
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "MyFile");
        string result = "";
        if (filePath.Contains("://"))
        {
            UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(filePath);
            yield return www.SendWebRequest();
            result = www.downloadHandler.text;
        }
        else
            result = System.IO.File.ReadAllText(filePath);
    }
    
}