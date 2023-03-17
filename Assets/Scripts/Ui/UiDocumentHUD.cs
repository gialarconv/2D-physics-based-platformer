using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public abstract class UiDocumentHUD : MonoBehaviour
{
    [SerializeField] protected UIDocument _uiDocument;
    [SerializeField] protected Volume _volume;

    protected VisualElement _rootScreen;

    protected void Awake()
    {
        _rootScreen = _uiDocument.rootVisualElement;
    }

    protected void BlurBackground(bool state)
    {
        if (_volume == null)
            return;

        DepthOfField blurDOF;
        if (_volume.profile.TryGet<DepthOfField>(out blurDOF))
        {
            blurDOF.active = state;
        }
    }
    protected void ShowVisualElement(VisualElement visualElement, bool state)
    {
        if (visualElement == null)
            return;

        visualElement.style.display = (state) ? DisplayStyle.Flex : DisplayStyle.None;
    }
}
