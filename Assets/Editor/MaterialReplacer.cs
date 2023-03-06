using UnityEngine;
using UnityEditor;

public class MaterialReplacer : EditorWindow
{
    // 置換前と置換後のマテリアルを保存する変数
    private Material originalMaterial;
    private Material replacementMaterial;

    // エディタのメニューからウィンドウを開くためのメソッド
    [MenuItem("Tools/Material Replacer")]
    public static void ShowWindow()
    {
        GetWindow<MaterialReplacer>("Material Replacer");
    }

    // ウィンドウの表示内容を設定するメソッド
    private void OnGUI()
    {
        GUILayout.Label("Material Replacer", EditorStyles.boldLabel);

        // オリジナルと置換用のマテリアルを選択できるようにする
        originalMaterial = EditorGUILayout.ObjectField("Original Material", originalMaterial, typeof(Material), true) as Material;
        replacementMaterial = EditorGUILayout.ObjectField("Replacement Material", replacementMaterial, typeof(Material), true) as Material;

        // "Replace Materials"ボタンを追加し、マテリアルを置換する
        if (GUILayout.Button("Replace Materials"))
        {
            if (originalMaterial == null || replacementMaterial == null)
            {
                // エラーを表示するダイアログを表示する
                EditorUtility.DisplayDialog("Error", "Please select both the original and replacement materials", "Ok");
                return;
            }

            ReplaceMaterialsInScene(originalMaterial, replacementMaterial);
        }
    }

    // シーン内のRendererで使用されているオリジナルマテリアルを、置換用のマテリアルに置き換える
    private void ReplaceMaterialsInScene(Material originalMaterial, Material replacementMaterial)
    {
        // 変更を元に戻すために、Undo.RecordObjectsを使用する
        Undo.RecordObjects(FindObjectsOfType<Renderer>(), "Replace Materials");

        foreach (Renderer renderer in FindObjectsOfType<Renderer>())
        {
            if (renderer.sharedMaterial == originalMaterial)
            {
                renderer.sharedMaterial = replacementMaterial;
            }
        }
    }
}