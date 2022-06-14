using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCreateSolution : MonoBehaviour
{
    public GameObject StartObject;

    private static GameObject restartGame;

    void Start()
    {
        restartGame = StartObject;

        StartObject.GetComponent<StartGameMechanic>().GenerateMap(VariablesCreateTasks.sizeMap, VariablesCreateTasks.chooseMehanicBlock);
        StartObject.GetComponent<StartGameMechanic>().SetBlockCount(VariablesCreateTasks.sensorBlocks, VariablesCreateTasks.setBlocksCount);
        StartObject.GetComponent<StartGameMechanic>().SetTaskText(VariablesCreateTasks.taskText);
    }

    public static void Restart()
    {
        restartGame.GetComponent<StartGameMechanic>().GenerateMap(VariablesCreateTasks.sizeMap, VariablesCreateTasks.chooseMehanicBlock);
        restartGame.GetComponent<StartGameMechanic>().SetBlockCount(VariablesCreateTasks.sensorBlocks, VariablesCreateTasks.setBlocksCount);
        VariablesMechanic.SetBlockSprites();
        restartGame.GetComponent<StartGameMechanic>().SetTaskText(VariablesCreateTasks.taskText);
    }
}
