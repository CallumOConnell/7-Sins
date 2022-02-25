using UnityEngine;

namespace Sins.Utils
{
    public class FindMissingScripts : MonoBehaviour
    {
        public void FindMissingComponents()
        {
            var gameObjects = FindObjectsOfType<GameObject>();

            foreach (var go in gameObjects)
            {
                var components = go.GetComponents<Component>();

                foreach (var component in components)
                {
                    if (component == null)
                    {
                        Debug.Log($"GameObject: {go.name}");
                        Debug.Log($"Parent: {go.transform.root.gameObject.name}");
                        //Debug.Log($"Component: {component.name}");

                        DestroyImmediate(component);
                    }
                }
            }
        }
    }
}