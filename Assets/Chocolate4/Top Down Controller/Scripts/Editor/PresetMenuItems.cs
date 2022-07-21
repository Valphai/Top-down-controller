using UnityEditor;
using UnityEngine;

namespace TopDownController.Editor
{
    public class PresetMenuItems : MonoBehaviour
    {
        [MenuItem("Tools/Top down controler/Essentials")]
        private static void CreateEssentials(MenuCommand menuCommand)
        {
            Spawn(menuCommand, "Essentials/---CAMERA---.prefab", false);
            Spawn(menuCommand, "Essentials/---SELECTIONS---.prefab", false);
        }
        [MenuItem("Tools/Top down controler/Spawn player")]
        private static void CreatePlayer(MenuCommand menuCommand)
        {
            Spawn(menuCommand, "Entities/Player.prefab");
        }
        [MenuItem("Tools/Top down controler/Spawn enemy")]
        private static void CreateEnemy(MenuCommand menuCommand)
        {
            Spawn(menuCommand, "Entities/Enemy.prefab");
        }
        [MenuItem("Tools/Top down controler/Spawn chest")]
        private static void CreateChest(MenuCommand menuCommand)
        {
            Spawn(menuCommand, "Entities/Chest.prefab");
        }
        [MenuItem("Tools/Top down controler/Spawn NPC")]
        private static void CreateNPC(MenuCommand menuCommand)
        {
            Spawn(menuCommand, "Entities/NPC.prefab");
        }
        [MenuItem("Tools/Top down controler/With ragdoll/Spawn player")]
        private static void CreateRagdollPlayer(MenuCommand menuCommand)
        {
            Spawn(menuCommand, "Entities/PlayerRagdoll.prefab");
        }
        [MenuItem("Tools/Top down controler/With ragdoll/Spawn enemy")]
        private static void CreateRagdollEnemy(MenuCommand menuCommand)
        {
            Spawn(menuCommand, "Entities/EnemyRagdoll.prefab");
        }
        [MenuItem("Tools/Top down controler/With ragdoll/Spawn NPC")]
        private static void CreateRagdollNPC(MenuCommand menuCommand)
        {
            Spawn(menuCommand, "Entities/NPCRagdoll.prefab");
        }
        private static void Spawn(
            MenuCommand menuCommand, string prefabPath, bool onSelectedTransform=true
        )
        {
            GameObject go =
                        AssetDatabase.LoadAssetAtPath(
                            "Assets/Chocolate4/Top Down Controller/Prefabs/" + prefabPath, 
                            typeof(GameObject)
                        ) as GameObject;
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