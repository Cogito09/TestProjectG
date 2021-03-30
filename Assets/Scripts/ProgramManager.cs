using UnityEngine;

public class ProgramManager : MonoBehaviour
{
    public static ProgramManager Instance;

    private AssetManager _assetManager;
    private Images _images = new Images();
    
    public static AssetManager AssetManager => Instance._assetManager ??= Instance._assetManager = new AssetManager();
    public static Images Images => Instance._images;

    private void Awake()
    {
        Instance = this;
    }
}