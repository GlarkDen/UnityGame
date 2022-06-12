using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCreateSolution : MonoBehaviour
{
    public GameObject StartObject;

    void Start()
    {
        StartObject.GetComponent<StartGameMechanic>().GenerateMap(VariablesCreateTasks.sizeMap, VariablesCreateTasks.chooseMehanicBlock);
        StartObject.GetComponent<StartGameMechanic>().SetBlockCount(VariablesCreateTasks.sensorBlocks, VariablesCreateTasks.setBlocksCount);
        StartObject.GetComponent<StartGameMechanic>().SetTaskText(VariablesCreateTasks.taskText);
    }
}
