namespace SceneLoaderSystem
{
    [System.Serializable]
    public class LoadSceneRequest
    {
        public SceneSO scene;
        public bool loadingScreen;
        public bool requestFromDeath;

        public LoadSceneRequest(SceneSO scene, bool loadingScreen, bool requestFromDeath = false)
        {
            this.scene = scene;
            this.loadingScreen = loadingScreen;
            this.requestFromDeath = requestFromDeath;
        }
    }
}