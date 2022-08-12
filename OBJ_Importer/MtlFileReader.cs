using System.IO;
using System;

public class MtlFileReader
{
    public static MtlBuilder.MaterialData RetreiveMaterialData(string filePath)
    {
        var lines = File.ReadLines(filePath);

        var builder = new MtlBuilder();

        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line) || line[0] == '#') continue;

            ParseLine(builder, line.AsSpan());
        }

        return builder.Build();
    }

    private static void ParseLine(MtlBuilder builder, ReadOnlySpan<char> line)
    {
        var span = line.Trim();

        var symbol = span.SliceUpTo(' ').ToString();

        UnityEngine.Debug.Log(symbol);

        switch (symbol)
        {
            case "newmtl":
                builder.AddNewMaterial(span.SliceUpTo(' ').ToString());
                break;
            case "Ka":
                builder.AddAmbientRGB(FileReaderUtility.ParseVertex(span));
                break;
            case "Kd":
                builder.AddDiffuseRGB(FileReaderUtility.ParseVertex(span));
                break;
            case "Ks":
                builder.AddSpecularRGB(FileReaderUtility.ParseVertex(span));
                break;
            case "Ns":
                builder.AddSpecularFocus(float.Parse(span.SliceUpTo(' ')));
                break;
            case "Ni":
                builder.AddOpticalDensity(float.Parse(span.SliceUpTo(' ')));
                break;
            case "d":
                builder.AddDissolveFactor(float.Parse(span.SliceUpTo(' ')));
                break;
            case "map_Kd":
                builder.AddDiffuseMap(span.SliceUpTo(' ').ToString());
                break;
            default:
                UnityEngine.Debug.LogWarning($"Unhandled symbol encountered: <color=cyan>{ symbol }</color>");
                break;
        }
    }
}