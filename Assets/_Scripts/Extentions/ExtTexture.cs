using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class ExtTexture
{
    /// <summary>
    /// Saves a texture to PNG.
    /// </summary>
    /// <param name="path">The path.</param>
    public static void SaveToPNG(this Texture2D texture, string path)
    {
        byte[] bytes = texture.EncodeToPNG();
        File.WriteAllBytes(path, bytes);
    }

    /// <summary>
    /// Converts a RenderTexture to a Texture2D.
    /// </summary>
    /// <returns>The Texture2D.</returns>
    public static Texture2D ToTexture2D(this RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGB24, false);
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }

    #region EncodeToTGA
    /// <summary>
    /// 
    /// </summary>
    /// <param name="_texture2D"></param>
    /// <returns></returns>
    public static byte[] EncodeToTGA(this Texture2D _texture2D)
    {
        const int iTgaHeaderSize = 18;
        const int iBytesPerPixelRGB24 = 3; // 1 byte per channel (rgb)
        const int iBytesPerPixelARGB32 = 4; // ~ (rgba)

        var useAlpha = SupportsAlpha(_texture2D.format);
        int iBytesPerPixel = useAlpha ? iBytesPerPixelARGB32 : iBytesPerPixelRGB24;

        //

        using (MemoryStream memoryStream = new MemoryStream(iTgaHeaderSize + _texture2D.width * _texture2D.height * iBytesPerPixel))
        {
            using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
            {
                // Write TGA Header

                binaryWriter.Write((byte)0);                    // IDLength (not in use)
                binaryWriter.Write((byte)0);                    // ColorMapType (not in use)
                binaryWriter.Write((byte)10);                   // DataTypeCode == 10 (Runlength encoded RGB images)
                binaryWriter.Write((short)0);                   // ColorMapOrigin (not in use)
                binaryWriter.Write((short)0);                   // ColorMapLength (not in use)
                binaryWriter.Write((byte)0);                    // ColorMapDepth (not in use)
                binaryWriter.Write((short)0);                   // Origin X
                binaryWriter.Write((short)0);                   // Origin Y
                binaryWriter.Write((short)_texture2D.width);    // Width
                binaryWriter.Write((short)_texture2D.height);   // Height
                binaryWriter.Write((byte)(iBytesPerPixel * 8)); // Bits Per Pixel
                binaryWriter.Write((byte)0);                    // ImageDescriptor (not in use)

                // Write RLE Encoded Pixels

                Color32[] arPixels = _texture2D.GetPixels32();

                const int iMaxPacketLength = 128;
                int iPacketStart = 0;
                int iPacketEnd = 0;

                while (iPacketStart < arPixels.Length)
                {
                    Color32 c32PreviousPixel = arPixels[iPacketStart];

                    // Get current Packet Type
                    RLEPacketType packetType = ExtTexture.PacketType(arPixels, iPacketStart);

                    // Find Packet End
                    int iReadEnd = Mathf.Min(iPacketStart + iMaxPacketLength, arPixels.Length);
                    for (iPacketEnd = iPacketStart + 1; iPacketEnd < iReadEnd; ++iPacketEnd)
                    {
                        bool bPreviousEqualsCurrent = ExtTexture.Equals(arPixels[iPacketEnd - 1], arPixels[iPacketEnd]);

                        // Packet End if change in Packet Type or if max Packet-Size reached
                        if (packetType == RLEPacketType.RAW && bPreviousEqualsCurrent ||
                            packetType == RLEPacketType.RLE && !bPreviousEqualsCurrent)
                        {
                            break;
                        }
                    }

                    // Write Packet

                    int iPacketLength = iPacketEnd - iPacketStart;

                    switch (packetType)
                    {
                        case RLEPacketType.RLE:

                            // Add RLE-Bit to PacketLength
                            binaryWriter.Write((byte)((iPacketLength - 1) | (1 << 7)));

                            binaryWriter.Write(c32PreviousPixel.b);
                            binaryWriter.Write(c32PreviousPixel.g);
                            binaryWriter.Write(c32PreviousPixel.r);

                            if (useAlpha)
                                binaryWriter.Write(c32PreviousPixel.a);

                            break;
                        case RLEPacketType.RAW:

                            binaryWriter.Write((byte)(iPacketLength - 1));

                            for (int iPacketPosition = iPacketStart; iPacketPosition < iPacketEnd; ++iPacketPosition)
                            {
                                binaryWriter.Write(arPixels[iPacketPosition].b);
                                binaryWriter.Write(arPixels[iPacketPosition].g);
                                binaryWriter.Write(arPixels[iPacketPosition].r);

                                if (useAlpha)
                                    binaryWriter.Write(arPixels[iPacketPosition].a);
                            }

                            break;
                    }

                    iPacketStart = iPacketEnd;
                }
            }

            return memoryStream.GetBuffer();
        }
    }

    //

    // RLE Helper

    private enum RLEPacketType { RLE, RAW }

    private static bool Equals(Color32 _first, Color32 _second)
    {
        return _first.r == _second.r && _first.g == _second.g && _first.b == _second.b && _first.a == _second.a;
    }

    private static RLEPacketType PacketType(Color32[] _arData, int _iPacketPosition)
    {
        if ((_iPacketPosition != _arData.Length - 1) && ExtTexture.Equals(_arData[_iPacketPosition], _arData[_iPacketPosition + 1]))
        {
            return RLEPacketType.RLE;
        }
        else
        {
            return RLEPacketType.RAW;
        }
    }

    private static bool SupportsAlpha(TextureFormat format)
    {
        switch (format)
        {
            case TextureFormat.RGBA32:
            case TextureFormat.ARGB32:
                return true;
            default:
                return false;
        }
    }

    #endregion
}
