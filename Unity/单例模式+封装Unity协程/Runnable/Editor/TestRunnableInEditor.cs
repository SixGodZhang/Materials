using gamemanager.util;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class TestRunnableInEditor : Editor
{
    [MenuItem("MyTool/TestRunnable")]
    public static void TestRunnable()
    {
        Runnable.EnableRunnableInEditor();
        Runnable.Run(DoAction());
    }

    private static IEnumerator DoAction()
    {
        Debug.Log("begin...");
        yield return null;
        Debug.Log("3s....");
        yield return null;
        Debug.Log("3s....");
    }
}
