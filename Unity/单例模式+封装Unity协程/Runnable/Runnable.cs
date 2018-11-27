using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace gamemanager.util
{
    /// <summary>
    /// 重构协程
    /// 应用场景:用于没有继承MonoBehavior类中开启协程
    /// </summary>
    public class Runnable : MonoBehaviour
    {
        public static Runnable Instance { get { return Singleton<Runnable>.Instance; } }

        private int m_NextRountineId = 0;
        private Dictionary<int, Routine> m_Routines = new Dictionary<int, Routine>();

        /// <summary>
        /// 创建并开启一个协程
        /// </summary>
        /// <param name="routine"></param>
        /// <returns></returns>
        public static int Run(IEnumerator routine)
        {
            Routine r = new Routine(routine);
            return r.ID;
        }

        /// <summary>
        /// 停止协程
        /// </summary>
        /// <param name="ID"></param>
        public static void Stop(int ID)
        {
            Routine r = null;
            if (Instance.m_Routines.TryGetValue(ID, out r))
                r.Stop = true;
        }

        /// <summary>
        /// 判断某个协程任务是否还在执行
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool isRunning(int id)
        {
            return Instance.m_Routines.ContainsKey(id);
        }

#if UNITY_EDITOR
        private static bool sm_EditorRunnable = false;

        public static void EnableRunnableInEditor()
        {
            if (!sm_EditorRunnable)
            {
                sm_EditorRunnable = true;
                UnityEditor.EditorApplication.update += UpdateRunnable;
            }
        }

        private static void UpdateRunnable()
        {
            if (!Application.isPlaying)
            {
                Instance.UpdateRoutines();
            }
        }

        public void UpdateRoutines()
        {
            if (m_Routines.Count>0)
            {
                List<Routine> routines = new List<Routine>();
                foreach (var kp in m_Routines)
                    routines.Add(kp.Value);
                foreach (var r in routines)
                {
                    r.MoveNext();
                }
            }
        }
#endif

        /// <summary>
        /// 内部类,重构协程的主要代码
        /// </summary>
        private class Routine:IEnumerator
        {
            public int ID { get; private set; }
            public bool Stop { get; set; }

            private bool m_bMoveNext = false;
            private IEnumerator m_Enumrator = null;

            public Routine(IEnumerator a_enumrator)
            {
                m_Enumrator = a_enumrator;
                Runnable.Instance.StartCoroutine(this);
                Stop = false;
                ID = Runnable.Instance.m_NextRountineId++;
                Runnable.Instance.m_Routines[ID] = this;
#if ATDebug
                Debug.Log(string.Format("Coroutine {0} started.", ID));
#endif
            }

            public object Current { get { return m_Enumrator.Current; } }

            public bool MoveNext()
            {
                m_bMoveNext = m_Enumrator.MoveNext();
                if (m_bMoveNext && Stop)
                    m_bMoveNext = false;
                if (!m_bMoveNext)
                {
                    Runnable.Instance.m_Routines.Remove(ID);
#if ATDebug
                    Debug.Log(string.Format("Coroutine {0} Stopped.", ID));
#endif
                }

                return m_bMoveNext;
            }

            public void Reset()
            {
                m_Enumrator.Reset();
            }
        }

    }
}