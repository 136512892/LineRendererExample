using SK.Framework;

/// <summary>
/// IO配置输入端口
/// </summary>
public class IOConfigInput : AimableObject
{
    private HighlightOutline highlight;

    private void Start()
    {
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
}