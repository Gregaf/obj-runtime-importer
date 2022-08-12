using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


/// <summary>
/// Reads an .OBJ file parsing the data sequentially into a MeshData struct.
/// Also performing the same for a .MTL file associated with the .OBJ.
/// </summary>
public class ObjFileReader
{
    #region Const Fields
    private const string MaterialLibrary = "mtllib";
    private const string Vertex = "v";
    private const string Normal = "vn";
    private const string UV = "vt";
    private const string UseMaterial = "usemtl";
    private const string Face = "f";
    #endregion

    public static ObjBuilder.MeshData RetreiveMeshData(string targetPath)
    {
        var lines = File.ReadLines(targetPath);

        var builder = new ObjBuilder();

        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line) || line[0] == '#') continue;

            ParseLine(builder, line.AsSpan());
        }

        return builder.Build();
    }

    /// <summary>
    /// Retreive the OBJ data associated with the "targetPath". All data is parsed using
    /// the ReadOnlySpan<char> to reduce the number of allocations when operating. Note that
    /// the entire file is read into an enumerator and thus a high number of allocations are
    /// created.
    /// </summary>
    /// <param name="targetPath"></param>
    /// <returns>When the "targetPath" is invalid an EmptyMeshData is returned</returns>
    private static void ParseLine(ObjBuilder builder, ReadOnlySpan<char> line)
    {
        // The file extension will be verified higher up the chain...
        // if (!ObjFileReaderUtility.FileExtensionMatches(@targetPath, ".obj"))
        // {
        //     Debug.LogWarning($"The target file at '{targetPath}' was not of the type '.obj'");
        //     return EmptyMeshData;
        // }
        var span = line;

        // Take the symbol snippet from the current line.
        var symbol = span.SliceUpTo(' ').ToString();

        switch (symbol)
        {
            case MaterialLibrary:

                builder.SetMaterialFile(span.SliceUpTo(' ').ToString());
                break;
            case UseMaterial:
                builder.AddUseMtls(span.SliceUpTo(' ').ToString());
                break;
            case Vertex:
                builder.AddVertex(FileReaderUtility.ParseVertex(span));
                break;
            case Normal:
                builder.AddNormal(FileReaderUtility.ParseVertex(span));
                break;
            case UV:
                builder.AddUV((Vector2) FileReaderUtility.ParseVertex(span, count: 2));
                break;
            case Face:
                var end = 0;
                var offset = 1;
                var other = 0;
                span = span.Trim();

                while (other != -1)
                {
                    end = span.IndexOf(' ');
                    other = end;
                    if (end == -1)
                    {
                        end = span.Length;
                        offset = 0;
                    }

                    var slice = span.Slice(0, end);

                    builder.AddFace(FileReaderUtility.ParseFace(slice, builder.VertexCount));

                    span = span.Slice(end + offset, span.Length - end - offset);
                }
                break;
            default:
                Debug.LogWarning($"Weird thing! {line.ToString()}");
                break;


        }
    }

    public static Vector3 TestParseVertex(string vertex)
    {
        var tokens = vertex.Trim().Split();

        var nums = Array.ConvertAll(tokens, (s) => float.Parse(s));


        return new Vector3(nums[0], nums[1], nums[2]);
    }


}
