using UnityEditor;
using UnityEngine;

namespace TopDownController.Editor
{
    public class PresetMenuItems : MonoBehaviour
    {
        [MenuItem("Tools/Top down controler/Essentials")]
        private static void CreateEssentials(MenuCommand menuCommand)
        {
            Spawn(menuCommand, "Assets/Prefabs/Essentials/---CAMERA---.prefab", false);
            Spawn(menuCommand, "Assets/Prefabs/Essentials/---SELECTIONS---.prefab", false);
        }
        [MenuItem("Tools/Top down controler/Spawn player")]
        private static void CreatePlayer(MenuCommand menuCommand)
        {
            Spawn(menuCommand, "Assets/Prefabs/Player.prefab");
        }
        [MenuItem("Tools/Top down controler/Spawn enemy")]
        private static void CreateEnemy(MenuCommand menuCommand)
        {
            Spawn(menuCommand, "Assets/Prefabs/Enemy.prefab");
        }
        [MenuItem("Tools/Top down controler/Spawn NPC")]
        private static void CreateNPC(MenuCommand menuCommand)
        {
            Spawn(menuCommand, "Assets/Prefabs/NPC.prefab");
        }
        [MenuItem("Tools/Top down controler/With ragdoll/Spawn player")]
        private static void CreateRagdollPlayer(MenuCommand menuCommand)
        {
            Spawn(menuCommand, "Assets/Prefabs/PlayerRagdoll.prefab");
        }
        [MenuItem("Tools/Top down controler/With ragdoll/Spawn enemy")]
        private static void CreateRagdollEnemy(MenuCommand menuCommand)
        {
            Spawn(menuCommand, "Assets/Prefabs/EnemyRagdoll.prefab");
        }
        [MenuItem("Tools/Top down controler/With ragdoll/Spawn NPC")]
        private static void CreateRagdollNPC(MenuCommand menuCommand)
        {
            Spawn(menuCommand, "Assets/Prefabs/NPCRagdoll.prefab");
        }
        private static void Spawn(
            MenuCommand menuCommand, string prefabPath, bool onSelectedTransform=true
        )
        {
            GameObject go =
                        AssetDatabase.LoadAssetAtPath(prefabPath, typeof(GameObject))
                        as GameObject;
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            
            if (onSelectedTransform)
                PrefabUtility.InstantiatePrefab(go, Selection.activeTransform);
            else
                PrefabUtility.InstantiatePrefab(go);
    
            // Register the creation in the undo system
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }
    }
}