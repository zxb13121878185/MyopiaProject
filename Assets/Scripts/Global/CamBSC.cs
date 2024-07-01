using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 摄像机的亮度、对比度和饱和度调整
/// </summary>
public class CamBSC : PostEffectsBase
{
    public Shader briSatConShader;
    private Material briSatConMaterial;
    [Range(0.0f, 3.0f)]
    public float brightness = 1.0f;
    [Range(0.0f, 3.0f)]
    public float saturation = 1.0f;
    [Range(0.0f, 3.0f)]
    public float contrast = 1.0f;

    public Material material
    {
        get
        {
            briSatConMaterial = CheckShaderAndCreateMaterial(briSatConShader, briSatConMaterial);
            return briSatConMaterial;
        }
    }
    private void Awake()
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        this.enabled = false;

#elif UNITY_ANDROID || UNITY_IOS
           this.enabled = true;
#endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (material != null)
        {
            material.SetFloat("_Brightness", brightness);
            material.SetFloat("_Saturation", saturation);
            material.SetFloat("_Contrast", contrast);

            Graphics.Blit(src, dest, material);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }

    #region 测试

    public void Test()
    {
        brightness -= 0.1f;
        saturation -= 0.1f;

    }

    #endregion

}
