using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewtonInterpolate : MonoBehaviour
{
    public float[,] DiffTable { get; set; }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public float GetYByDiffTable(float x, PointsHandle.Point[] points)
    {
        float y = 0;

        for (int i = 0; i < points.Length; i++)
        {
            float term = DiffTable[0, i];
            for (int j = 0; j < i; j++)
            {
                term *= (x - points[j].XPosition);
            }
            y += term;
        }
        return y;
    }
    private float[,] GenerateDiffTable(PointsHandle.Point[] points)
    {
        DiffTable = new float[points.Length, points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            DiffTable[i, 0] = points[i].YPosition;
        }

        for (int i = 1; i < points.Length; i++)
        {
            for (int j = 0; j < points.Length - i; j++)
            {
                DiffTable[j, i] = (DiffTable[j, i - 1] - DiffTable[j + 1, i - 1]) / (points[j].XPosition - points[i + j].XPosition);
            }
        }
        return DiffTable;
    }

    public string GenerateFunctionInterpolate(PointsHandle.Point[] points)
    {
        GenerateDiffTable(points);

        string function = "P(x) = ";

        for (int i = 0; i < points.Length; i++)
        {
            if (DiffTable[0, i] != 0)
                function += $"({DiffTable[0, i]})";
            for (int j = 0; j < i; j++)
            {
                if (points[j].XPosition < 0)
                    function += $"(x + {Mathf.Abs(points[j].XPosition)})";
                else if (points[j].XPosition == 0)
                    function += "(x)";
                else
                    function += $"(x - {points[j].XPosition})";
            }
            if (i < points.Length - 1 && DiffTable[0, i] != 0)
                function += " + ";
        }
        return function;
    }
}
