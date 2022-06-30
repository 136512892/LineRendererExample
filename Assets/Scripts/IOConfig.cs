using UnityEngine;
using SK.Framework;

/// <summary>
/// IO配置
/// </summary>
public class IOConfig : MonoBehaviour, ISingleton
{
    public void OnInit() { }

    /// <summary>
    /// 当前正在连线的LineRenderer
    /// </summary>
    public LineRenderer LineRenderer { get; set; }

    private void Update()
    {
        if (LineRenderer != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out RaycastHit hit);
            Vector3 startPos = LineRenderer.gameObject.transform.position;
            Vector3 half = (LineRenderer.transform.position + hit.point) * .5f;
            LineRenderer.SetPosition(1, new Vector3(startPos.x, hit.point.y, half.z));
            LineRenderer.SetPosition(2, new Vector3(hit.point.x, hit.point.y, half.z));
            LineRenderer.SetPosition(3, hit.point);

            if (Input.GetMouseButtonDown(0))
            {
                LineRenderer.enabled = hit.collider.GetComponent<IOConfigInput>() != null;
                LineRenderer = null;
            }
        }
    }

    private void OnDestroy()
    {
        if (Application.isPlaying)
        {
            MonoSingleton<IOConfig>.Dispose();
        }
    }
}