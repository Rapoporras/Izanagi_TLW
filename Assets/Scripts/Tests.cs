using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tests : MonoBehaviour
{
    public Transform objectTest;

    [ContextMenu("Test")]
    private void ObjectTest()
    {
        Debug.Log($"Root: {objectTest.root.name}, Child: {objectTest.name}");
    }
}
