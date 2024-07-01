using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover3DObj : MonoBehaviour
{
    public static Hover3DObj M_Instance
    {
        get
        {
            if (null == _instance)
            {
                _instance = FindObjectOfType<Hover3DObj>();
            }
            return _instance;
        }
    }
    private static Hover3DObj _instance;
    protected GameObject highlightHolder;
    protected MeshRenderer[] highlightRenderers;
    protected MeshRenderer[] existingRenderers;
    protected SkinnedMeshRenderer[] highlightSkinnedRenderers;
    protected SkinnedMeshRenderer[] existingSkinnedRenderers;
    public Material curHovePartHightMat;//悬浮在部件上时部件显示的材质
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            CreateHighlightRenderers(gameObject);
            transform.localScale *= 2;
        }
        //Debug.Log("" + Input.mousePosition);
    }
    public void CreateHighlightRenderers(GameObject HoveObj)
    {
        existingSkinnedRenderers = HoveObj.GetComponentsInChildren<SkinnedMeshRenderer>(true);

        //增加了在创建之前先判断是否为空的代码
        if (null != highlightHolder)
        {
            Destroy(highlightHolder);
        }
        highlightHolder = new GameObject(HoveObj.name);
        highlightHolder.transform.parent = HoveObj.transform;
        highlightHolder.transform.position = HoveObj.transform.position;
        highlightHolder.transform.rotation = HoveObj.transform.rotation;
        //  highlightHolder.transform.localScale = HoveObj.transform.localScale;
        highlightHolder.transform.localScale = Vector3.one;
          highlightSkinnedRenderers = new SkinnedMeshRenderer[existingSkinnedRenderers.Length];

        for (int skinnedIndex = 0; skinnedIndex < existingSkinnedRenderers.Length; skinnedIndex++)
        {
            SkinnedMeshRenderer existingSkinned = existingSkinnedRenderers[skinnedIndex];

            GameObject newSkinnedHolder = new GameObject("SkinnedHolder");
            newSkinnedHolder.transform.parent = highlightHolder.transform;
            SkinnedMeshRenderer newSkinned = newSkinnedHolder.AddComponent<SkinnedMeshRenderer>();
            Material[] materials = new Material[existingSkinned.sharedMaterials.Length];
            for (int materialIndex = 0; materialIndex < materials.Length; materialIndex++)
            {
                materials[materialIndex] = curHovePartHightMat;
            }

            newSkinned.sharedMaterials = materials;
            newSkinned.sharedMesh = existingSkinned.sharedMesh;
            newSkinned.rootBone = existingSkinned.rootBone;
            newSkinned.updateWhenOffscreen = existingSkinned.updateWhenOffscreen;
            newSkinned.bones = existingSkinned.bones;

            highlightSkinnedRenderers[skinnedIndex] = newSkinned;
        }

        MeshFilter[] existingFilters = HoveObj.GetComponentsInChildren<MeshFilter>(true);
        existingRenderers = new MeshRenderer[existingFilters.Length];
        highlightRenderers = new MeshRenderer[existingFilters.Length];

        for (int filterIndex = 0; filterIndex < existingFilters.Length; filterIndex++)
        {
            MeshFilter existingFilter = existingFilters[filterIndex];
            MeshRenderer existingRenderer = existingFilter.GetComponent<MeshRenderer>();

            if (existingFilter == null || existingRenderer == null)
                continue;

            GameObject newFilterHolder = new GameObject(existingFilters[filterIndex].name);
            newFilterHolder.transform.parent = highlightHolder.transform;
            newFilterHolder.transform.position = existingFilters[filterIndex].transform.position;
            newFilterHolder.transform.rotation = existingFilters[filterIndex].transform.rotation;
            newFilterHolder.transform.localScale = existingFilters[filterIndex].transform.localScale;
            MeshFilter newFilter = newFilterHolder.AddComponent<MeshFilter>();
            newFilter.sharedMesh = existingFilter.sharedMesh;
            MeshRenderer newRenderer = newFilterHolder.AddComponent<MeshRenderer>();

            Material[] materials = new Material[existingRenderer.sharedMaterials.Length];
            for (int materialIndex = 0; materialIndex < materials.Length; materialIndex++)
            {
                materials[materialIndex] = curHovePartHightMat;
            }
            newRenderer.sharedMaterials = materials;

            highlightRenderers[filterIndex] = newRenderer;
            existingRenderers[filterIndex] = existingRenderer;
        }
    }
}
