using UnityEngine;
using System.Collections;

public class MatrixBlender : MonoBehaviour
{
    void Start()
    {
        _cam = GetComponent<Camera>();
        perspectiveSwitcher = _cam.GetComponent<PerspectiveSwitcher>();
    }

    public static Matrix4x4 MatrixLerp(Matrix4x4 from, Matrix4x4 to, float time)
    {
        Matrix4x4 ret = new Matrix4x4();
        for (int i = 0; i < 16; i++)
            ret[i] = Mathf.Lerp(from[i], to[i], time);
        return ret;
    }

    private IEnumerator LerpFromTo(Matrix4x4 src, Matrix4x4 dest, float duration)
    {
        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            _cam.projectionMatrix = MatrixLerp(src, dest, (Time.time - startTime) / duration);
            yield return 1;
        }
        _cam.projectionMatrix = dest;

        // stuff after transition
        if (disableShadows) GameObject.Find(Tags.Light).GetComponent<Light>().shadowStrength = 1f;

        _cam.ResetProjectionMatrix();
        if (perspectiveSwitcher.orthoOn) _cam.orthographicSize = perspectiveSwitcher.GetNewOrthoSize();
        _cam.orthographic = perspectiveSwitcher.orthoOn;
    }

    public Coroutine BlendToMatrix(Matrix4x4 targetMatrix, float duration)
    {
        if (disableShadows) GameObject.Find(Tags.Light).GetComponent<Light>().shadowStrength = 0f;
        StopAllCoroutines();
        return StartCoroutine(LerpFromTo(_cam.projectionMatrix, targetMatrix, duration));
    }

    private Camera _cam;
    private PerspectiveSwitcher perspectiveSwitcher;

    // disables the shadows on matrix lerp, because they look weird while lerping
    public bool disableShadows = true;
}