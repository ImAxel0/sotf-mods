using RedLoader;
using SonsSdk;
using SUI;
using System.Xml.Linq;
using TheForest.Utils;
using UnityEngine;
using Unity;
using static UnityEngine.EventSystems.StandaloneInputModule;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using Sons.Input;
using InputSystem = Sons.Input.InputSystem;
using UnityEngine.EventSystems;

namespace HotkeyGUI;

public class HotkeyGUI : SonsMod
{
    static bool _showPanel;
    static ConfigEntry<string> _itemToChange;

    public static bool ShowPanel
    {
        get { return _showPanel; }
        set { _showPanel = value; }
    }

    public HotkeyGUI()
    {

    }

    protected override void OnInitializeMod()
    {
        Config.Init();
    }

    protected override void OnSdkInitialized()
    {
        SettingsRegistry.CreateSettings(this, null, typeof(Config), false);
    }

    protected override void OnGameStart()
    {
        HotkeyGUIUi.Create();
        GlobalEvents.OnUpdate.Subscribe(HotkeyGUIUi.Update);
    }

    protected override void OnSonsSceneInitialized(ESonsScene sonsScene)
    {
        if (sonsScene == ESonsScene.Title) HotkeyGUIUi.Cleanup();
    }

    public static void ChangeItemAt(ConfigEntry<string> entry)
    {
        _itemToChange = entry;
        SetSelectionPanel(true);
    }

    public static void ChangeItem(int itemId)
    {
        _itemToChange.Value = $"{itemId}";
        SetSelectionPanel(false);
    }

    public static void CrossClick()
    {
        SetSelectionPanel(false);
    }

    public static void TogglePanel()
    {
        if ((!LocalPlayer.IsInWorld || LocalPlayer.IsInMidAction) && !_showPanel) 
            return;
        Config.Category.SaveToFile(false);
        _showPanel = !_showPanel;
        SetSelectionPanel(false);
        HotkeyGUIUi.RootPanel.Active(_showPanel);
    }

    public static void SetSelectionPanel(bool onoff)
    {
        HotkeyGUIUi.SelectionListPanel.Active(onoff);
        HotkeyGUIUi._itemSearch.Value = "";
        if (onoff) HotkeyGUIUi._searchBox.InputFieldObject.Select();
    }

    public static void TryEquipItem(string itemId)
    {
        HotkeyGUIUi.SelectionListPanel.Active(false);
        HotkeyGUIUi._itemSearch.Value = "";
        if (LocalPlayer.Inventory.TryEquip(int.Parse(itemId), false))
        {
            HotkeyGUIUi.RootPanel.Active(false);
            _showPanel = false;
        }
    }
}