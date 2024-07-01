using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostEffectsBase : MonoBehaviour
{
	protected void CheckResources()
	{
		bool isSupported = CheckSupport();

		if (isSupported == false)
		{
			NotSupported();
		}
	}

	protected bool CheckSupport()//��������Դ�������Ƿ�����
	{
		if (SystemInfo.supportsImageEffects == false)
		{
			Debug.LogWarning("This platform does not support image effects or render textures.");
			return false;
		}

		return true;
	}


	protected void NotSupported()//��֧�־ͽ���
	{
		enabled = false;
	}

	protected void Start()
	{
		CheckResources();
	}


	protected Material CheckShaderAndCreateMaterial(Shader shader, Material material)
	{
		if (shader == null)
		{
			return null;
		}

		if (shader.isSupported && material && material.shader == shader)
			return material;

		if (!shader.isSupported)
		{
			return null;
		}
		else
		{
			material = new Material(shader);
			material.hideFlags = HideFlags.DontSave;
			if (material)
				return material;
			else
				return null;
		}
	}
}
