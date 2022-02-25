using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Sins.Utils
{
    public class FindGameObjectsWithMissingScripts : Editor
    {
        [MenuItem("Component/Find game objects with missing scripts")]
        public static void FindPrefabsGameObjects()
        {
            var prefabPaths = AssetDatabase.GetAllAssetPaths().Where(path => path.EndsWith(".prefab", System.StringComparison.OrdinalIgnoreCase)).ToArray();

            GameObject parent = null;

            foreach (var prefabPath in prefabPaths)
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

                var components = prefab.GetComponents<Component>();

                foreach (var component in components)
                {
                    if (component == null)
                    {
                        if (parent == null)
                        {
                            parent = new GameObject("Missing Component Objects");
                        }

                        var instance = Instantiate(prefab, parent.transform);

                        break;
                    }
                }
            }
        }

        [MenuItem("Component/Find game objects with missing scripts 2")]
        public static void FindMissingGameObjectComponents()
        {
            var script = FindObjectOfType<FindMissingScripts>();

            if (script != null)
            {
                script.FindMissingComponents();
            }
        }
    }
}