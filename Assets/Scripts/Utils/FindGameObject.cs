using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class FindGameObject
    {
        public static GameObject WithCaseInsensitiveTag(string tag)
        {
            GameObject[] allGameObjects = GameObject.FindObjectsOfType<GameObject>();
            foreach (GameObject gameObject in allGameObjects)
            {
                if (gameObject.CompareTag(tag) || (gameObject.tag != null && gameObject.tag.Equals(tag, StringComparison.OrdinalIgnoreCase)))
                {
                    return gameObject;
                }
            }
            return null;
        }

        public static List<GameObject> AllWithCaseInsensitiveTag(string tag)
        {
            GameObject[] allGameObjects = GameObject.FindObjectsOfType<GameObject>();
            List<GameObject> matchingGameObjects = new List<GameObject>();

            foreach (GameObject gameObject in allGameObjects)
            {
                if (gameObject.CompareTag(tag) || (gameObject.tag != null && gameObject.tag.Equals(tag, StringComparison.OrdinalIgnoreCase)))
                {
                    matchingGameObjects.Add(gameObject);
                }
            }

            return matchingGameObjects;
        }
    }
}
