using System;
using System.Threading;

namespace TaoTie
{
    public static class Entry
    {
        public static void Start()
        {
            try
            {
                ManagerProvider.RegisterManager<Messager>();
                ManagerProvider.RegisterManager<AttributeManager>();
                
                ManagerProvider.RegisterManager<CoroutineLockManager>();
                ManagerProvider.RegisterManager<TimerManager>();
                
                ManagerProvider.RegisterManager<ConfigManager>();
                ManagerProvider.RegisterManager<ResourcesManager>();
                ManagerProvider.RegisterManager<GameObjectPoolManager>();
                ManagerProvider.RegisterManager<ImageLoaderManager>();
                ManagerProvider.RegisterManager<MaterialManager>();
                
                ManagerProvider.RegisterManager<I18NManager>();
                ManagerProvider.RegisterManager<UIManager>();

                ManagerProvider.RegisterManager<CameraManager>();
                ManagerProvider.RegisterManager<SceneManager>();
                
                ManagerProvider.RegisterManager<ServerConfigManager>();
                StartGameAsync().Coroutine();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
        static async ETTask StartGameAsync()
        {
            if(Define.Networked||Define.ForceUpdate)
                await UIManager.Instance.OpenWindow<UIUpdateView,Action>(UIUpdateView.PrefabPath,StartGame);//下载热更资源
            else
                StartGame();
        }

        static void StartGame()
        {
            //热更完
            SceneManager.Instance.SwitchScene<LoginScene>().Coroutine();
        }
    }
    
}