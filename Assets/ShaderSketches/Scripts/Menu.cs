﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField]
    Shader[] shaders;
    [SerializeField]
    GameObject container;
    [SerializeField]
    GameObject menuRoot;
    [SerializeField]
    GameObject itemPrefab;
    [SerializeField]
    Image sketch;
    [SerializeField]
    Toggle toggle;

    List<Material> menuMaterials = new List<Material>();
    List<Material> canvasMaterials = new List<Material>();

    readonly int screenAspectPropertyId = Shader.PropertyToID("_ScreenAspect");
    void Start()
    {
        for (int i = 0; i < shaders.Length; i++)
        {
            var item = Instantiate(itemPrefab);
            item.transform.SetParent(container.transform);
            item.transform.localScale = Vector3.one;

            var menuMaterial = CreateMaterial(shaders[i]);
            menuMaterial.SetInt(screenAspectPropertyId, 0);
            menuMaterials.Add(menuMaterial);

            var canvasMaterial = CreateMaterial(shaders[i]);
            canvasMaterial.SetInt(screenAspectPropertyId, 1);
            canvasMaterials.Add(canvasMaterial);

            var button = item.GetComponent<Button>();
            button.transform.GetChild(0).GetComponent<Image>().material = menuMaterial;

            var index = i;
            button.onClick.AddListener(() => ShowSketch(index));

            if (i == shaders.Length - 1) ShowSketch(i);
        }

        toggle.onValueChanged.AddListener(SetListVisible);
        SetListVisible(true);
    }

    Material CreateMaterial(Shader shader)
    {
        var material = new Material(shader);
        material.hideFlags = HideFlags.DontSave;
        return material;
    }

    void ShowSketch(int index)
    {
        if (index < 0 || index >= canvasMaterials.Count)
        {
            return;
        }
        sketch.material = canvasMaterials[index];
        SetListVisible(false);
    }

    void SetListVisible(bool show)
    {
        menuRoot.gameObject.SetActive(show);
        toggle.isOn = show;
    }
}