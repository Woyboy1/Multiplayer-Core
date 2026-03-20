using UnityEngine;

namespace MultiplayerCore_Woyboy
{
    /// <summary>
    /// A script for NetworkPlayer.cs to reference to when loading into the world
    /// for the first time.
    /// </summary>
    public class SceneSpawnPoint : MonoBehaviour
    {
        public Transform[] spawnPoints;

        // -------------------- Core --------------------

        public Transform GetSpawn(int index)
        {
            if (spawnPoints.Length == 0)
                return transform;

            index %= spawnPoints.Length;
            return spawnPoints[index];
        }
    }
}