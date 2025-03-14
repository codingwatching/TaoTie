﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace TaoTie
{
    public class I18NManager : IManager
    {
        public event Action OnLanguageChangeEvt;

        public static I18NManager Instance;
        //语言类型枚举

        public LangType CurLangType { get; private set; }
        private Dictionary<string, string> i18nTextKeyDic;
        private bool addFonts = false;
        #region override

        public void Init()
        {
            Instance = this;
            I18NBridge.Instance.GetValueByKey = I18NGetText;
            var lang = CacheManager.Instance.GetInt(CacheKeys.CurLangType, -1);
            if (lang < 0)
            {
                this.CurLangType = Application.systemLanguage == SystemLanguage.Chinese
                    ? LangType.Chinese
                    : LangType.English;
            }
            else
            {
                this.CurLangType = (LangType)lang;
            }
            this.i18nTextKeyDic = new Dictionary<string, string>();
            InitAsync().Coroutine();
            //AddSystemFonts();
        }

        private async ETTask InitAsync()
        {
            var res = await ConfigManager.Instance.LoadOneConfig<I18NConfigCategory>(this.CurLangType.ToString());
            for (int i = 0; i <res.GetAllList().Count; i++)
            {
                var item = res.GetAllList()[i];
                this.i18nTextKeyDic.Add(item.Key, item.Value);
            }
        }

        public void Destroy()
        {
            OnLanguageChangeEvt = null;
            Instance = null;
            this.i18nTextKeyDic.Clear();
            this.i18nTextKeyDic = null;
        }

        #endregion

        /// <summary>
        /// 取不到返回key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string I18NGetText(string key)
        {
            if (!this.i18nTextKeyDic.TryGetValue(key, out var result))
            {
                Log.Error("多语言key未添加！ " + key);
                result = key;
                return result;
            }

            return result;
        }

        /// <summary>
        /// 根据key取多语言取不到返回key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public string I18NGetParamText(string key, params object[] paras)
        {
            if (!this.i18nTextKeyDic.TryGetValue(key, out var value))
            {
                Log.Error("多语言key未添加！ " + key);
                return key;
            }

            if (paras != null)
                return string.Format(value, paras);
            else
                return value;
        }

        /// <summary>
        /// 取不到返回key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool I18NTryGetText(string key, out string result)
        {
            if (!this.i18nTextKeyDic.TryGetValue(key, out result))
            {
                Log.Info("多语言key未添加！ " + key);
                result = key;
                return false;
            }

            return true;
        }

        /// <summary>
        /// 切换语言,外部接口
        /// </summary>
        /// <param name="langType"></param>
        public async ETTask SwitchLanguage(int langType)
        {
            //修改当前语言
            CacheManager.Instance.SetInt(CacheKeys.CurLangType, langType);
            this.CurLangType = (LangType)langType;
            var res = await ConfigManager.Instance.LoadOneConfig<I18NConfigCategory>(this.CurLangType.ToString());
            this.i18nTextKeyDic.Clear();
            for (int i = 0; i <res.GetAllList().Count; i++)
            {
                var item = res.GetAllList()[i];
                this.i18nTextKeyDic.Add(item.Key, item.Value);
            }

            I18NBridge.Instance.OnLanguageChangeEvt?.Invoke();
            OnLanguageChangeEvt?.Invoke();
        }

        public void RegisterI18NEntity(II18N entity)
        {
            OnLanguageChangeEvt += entity.OnLanguageChange;
        }

        public void RemoveI18NEntity(II18N entity)
        {
            OnLanguageChangeEvt -= entity.OnLanguageChange;
        }
        
                
        #region 添加系统字体
#if !UNITY_WEBGL
        /// <summary>
        /// 需要就添加
        /// </summary>
        public void AddSystemFonts()
        {
             if(addFonts) return;
             addFonts = true;
#if UNITY_EDITOR||UNITY_STANDALONE_WIN
            string[] fonts = new[] { "msyhl" };//微软雅黑细体
#elif UNITY_ANDROID
            string[] fonts = new[] {
                "notosanscjksc-regular",
                "notosanscjk-regular",
            };
#elif UNITY_IOS
            string[] fonts = new[] {
                "pingfang" // 注意内存占用70m+
            };
#else
            string[] fonts = Array.Empty<string>();
#endif
            TextMeshFontAssetManager.Instance.AddWithOSFont(fonts);
        }
#else
        public void AddSystemFonts(){}
#endif
        public void RemoveSystemFonts()
        {
            if(!addFonts) return;
            addFonts = false;
#if UNITY_EDITOR||UNITY_STANDALONE_WIN
            string[] fonts = new[] { "msyhl" };//微软雅黑细体
#elif UNITY_ANDROID
            string[] fonts = new[] {
                "notosanscjksc-regular",
                "notosanscjk-regular",
            };
#elif UNITY_IOS
            string[] fonts = new[] {
                "pingfang"// 注意内存占用70m+
            };
#else
            string[] fonts = Array.Empty<string>();
#endif
            TextMeshFontAssetManager.Instance.RemoveWithOSFont(fonts);
        }
        #endregion
    }
}