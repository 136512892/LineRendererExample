using UnityEngine;
using SK.Framework;

/// <summary>
/// IO配置输出端口
/// </summary>
public class IOConfigOutput : AimableObject
{
    private LineRenderer lineRenderer;
    private HighlightOutline highlight;

    private void Start()
    {
        lineRenderer = GetComponentInChildren<LineRenderer>();
        highlight = GetComponent<HighlightOutline>();
    }

    protected override void OnEnter()
    {
        base.OnEnter();
        highlight.enabled = true;
    }

    protected override void OnExit()
    {
        base.OnExit();
        highlight.enabled = false;
    }

    protected override void OnStay()
    {
        base.OnStay();
        if (Input.GetMouseButtonDown(0))
        {
            lineRenderer.SetPosition(0, transform.position);
            if (!lineRenderer.enabled)
            {
                lineRenderer.enabled = true;
            }
            else
            {
                lineRenderer.SetPosition(1, transform.position);
                lineRenderer.SetPosition(2, transform.position);
                lineRenderer.SetPosition(3, transform.position);
            }
            Timer.NextFrame(() => MonoSingleton<IOConfig>.Instance.LineRenderer = lineRenderer).Launch();
        }
    }
}