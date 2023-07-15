using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Utils
{
    public static class ExtendedMethods
    {
        #region Transform

        /// <summary>
        /// Duplica la escala
        /// </summary>
        /// <param name="t"></param>
        public static void DuplicateScale(this Transform t)
        {
            t.localScale *= 2;
        }

        /// <summary>
        /// Cambia la posición en la coordenada X
        /// </summary>
        /// <param name="t"></param>
        /// <param name="newX"></param>
        public static void SetX(this Transform t, float newX)
        {
            t.position = new Vector3(newX, t.position.y, t.position.z);
        }

        /// <summary>
        /// Cambia la posición en la coordenada Y
        /// </summary>
        /// <param name="t"></param>
        /// <param name="newY"></param>
        public static void SetY(this Transform t, float newY)
        {
            t.position = new Vector3(t.position.x, newY, t.position.z);
        }

        /// <summary>
        /// Cambia la posición en la coordenada Z
        /// </summary>
        /// <param name="t"></param>
        /// <param name="newZ"></param>
        public static void SetZ(this Transform t, float newZ)
        {
            t.position = new Vector3(t.position.x, t.position.y, newZ);
        }

        #endregion

        #region Image

        /// <summary>
        /// Cambia el alfa de una imagen
        /// </summary>
        /// <param name="img"></param>
        /// <param name="alpha"></param>
        public static void SetImageAlpha(this Image img, float alpha)
        {
            Color c = img.color;
            c.a = alpha;
            img.color = c;
        }

        #endregion

        #region NavMeshAgent

        /// <summary>
        /// Indica si ha alcanzado su objetivo
        /// </summary>
        /// <param name="navMesh"></param>
        /// <returns></returns>
        public static bool HasReachedDestination(this NavMeshAgent navMesh)
        {
            return (!navMesh.pathPending &&
            (navMesh.remainingDistance < navMesh.stoppingDistance) &&
            (!navMesh.hasPath || navMesh.velocity.sqrMagnitude == 0f));
        }

        #endregion

    }
}
