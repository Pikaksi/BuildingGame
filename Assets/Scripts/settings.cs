using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class settings : MonoBehaviour
{
    private void Awake()
    {
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 60;
        StructureData.GenerateTreeStructure();
    }
}
