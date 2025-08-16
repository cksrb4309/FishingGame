using UnityEngine;

public class ChunkManager
{
    public static int[] GetChunkCoord(Vector3 worldPos)
    {
        int[] chunkCoord = new int[2];

        chunkCoord[0] = Mathf.FloorToInt(worldPos.x / 10);
        chunkCoord[1] = Mathf.FloorToInt(worldPos.z / 10);

        return chunkCoord;
    }
}
