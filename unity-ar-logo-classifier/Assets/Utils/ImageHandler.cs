
using UnityEngine;

public static class ImageHandler
{
    public static byte[] GetTextureBytes(Texture2D tex, string encode)
    {
        // Encode texture into PNG
        byte[] bytes = encode switch
        {
            "jpg" => tex.EncodeToJPG(),
            "png" => tex.EncodeToPNG(),
            _ => tex.EncodeToPNG()
        };
        
        return bytes;
    }
}