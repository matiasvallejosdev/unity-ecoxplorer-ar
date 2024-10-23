
using System.IO;
using UnityEngine;

namespace Components.Utils
{
    public static class Screenshot
    {
        private static string TEMP_FILE_NAME = ".png";
        private static RenderTexture renderTexture;
        private static Texture2D screenShot;

        public static Texture2D GetScreenShot(Camera camera)
        {
            int sceenWidth = Screen.width;
            int sceenHeight = Screen.height;

            if (renderTexture == null || renderTexture.width != sceenWidth || renderTexture.height != sceenHeight)
            {
                if (renderTexture != null)
                {
                    renderTexture.Release(); // Release the previous RenderTexture if it exists
                }

                renderTexture = new RenderTexture(sceenWidth, sceenHeight, 24);
                screenShot = new Texture2D(sceenWidth, sceenHeight, TextureFormat.ARGB32, false);
            }

            camera.targetTexture = renderTexture;
            camera.Render();

            RenderTexture.active = renderTexture;
            screenShot.ReadPixels(new Rect(0, 0, sceenWidth, sceenHeight), 0, 0);
            screenShot.Apply();

            camera.targetTexture = null;
            RenderTexture.active = null;

            return screenShot;
        }

        public static void SaveScreenshot(Texture2D screenshot, string fileName = "file")
        {
            string dateTimeName = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            byte[] textureBytes = screenshot.EncodeToPNG();
            string filePath = Path.Combine(Application.persistentDataPath, $"{fileName}_{dateTimeName}{TEMP_FILE_NAME}");
            File.WriteAllBytes(filePath, textureBytes);
            Debug.Log($"Screenshot saved to: {filePath}");
        }

        public static Texture2D ResampleAndCrop(Texture2D source, int targetWidth, int targetHeight)
        {
            int sourceWidth = source.width;
            int sourceHeight = source.height;
            float sourceAspect = (float)sourceWidth / sourceHeight;
            float targetAspect = (float)targetWidth / targetHeight;

            float factor;
            int xOffset = 0, yOffset = 0;
            if (sourceAspect > targetAspect)
            {
                // Crop width
                factor = (float)sourceHeight / targetHeight;
                xOffset = (int)((sourceWidth - targetWidth * factor) * 0.5f);
            }
            else
            {
                // Crop height
                factor = (float)sourceWidth / targetWidth;
                yOffset = (int)((sourceHeight - targetHeight * factor) * 0.5f);
            }

            Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, false);
            for (int y = 0; y < targetHeight; y++)
            {
                for (int x = 0; x < targetWidth; x++)
                {
                    float xFloored = Mathf.Floor((x * factor) + xOffset);
                    float yFloored = Mathf.Floor((y * factor) + yOffset);
                    Color color = source.GetPixel((int)xFloored, (int)yFloored);
                    result.SetPixel(x, y, color);
                }
            }
            result.Apply();
            return result;
        }
    }
}
