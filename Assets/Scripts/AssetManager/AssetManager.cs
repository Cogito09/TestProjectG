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

    public IEnumerator GetImage(string path,Action<ImageData> OnFinish)
    {
        var creationTime = File.GetCreationTime(path);
        var name = Path.GetFileName(path);
        var texture = new Texture2D(2, 2);
        bool isImageLoaded = false;
        ReadImageAsync(path, bytes =>
        {
            //texture.LoadRawTextureData(bytes);
            texture.LoadImage(bytes);
            isImageLoaded = true;
        });

        yield return new WaitUntil(() => isImageLoaded);
        ImageData image = new ImageData();
        image.CreationTime = creationTime;
        image.Name = name;
        image.Sprite = Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            Vector2.zero);
        
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

    public string[] GetAllImagesFilesPaths()
    {
        return Directory.GetFiles(ImagesFullPath);
    }
}