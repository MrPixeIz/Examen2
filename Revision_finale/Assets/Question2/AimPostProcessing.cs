using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class AimPostProcessing : MonoBehaviour
{

	  private Material material;
    public Texture2D textureAim;
    // Use this for initialization

    void Awake()
    {
        material = new Material(Shader.Find("Hidden/ShaderQ2"));
        material.SetTexture("_AimTexture", textureAim);
    }
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest, material);
    }
}
