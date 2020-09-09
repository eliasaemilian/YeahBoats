using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Mathfs 
{
    public static float TAU = 6.28318530718f;

    public static float Remap(float value, float start, float end, float mapToStart, float mapToEnd)
    {
        // Remaps a value on range to the according value on a different range
        return mapToStart + (value - start) * (mapToEnd - mapToStart) / (end - start);
    }
    public static Vector2 GetUnitVectorByAngle(float angRad)
    {
        return new Vector2(
                Mathf.Cos(angRad),
                Mathf.Sin(angRad)
                );
    }

    public static Vector3 LerpLinear(Vector3 start, Vector3 end, float t)
    {
        float x = ((end.x - start.x) * t) + start.x;
        float y = ((end.y - start.y) * t) + start.y;
        float z = ((end.z - start.z) * t) + start.z;


        return new Vector3(x,y,z);
    }
}
