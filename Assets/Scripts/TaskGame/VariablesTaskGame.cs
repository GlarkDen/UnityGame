using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VariablesTaskGame : MonoBehaviour
{
    private TruthTable truthTable = new TruthTable();
    public Transform trueAnswer;
    public Transform falseAnswer;

    private Coroutine falseAnswerOff;

    private void Start()
    {
        VariablesMechanic.TruthTableUpdate += ChangeTruthTable;
    }

    public void ChangeTruthTable(TruthTable truthTable)
    {
        this.truthTable = truthTable;
    }

    public void CheckSolution()
    {
        bool result = truthTable.Compare(StartTaskGame.GetCurrentTask().truthTable);
        Timer.Pause();

        if (result) 
        {
            trueAnswer.gameObject.SetActive(true);
            trueAnswer.GetChild(3).GetComponent<Text>().text = "";
            trueAnswer.GetChild(3).GetComponent<Text>().text += "Ёффективность: " + StartTaskGame.GetCurrentTask().solutionCountBlocks + "/" + VariablesMechanic.SolutionCountBlocks;
            trueAnswer.GetChild(3).GetComponent<Text>().text += "\n¬рем€: " + Timer.Current.ToString();

            OnCheckedSolution(true, StartTaskGame.GetCurrentTask().solutionCountBlocks + "/" + VariablesMechanic.SolutionCountBlocks, Timer.Current.ToString());
        }
        else
        {
            falseAnswer.gameObject.SetActive(true);
            falseAnswerOff = StartCoroutine(FalseAnswer());

            OnCheckedSolution(false);
        }
    }

    public delegate void CheckSolutionHandler(bool result, string efficiency = "", string time = "");

    public event CheckSolutionHandler CheckedSolution;

    private void OnCheckedSolution(bool result, string efficiency = "", string time = "")
    {
        if (CheckedSolution != null)
            CheckedSolution(result, efficiency, time);
    }

    private IEnumerator FalseAnswer()
    {
        yield return new WaitForSeconds(2.6f);
        falseAnswer.gameObject.SetActive(false);
        Timer.Continue();
    }

    public void StopCoroutineAnswer()
    {
        StopCoroutine(falseAnswerOff);
    }
}
