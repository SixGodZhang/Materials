using System;
using UnityEngine;

namespace gamemanager.util
{
    /// <summary>
    /// 泛型单例类
    /// 用法:
    /// 1.继承UnityEngine.Object的类 
    ///     public static Runnable Instance { get { return Singleton<Runnable>.Instance; } }
    /// 2.继承System.Object的类 
    ///     public static AInstance Instance { get { return Singleton<AInstance>.Instance; } }
    /// </summary>
    /// <typeparam name="T">需要创建单例的类型</typeparam>
    public class Singleton<T> where T:class
    {
        private static T sm_Instance = default(T);

        /// <summary>
        /// 单例
        /// </summary>
        public static T Instance
        {
            get {
                if (sm_Instance == null)
                    CreateInstance();
                return sm_Instance;
            }
        }

        private static void CreateInstance()
        {
            if (typeof(MonoBehaviour).IsAssignableFrom(typeof(T)))
            {//if T inherit MonoBehaviour
                string singletonName = "_" + typeof(T).Name;
                GameObject singletonObject = GameObject.Find(singletonName);
                if (singletonObject == null)
                    singletonObject = new GameObject(singletonName);
                singletonObject.hideFlags = HideFlags.HideAndDontSave;
                sm_Instance = singletonObject.AddComponent(typeof(T)) as T;
            }
            else // if T inherit System.Object
                sm_Instance = Activator.CreateInstance(typeof(T)) as T;

            if (sm_Instance == null)
                throw new Exception("Failed to create instance " + typeof(T).Name);
        }
    }
}


