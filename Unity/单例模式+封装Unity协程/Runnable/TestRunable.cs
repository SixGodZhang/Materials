using UnityEngine;
using System.Collections;
using gamemanager.util;

public class TestRunable : MonoBehaviour
{
    private void Start()
    {
        //单例测试
        //Debug.LogError(AInstance.Instance.id);

        //Unity MonoBehaviour 协程测试
        //StartCoroutine(DoAction());

        //自定义封装Unity 协程测试
        //Runnable.Run(DoAction());

    }

    private IEnumerator DoAction()
    {
        Debug.Log("begin...");
        yield return new WaitForSeconds(3.0f);
        Debug.Log("3s....");
        yield return new WaitForSeconds(3.0f);
        Debug.Log("3s....");
    }
}

public class AInstance
{
    public static AInstance Instance { get { return Singleton<AInstance>.Instance; } }
    public int id = 999;
}
