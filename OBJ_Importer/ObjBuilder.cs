using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Plastic.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class ObjBuilder
{
    public readonly struct MeshData
    {
        public readonly string ObjectName;        // References to an external material file.
        public readonly string MaterialFileName;
        public readonly List<string> Usemtls;
        public readonly List<Vector3> VMapping;
        public readonly List<Vector3> VnMapping;
        public readonly List<Vector2> UvMapping;
        public readonly List<List<int[]>> Faces;
        public bool HasNormals => VnMapping.Count == 0;

        public MeshData(string objectName, string materialFileName, List<string> usemtls, List<Vector3> vMapping, List<Vector3> vnMapping, List<Vector2> uvMapping, List<List<int[]>> faces)
        {
            ObjectName = objectName;
            MaterialFileName = materialFileName;
            Usemtls = usemtls;
            VMapping = vMapping;
            VnMapping = vnMapping;
            UvMapping = uvMapping;
            Faces = faces;
        }
    }

    private string _objectName = "Default";
    private string _materialFileName = "Deafult";
    private readonly List<string> _usemtls;
    private readonly List<Vector3> _vertices;
    private readonly List<Vector3> _normals;
    private readonly List<Vector2> _uvs;
    private readonly List<List<int[]>> _faces;

    public int VertexCount => _vertices.Count;

    public ObjBuilder()
    {
        _usemtls = new List<string>();
        _vertices = new List<Vector3>();
        _normals = new List<Vector3>();
        _uvs = new List<Vector2>();
        _faces = new List<List<int[]>>();
    }


    public void SetObjectName(string objectName)
    {
        _objectName = objectName;
    }

    public void SetMaterialFile(string materialFile)
    {
        _materialFileName = materialFile;
    }

    public void AddUseMtls(string mtl)
    {
        _usemtls.Add(mtl);
        _faces.Add(new List<int[]>());
    }

    public void AddVertex(Vector3 position)
    {
        _vertices.Add(position);
    }

    public void AddNormal(Vector3 position)
    {
        _normals.Add(position);
    }

    public void AddUV(Vector2 cordinates)
    {
        _uvs.Add(cordinates);
    }

    public void AddFace(int[] face)
    {
        _faces[_usemtls.Count - 1].Add(face);
    }

    public MeshData Build() => (new MeshData(_objectName, _materialFileName, _usemtls, _vertices, _normals, _uvs, _faces));
}
