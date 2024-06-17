using System.Diagnostics;
using UnityEditor;

class EditorManager
{
    [MenuItem("TRP/开发说明文档", false, 0)]
    public static void OpenDevelopCsv()
    {
        Process.Start("Assets\\TRP\\Resources\\CubeData.csv");
    }
    [MenuItem("TRP/方块数据文档", false, 1)]
    public static void OpenCubeCsv()
    {
        Process.Start("Assets\\TRP\\Resources\\CubeData.csv");
    }
    [MenuItem("TRP/更新预制数据", false, 1)]
    public static void UpdatePrefab()
    {
        //载入预制体数据
        CubeDataManager.LoadCubeData();
        //按类型生成预制体
        CubeDataManager.CreatNewPrefab();
        //更新物体缩略图
        CubeDataManager.UpdatePrefabsTexture();
    }
}
