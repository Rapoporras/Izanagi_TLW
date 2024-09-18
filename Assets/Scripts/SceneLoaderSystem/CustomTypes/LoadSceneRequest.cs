namespace SceneLoaderSystem
{
    [System.Serializable]
    public class LoadSceneRequest
    {
        public SceneSO Scene { get; private set; }
        public bool LoadingScreen { get; private set; }

        public LoadSceneRequest(SceneSO scene, bool loadingScreen)
        {
            Scene = scene;
            LoadingScreen = loadingScreen;
        }
    }
}