using System.Collections.Generic;
using UnityEngine;
using ViewModel;

public static class AWSWebFormBuilder
{
    public static WWWForm BuildUploadRequest(Fields fields, Texture2D textureScreenshot, string fileName)
    {
        WWWForm form = new WWWForm();

        // Assuming fields["policy"] is already a base64-encoded string, so we directly use its value
        string policy = fields.policy;

        string encode = "png";
        byte[] image = ImageHandler.GetTextureBytes(textureScreenshot, encode); // Ensure this method returns the correct byte array for the image
        string date = fields.XAmzDate; // Use .Value to get the string without quotes
        var key = fields.key;
        var algorithm = fields.XAmzAlgorithm;
        var credentials = fields.XAmzCredential;
        var securityToken = fields.XAmzSecurityToken;
        var signature = fields.XAmzSignature;

        // Use .Value for all string fields to ensure you don't include additional quotation marks
        form.AddField("key", key);
        form.AddField("x-amz-algorithm", algorithm);
        form.AddField("x-amz-credential", credentials);
        form.AddField("x-amz-date", date);
        form.AddField("x-amz-security-token", securityToken);
        form.AddField("policy", policy); // No need to re-encode to base64
        form.AddField("x-amz-signature", signature);
        form.AddBinaryData("file", image, fileName, $"image/{encode}");

        return form;
    }
}