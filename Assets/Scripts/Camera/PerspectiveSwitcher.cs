using Assets.Scripts;
using UnityEngine;

public class PerspectiveSwitcher : MonoBehaviour
{
    public bool orthoOn;

    void Start()
    {
        _cam = GetComponent<Camera>();

        aspect = (float)Screen.width / (float)Screen.height;
        perspective = Matrix4x4.Perspective(fov, aspect, near, far);
        blender = _cam.GetComponent<MatrixBlender>();
    }

    public void Switch()
    {
        orthoOn = !orthoOn;
        if (orthoOn)
        {
            ortho = CalcNewOrtho();
            blender.BlendToMatrix(ortho, 1f);
        }
        else
        {
            blender.BlendToMatrix(perspective, 1f);
        }
    }

    private Matrix4x4 CalcNewOrtho()
    {
        float size = GetNewOrthoSize();

        return Matrix4x4.Ortho(-size * aspect, size * aspect, -size, size, near, far);
    }

    public float GetNewOrthoSize()
    {
        float size = orthographicSize;
        if (GameObject.Find(Tags.Globals).GetComponent<Globals>().world != null)
        {
            float height = GameObject.Find(Tags.Globals).GetComponent<Globals>().world.Height + 2f;
            size = height - ((height/60) * height);
        }

        return size;
    }

    private Camera _cam;
    private Matrix4x4 ortho, perspective;

    private float fov = 60f;
    private float near = .3f;
    private float far = 1000f;
    private float orthographicSize = 10f;

    private float aspect;
    private MatrixBlender blender;
}