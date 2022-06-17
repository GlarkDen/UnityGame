using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VariablesCreateSolution : MonoBehaviour
{
    public GameObject taskOptionsStage;

    public GameObject createSolutionStage;

    public GameObject restartGame;

    public Button finishCreate;

    private TruthTable truthTable;
    private int currentSensorCount = 1;

    void Start()
    {
        VariablesMechanic.UpdateSensorCount += new VariablesMechanic.SensorCountHandler(ChangeSensorCount);
        VariablesMechanic.TruthTableUpdate += new VariablesMechanic.TruthTableHandler(TruthTableUpdated);
    }

    public void BackStage()
    {
        finishCreate.interactable = false;
        taskOptionsStage.SetActive(true);
        createSolutionStage.SetActive(false);

        VariablesMechanic.ClearTruthTable();
        restartGame.GetComponent<StartGameMechanic>().RemoveMap();
        restartGame.GetComponent<StartGameMechanic>().RemoveBlockCount();
    }

    public void FinishCreate()
    {
        Task createTask = new Task();

        createTask.mehanicBlock = StartGameMechanic.MehanicBlock;
    }

    public void TruthTableUpdated(TruthTable truthTable)
    {
        this.truthTable = truthTable;
        FinishReady();
    }

    public void ChangeSensorCount(int count)
    {
        currentSensorCount = count;
        FinishReady();
    }

    public void FinishReady()
    {
        if (currentSensorCount == 0)
        {
            if (!finishCreate.interactable)
                finishCreate.interactable = true;
        }
        else if (finishCreate.interactable)
            finishCreate.interactable = false;
    }
}
