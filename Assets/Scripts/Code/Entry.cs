using System;
using System.Threading;
using UnityEngine;
using YooAsset;

namespace TaoTie
{
    public static class Entry
    {
        public static void Start()
        {
            StartAsync().Coroutine();
        }

        private static async ETTask StartAsync()
        {
            try
            {
                ManagerProvider.RegisterManager<Messager>();
                ManagerProvider.RegisterManager<LogManager>();
                
                ManagerProvider.RegisterManager<AttributeManager>();
                
                ManagerProvider.RegisterManager<CoroutineLockManager>();
                ManagerProvider.RegisterManager<TimerManager>();
                
                ManagerProvider.RegisterManager<CacheManager>();

                ManagerProvider.RegisterManager<ConfigManager>();
                
                ManagerProvider.RegisterManager<ResourcesManager>();
                ManagerProvider.RegisterManager<GameObjectPoolManager>();
                
                ManagerProvider.RegisterManager<I18NManager>();
                ManagerProvider.RegisterManager<UIManager>();
                
                if(PackageManager.Instance.PlayMode == EPlayMode.HostPlayMode && (Define.Networked||Define.ForceUpdate))
                {
                    await ConfigManager.Instance.LoadAsync();
                    ManagerProvider.RegisterManager<ServerConfigManager>();
                    await UIManager.Instance.OpenWindow<UIUpdateView, Action>(UIUpdateView.PrefabPath,
                        UpdateOverStartGame); //下载热更资源
                }
                else
                {
                    await StartGameAsync(false);
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
        static void UpdateOverStartGame()
        {
            StartGameAsync(true).Coroutine();
        }

        static async ETTask StartGameAsync(bool configInit)
        {
            ManagerProvider.RegisterManager<ImageLoaderManager>();
            ManagerProvider.RegisterManager<MaterialManager>();
            ManagerProvider.RegisterManager<SceneManager>();
            ManagerProvider.RegisterManager<CameraManager>();
            ManagerProvider.RegisterManager<InputManager>();
            ManagerProvider.RegisterManager<SoundManager>();
            GameObjectPoolManager.GetInstance().AddPersistentPrefabPath(UIToast.PrefabPath);
            using (ListComponent<ETTask> tasks = ListComponent<ETTask>.Create())
            {
                if (!configInit)
                {
                    tasks.Add(ConfigManager.Instance.LoadAsync());
                }
                tasks.Add(SoundManager.Instance.InitAsync());
                tasks.Add(GameObjectPoolManager.GetInstance().PreLoadGameObjectAsync(UIToast.PrefabPath, 1));
                await ETTaskHelper.WaitAll(tasks);
            }
            await PackageManager.Instance.UnloadUnusedAssets(Define.DefaultName);
            SceneManager.Instance.SwitchScene<LoginScene>().Coroutine();
        }
    }
    
}