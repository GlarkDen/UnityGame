using UnityEngine;

public class ProjectPath : MonoBehaviour
{
    public static string Root = Application.streamingAssetsPath;

    public static string Blocks = Root + "/Game/blocks";
    public static string Schemes = Root + "/Game/schemes";
    public static string Tasks = Root + "/Game/tasks";
}
