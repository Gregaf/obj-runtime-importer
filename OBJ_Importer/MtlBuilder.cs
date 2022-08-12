using System.Collections.Generic;
using UnityEngine;

public class MtlBuilder
{

    public readonly struct MaterialData
    {
        public readonly List<string> NewMaterial;
        public readonly List<Vector3> AmbientRGB;
        public readonly List<Vector3> DiffuseRGB;
        public readonly List<Vector3> SpecularRGB;
        public readonly List<float> SpecularFocus;
        public readonly List<float> OpticalDensity;
        public readonly List<float> DissolveFactor;
        public readonly List<string> DiffuseMap;

        public MaterialData(List<string> newMaterial, List<Vector3> ambientRGB, List<Vector3> diffuseRGB, List<Vector3> specularRGB, List<float> specularFocus, List<float> opticalDensity, List<float> dissolveFactor, List<string> diffuseMap)
        {
            NewMaterial = newMaterial;
            AmbientRGB = ambientRGB;
            DiffuseRGB = diffuseRGB;
            SpecularRGB = specularRGB;
            SpecularFocus = specularFocus;
            OpticalDensity = opticalDensity;
            DissolveFactor = dissolveFactor;
            DiffuseMap = diffuseMap;
        }
    }


    private readonly List<string> NewMaterial;
    private readonly List<Vector3> AmbientRGB;
    private readonly List<Vector3> DiffuseRGB;
    private readonly List<Vector3> SpecularRGB;
    private readonly List<float> SpecularFocus;
    private readonly List<float> OpticalDensity;
    private readonly List<float> DissolveFactor;
    private readonly List<string> DiffuseMap;

    public MtlBuilder()
    {
        NewMaterial = new List<string>();
        AmbientRGB = new List<Vector3>();
        DiffuseRGB = new List<Vector3>();
        SpecularRGB = new List<Vector3>();
        SpecularFocus = new List<float>();
        OpticalDensity = new List<float>();
        DissolveFactor = new List<float>();
        DiffuseMap = new List<string>();
    }

    public void AddNewMaterial(string newMaterial) => NewMaterial.Add(newMaterial);
    public void AddAmbientRGB(Vector3 ambientRgb) => AmbientRGB.Add(ambientRgb);
    public void AddDiffuseRGB(Vector3 diffuseRgb) => DiffuseRGB.Add(diffuseRgb);
    public void AddSpecularRGB(Vector3 specularRgb) => SpecularRGB.Add(specularRgb);
    public void AddSpecularFocus(float newSpecularFocus) => SpecularFocus.Add(newSpecularFocus);
    public void AddOpticalDensity(float newOpticalDensity) => OpticalDensity.Add(newOpticalDensity);
    public void AddDissolveFactor(float newDissolveFactor) => DissolveFactor.Add(newDissolveFactor);
    public void AddDiffuseMap(string newDiffuseMap) => DiffuseMap.Add(newDiffuseMap);
    
    public MaterialData Build()
    {
        return new MaterialData(NewMaterial, AmbientRGB, DiffuseRGB, SpecularRGB, SpecularFocus, OpticalDensity, DissolveFactor, DiffuseMap);
    }

}