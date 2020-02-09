using UnityEngine;

public class Screenshots : MonoBehaviour
{
    // 4k: 3840x2160
    public int resWidth = 3840;
    public int resHeight = 2160;

    void LateUpdate()
    {
        if (Input.GetKeyDown("k"))
        {
            Camera camera = this.GetComponent<Camera>();
            RenderTexture rt = new RenderTexture(1920, 1080, 24);
            camera.targetTexture = rt;
            Texture2D screenShot = new Texture2D(512, 512, TextureFormat.RGB24, false);
            camera.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(704, 284, 512, 512), 0, 0);
            camera.targetTexture = null;
            RenderTexture.active = null;
            Destroy(rt);
            byte[] bytes = screenShot.EncodeToPNG();
            string filename = string.Format("{0}/Screenshots/Vivaio {1}.png", Application.dataPath, System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
            System.IO.File.WriteAllBytes(filename, bytes);
            Debug.Log(string.Format("Took screenshot to: {0}", filename));
        }
    }
}