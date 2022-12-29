using System;
using Tools;
using UnityEngine;

[Serializable]
public class Line
{
    [SerializeField] private LineRenderer[] _lineRenderers;
    [SerializeField] private float _duration;
    private LineData[] _linesData;

    public void SetParameters()
    {
        _linesData = new LineData[_lineRenderers.Length];
        for (int i = 0; i < _lineRenderers.Length; i++) _linesData[i] = new LineData(_lineRenderers[i]);
    }

    public void ClearLines()
    {
        foreach (var line in _linesData) line.LineRenderer.positionCount = 0;
    }

    public void DoAnimation()
    {
        Toolbox.Get<LinePainter>().Draw(_linesData, _duration);
    }
}

[Serializable]
public class LineData
{
    public Vector3[] Points { get; }
    public Transform Transform => LineRenderer.transform;
    public LineRenderer LineRenderer { get; private set; }

    public LineData(LineRenderer lineRenderer)
    {
        LineRenderer = lineRenderer;
        Points = new Vector3[LineRenderer.positionCount];
        LineRenderer.GetPositions(Points);
    }
}