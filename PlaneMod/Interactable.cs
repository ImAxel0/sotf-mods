using Sons.Gui.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PlaneMod;

public class Interactable
{
    public static Func<GameObject, LinkUiElement> GetUIElement = InteractionElement => InteractionElement.GetComponentInChildren<LinkUiElement>();

    public enum InteractableType
    {
        Take = 0,
        Open = 1
    }

    public static GameObject AddInteractable(GameObject go, float activeDistance, InteractableType type = InteractableType.Take, Texture tex = null)
    {
        bool useTex = tex != null;
        string _type = (type == InteractableType.Take) ? "screen.take" : "screen.open";

        GameObject _PickupGui_ = new()
        {
            name = "_PickupGui_"
        };

        GameObject InteractionElement = new()
        {
            name = "InteractionElement"
        };
        LinkUiElement uiElement = InteractionElement.AddComponent<LinkUiElement>();
        InteractionElement.transform.SetParent(_PickupGui_.transform, false);

        _PickupGui_.transform.SetParent(go.transform, false);

        uiElement._applyMaterial = false;
        uiElement._applyText = false;
        uiElement._applyTexture = useTex;
        uiElement._texture = tex;
        uiElement._maxDistance = activeDistance;
        uiElement._text = "";
        uiElement._uiElementId = _type;
        return InteractionElement;
    }
}
