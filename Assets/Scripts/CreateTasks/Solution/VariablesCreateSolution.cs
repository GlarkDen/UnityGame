using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VariablesCreateSolution : MonoBehaviour
{
    public GameObject taskOptionsStage;

    public GameObject createSolutionStage;

    public GameObject restartGame;

    void Start()
    {

    }

    public void BackStage()
    {
        taskOptionsStage.SetActive(true);
        createSolutionStage.SetActive(false);

        VariablesMechanic.ClearTruthTable();
        restartGame.GetComponent<StartGameMechanic>().RemoveMap();
        restartGame.GetComponent<StartGameMechanic>().RemoveBlockCount();
    }

    public void FinishCreate()
    {

    }
}
