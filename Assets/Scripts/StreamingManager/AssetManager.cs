using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AssetManager 
{
    public string ImagesDirectoryName = "Images";
    public string ImagesFullPath => Path.GetFullPath(Path.Combine(Application.persistentDataPath, ImagesDirectoryName));
    
    public AssetManager()
    {
        Initialize();
    }

    private void Initialize()
    {
        CreateImagesDirectory();
    }

    public void CreateImagesDirectory()
    {
        var path = Application.persistentDataPath;
        if (Directory.Exists(ImagesFullPath) == false)
        {
            Debug.Log("Images directory does not exist. Creating");
            Directory.CreateDirectory(ImagesFullPath);
        }
        
        Debug.Log("Images directory exist.");
    }

    public IEnumerator SyncImages(Action<List<ImageData>> OnFinish)
    {
        var allImages = new List<ImageData>();
        var allFiles = Directory.GetFiles(ImagesFullPath);
        Debug.Log($"Images full path : {ImagesFullPath}");
        
        for (int i = 0; i < allFiles.Length; i++)
        {
            var file = allFiles[i];
            Debug.Log($"File {i+1}");
            Debug.Log($"Existing file path: {file}");

            var creationTime = File.GetCreationTime(file);
            Debug.Log($"Creation Time: {creationTime}");


            var name = Path.GetFileName(file);
            Debug.Log($"Name: {name}");

            var texture = new Texture2D(2, 2);
            yield return null;
            bool isImageLoaded = false;
            ReadImageAsync(file, bytes =>
            {
                //texture.LoadRawTextureData(bytes);
                texture.LoadImage(bytes);
                isImageLoaded = true;
            });

            yield return new WaitUntil(() => isImageLoaded);
            
            ImageData image = new ImageData();
            image.CreationTime = creationTime;
            image.Name = name;
                
            yield return null;
            image.Sprite = Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.height),
                Vector2.zero);
            
            allImages.Add(image);
            yield return null;
        }
        
        OnFinish?.Invoke(allImages);
    }
    
    public IEnumerator GetImage(string path,Action<ImageData> OnFinish)
    {
        var file = path;
        Debug.Log($"Existing file path: {file}");

        var creationTime = File.GetCreationTime(file);
        Debug.Log($"Creation Time: {creationTime}");


        var name = Path.GetFileName(file);
        Debug.Log($"Name: {name}");

        var texture = new Texture2D(2, 2);
        yield return null;
        bool isImageLoaded = false;
        ReadImageAsync(file, bytes =>
        {
            //texture.LoadRawTextureData(bytes);
            texture.LoadImage(bytes);
            isImageLoaded = true;
        });

        yield return new WaitUntil(() => isImageLoaded);
            
        ImageData image = new ImageData();
        image.CreationTime = creationTime;
        image.Name = name;
                
        yield return null;
        image.Sprite = Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            Vector2.zero);
            

        yield return null;
        OnFinish?.Invoke(image);
    }

    private async void LoadImageAsync(FileStream fileStream,Action<byte[]> result)
    {
       var bytes = await FileAsync.ReadAllBytes(fileStream);
       DispathToMainThread(() => result?.Invoke(bytes));
    }

    private void ReadImageAsync(string filePath,Action<byte[]> onFinish)
    {
        //Use ThreadPool to avoid freezing
        ThreadPool.QueueUserWorkItem(delegate
        {
            bool success = false;

            byte[] imageBytes;
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);

            try
            {
                int length = (int)fileStream.Length;
                imageBytes = new byte[length];
                int count;
                int sum = 0;

                // read until Read method returns 0
                while ((count = fileStream.Read(imageBytes, sum, length - sum)) > 0)
                    sum += count;

                success = true;
            }
            finally
            {
                fileStream.Close();
            }

            //Create Texture2D from the imageBytes in the main Thread if file was read successfully
            if (success)
            {
                DispathToMainThread(() =>
                {
                    onFinish?.Invoke(imageBytes);
                });
            }
        });
    }

    private void DispathToMainThread(Action action)
    {
        UnityMainThreadDispatcher.Instance().Enqueue(action);
    }
}