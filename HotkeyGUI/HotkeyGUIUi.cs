using SonsAxLib;
using Sons.Items.Core;
using UnityEngine;
using SonsSdk;
using SUI;
using TheForest.Utils;

namespace HotkeyGUI;

using static SonsAxLib.AXSUI;
using static SUI.SUI;

public class HotkeyGUIUi
{
    public static SContainerOptions RootPanel { get; private set; }
    public static SContainerOptions TopLeftPanel { get; private set; }
    public static SContainerOptions TopRightPanel { get; private set; }
    public static SContainerOptions BottomLeftPanel { get; private set; }
    public static SContainerOptions BottomRightPanel { get; private set; }

    public static SContainerOptions SelectionListPanel { get; private set; }
     
    static Observable<Texture> Item1 = new(new Texture());
    static Observable<Texture> Item2 = new(new Texture());
    static Observable<Texture> Item3 = new(new Texture());
    static Observable<Texture> Item4 = new(new Texture());
    static Observable<Texture> Item5 = new(new Texture());
    static Observable<Texture> Item6 = new(new Texture());
    static Observable<Texture> Item7 = new(new Texture());
    static Observable<Texture> Item8 = new(new Texture());
    static Observable<Texture> Item9 = new(new Texture());
    static Observable<bool> bItem1 = new(false);
    static Observable<bool> bItem2 = new(false);
    static Observable<bool> bItem3 = new(false);
    static Observable<bool> bItem4 = new(false);
    static Observable<bool> bItem5 = new(false);
    static Observable<bool> bItem6 = new(false);
    static Observable<bool> bItem7 = new(false);
    static Observable<bool> bItem8 = new(false);
    static Observable<bool> bItem9 = new(false);

    static Observable<Texture> Item10 = new(new Texture());
    static Observable<Texture> Item11 = new(new Texture());
    static Observable<Texture> Item12 = new(new Texture());
    static Observable<Texture> Item13 = new(new Texture());
    static Observable<Texture> Item14 = new(new Texture());
    static Observable<Texture> Item15 = new(new Texture());
    static Observable<Texture> Item16 = new(new Texture());
    static Observable<Texture> Item17 = new(new Texture());
    static Observable<Texture> Item18 = new(new Texture());
    static Observable<bool> bItem10 = new(false);
    static Observable<bool> bItem11 = new(false);
    static Observable<bool> bItem12 = new(false);
    static Observable<bool> bItem13 = new(false);
    static Observable<bool> bItem14 = new(false);
    static Observable<bool> bItem15 = new(false);
    static Observable<bool> bItem16 = new(false);
    static Observable<bool> bItem17 = new(false);
    static Observable<bool> bItem18 = new(false);

    static Observable<Texture> Item19 = new(new Texture());
    static Observable<Texture> Item20 = new(new Texture());
    static Observable<Texture> Item21 = new(new Texture());
    static Observable<Texture> Item22 = new(new Texture());
    static Observable<Texture> Item23 = new(new Texture());
    static Observable<Texture> Item24 = new(new Texture());
    static Observable<Texture> Item25 = new(new Texture());
    static Observable<Texture> Item26 = new(new Texture());
    static Observable<Texture> Item27 = new(new Texture());
    static Observable<bool> bItem19 = new(false);
    static Observable<bool> bItem20 = new(false);
    static Observable<bool> bItem21 = new(false);
    static Observable<bool> bItem22 = new(false);
    static Observable<bool> bItem23 = new(false);
    static Observable<bool> bItem24 = new(false);
    static Observable<bool> bItem25 = new(false);
    static Observable<bool> bItem26 = new(false);
    static Observable<bool> bItem27 = new(false);

    static Observable<Texture> Item28 = new(new Texture());
    static Observable<Texture> Item29 = new(new Texture());
    static Observable<Texture> Item30 = new(new Texture());
    static Observable<Texture> Item31 = new(new Texture());
    static Observable<Texture> Item32 = new(new Texture());
    static Observable<Texture> Item33 = new(new Texture());
    static Observable<Texture> Item34 = new(new Texture());
    static Observable<Texture> Item35 = new(new Texture());
    static Observable<Texture> Item36 = new(new Texture());
    static Observable<bool> bItem28 = new(false);
    static Observable<bool> bItem29 = new(false);
    static Observable<bool> bItem30 = new(false);
    static Observable<bool> bItem31 = new(false);
    static Observable<bool> bItem32 = new(false);
    static Observable<bool> bItem33 = new(false);
    static Observable<bool> bItem34 = new(false);
    static Observable<bool> bItem35 = new(false);
    static Observable<bool> bItem36 = new(false);

    static Dictionary<ItemData, Observable<bool>> _hasItemDict = new();
    public static STextboxOptions _searchBox;
    public static Observable<string> _itemSearch = new("");

    static readonly float _mainPanelsBlur = 0.6f;
    static readonly float _panelsBlur = 0.3f;
    static readonly float _crossMargin = 30f;
    static readonly EBackground _panelsBg = EBackground.RoundedStandard;

    public static void Cleanup()
    {
        _hasItemDict.Clear();
    }

    private static void OnItemSearchChanged(string search)
    {
        List<ItemData> toShow = (from item in _hasItemDict.Keys
        where item.Name.ToLower().Contains(search.ToLower())
        select item).ToList();
        foreach (KeyValuePair<ItemData, Observable<bool>> item in _hasItemDict)
        {
            if (!toShow.Contains(item.Key))
            {
                _hasItemDict[item.Key].Value = false;
            }
            else if (toShow.Contains(item.Key) && LocalPlayer.Inventory.Owns(item.Key.Id)) 
                _hasItemDict[item.Key].Value = true;
        }
    }

    public static void SetBackgroundBlur(bool before, bool after)
    {
        if (!LocalPlayer._instance) return;
        float blur = after ? 0.5f : 0;
        RootPanel.Background(Color.black.WithAlpha(blur), EBackground.None);
    }

    public static void Create()
    {
        Texture cross = new();
        if (Utils.TryGetEmbeddedResourceBytes("cross", out var bytes))
        {
            cross = ResourcesLoader.ByteToTex(bytes);
        }
        else
        {
            SonsTools.ShowMessageBox("Error", "HotkeyGUI couldn't load cross.png, mod won't work");
            return;
        }

        _itemSearch.OnValueChanged += OnItemSearchChanged;

        float blur = Config.BackgroundBlur.Value ? 0.5f : 0;
        RootPanel = AxCreateFillPanel("HotkeyRootPanel", null, true).Background(Color.black.WithAlpha(blur), EBackground.None).OverrideSorting(50)
            .Material(PanelBlur.GetForShade(1f))
            .Active(false);

        TopLeftPanel = AxCreate3x3Panel("TopLeftPanel", new Vector2(500, 500), AnchorType.TopLeft, Color.black, EBackground.RoundedStandard)
            .Material(PanelBlur.GetForShade(_mainPanelsBlur)).OverrideSorting(100);

        _ = AxGetContainerAt((SPanelOptions)TopLeftPanel, 0).Background(Color.black, _panelsBg).Material(PanelBlur.GetForShade(_panelsBlur))
            - SImage.Bind(Item1).Dock(EDockType.Fill).OnClick(() => HotkeyGUI.TryEquipItem(Config.Item1.Value))
            - AxButton("⚙", () => HotkeyGUI.ChangeItemAt(Config.Item1), new Vector2(30, 30), AnchorType.BottomRight).Background(EBackground.RoundedStandard)
            - SImage.Texture(cross).Dock(EDockType.Fill).Margin(_crossMargin).OnClick(HotkeyGUI.CrossClick).BindVisibility(bItem1);
        _ = AxGetContainerAt((SPanelOptions)TopLeftPanel, 1).Background(Color.black, _panelsBg).Material(PanelBlur.GetForShade(_panelsBlur))
            - SImage.Bind(Item2).Dock(EDockType.Fill).OnClick(() => HotkeyGUI.TryEquipItem(Config.Item2.Value))
            - AxButton("⚙", () => HotkeyGUI.ChangeItemAt(Config.Item2), new Vector2(30, 30), AnchorType.BottomRight).Background(EBackground.RoundedStandard)
            - SImage.Texture(cross).Dock(EDockType.Fill).Margin(_crossMargin).OnClick(HotkeyGUI.CrossClick).BindVisibility(bItem2);
        _ = AxGetContainerAt((SPanelOptions)TopLeftPanel, 2).Background(Color.black, _panelsBg).Material(PanelBlur.GetForShade(_panelsBlur))
            - SImage.Bind(Item3).Dock(EDockType.Fill).OnClick(() => HotkeyGUI.TryEquipItem(Config.Item3.Value))
            - AxButton("⚙", () => HotkeyGUI.ChangeItemAt(Config.Item3), new Vector2(30, 30), AnchorType.BottomRight).Background(EBackground.RoundedStandard)
            - SImage.Texture(cross).Dock(EDockType.Fill).Margin(_crossMargin).OnClick(HotkeyGUI.CrossClick).BindVisibility(bItem3);
        _ = AxGetContainerAt((SPanelOptions)TopLeftPanel, 3).Background(Color.black, _panelsBg).Material(PanelBlur.GetForShade(_panelsBlur))
            - SImage.Bind(Item4).Dock(EDockType.Fill).OnClick(() => HotkeyGUI.TryEquipItem(Config.Item4.Value))
            - AxButton("⚙", () => HotkeyGUI.ChangeItemAt(Config.Item4), new Vector2(30, 30), AnchorType.BottomRight).Background(EBackground.RoundedStandard)
            - SImage.Texture(cross).Dock(EDockType.Fill).Margin(_crossMargin).OnClick(HotkeyGUI.CrossClick).BindVisibility(bItem4);
        _ = AxGetContainerAt((SPanelOptions)TopLeftPanel, 4).Background(Color.black, _panelsBg).Material(PanelBlur.GetForShade(_panelsBlur))
            - SImage.Bind(Item5).Dock(EDockType.Fill).OnClick(() => HotkeyGUI.TryEquipItem(Config.Item5.Value))
            - AxButton("⚙", () => HotkeyGUI.ChangeItemAt(Config.Item5), new Vector2(30, 30), AnchorType.BottomRight).Background(EBackground.RoundedStandard)
            - SImage.Texture(cross).Dock(EDockType.Fill).Margin(_crossMargin).OnClick(HotkeyGUI.CrossClick).BindVisibility(bItem5);
        _ = AxGetContainerAt((SPanelOptions)TopLeftPanel, 5).Background(Color.black, _panelsBg).Material(PanelBlur.GetForShade(_panelsBlur))
            - SImage.Bind(Item6).Dock(EDockType.Fill).OnClick(() => HotkeyGUI.TryEquipItem(Config.Item6.Value))
            - AxButton("⚙", () => HotkeyGUI.ChangeItemAt(Config.Item6), new Vector2(30, 30), AnchorType.BottomRight).Background(EBackground.RoundedStandard)
            - SImage.Texture(cross).Dock(EDockType.Fill).Margin(_crossMargin).OnClick(HotkeyGUI.CrossClick).BindVisibility(bItem6);
        _ = AxGetContainerAt((SPanelOptions)TopLeftPanel, 6).Background(Color.black, _panelsBg).Material(PanelBlur.GetForShade(_panelsBlur))
            - SImage.Bind(Item7).Dock(EDockType.Fill).OnClick(() => HotkeyGUI.TryEquipItem(Config.Item7.Value))
            - AxButton("⚙", () => HotkeyGUI.ChangeItemAt(Config.Item7), new Vector2(30, 30), AnchorType.BottomRight).Background(EBackground.RoundedStandard)
            - SImage.Texture(cross).Dock(EDockType.Fill).Margin(_crossMargin).OnClick(HotkeyGUI.CrossClick).BindVisibility(bItem7);
        _ = AxGetContainerAt((SPanelOptions)TopLeftPanel, 7).Background(Color.black, _panelsBg).Material(PanelBlur.GetForShade(_panelsBlur))
            - SImage.Bind(Item8).Dock(EDockType.Fill).OnClick(() => HotkeyGUI.TryEquipItem(Config.Item8.Value))
            - AxButton("⚙", () => HotkeyGUI.ChangeItemAt(Config.Item8), new Vector2(30, 30), AnchorType.BottomRight).Background(EBackground.RoundedStandard)
            - SImage.Texture(cross).Dock(EDockType.Fill).Margin(_crossMargin).OnClick(HotkeyGUI.CrossClick).BindVisibility(bItem8);
        _ = AxGetContainerAt((SPanelOptions)TopLeftPanel, 8).Background(Color.black, _panelsBg).Material(PanelBlur.GetForShade(_panelsBlur))
            - SImage.Bind(Item9).Dock(EDockType.Fill).OnClick(() => HotkeyGUI.TryEquipItem(Config.Item9.Value))
            - AxButton("⚙", () => HotkeyGUI.ChangeItemAt(Config.Item9), new Vector2(30, 30), AnchorType.BottomRight).Background(EBackground.RoundedStandard)
            - SImage.Texture(cross).Dock(EDockType.Fill).Margin(_crossMargin).OnClick(HotkeyGUI.CrossClick).BindVisibility(bItem9);

        TopRightPanel = AxCreate3x3Panel("TopRightPanel", new Vector2(500, 500), AnchorType.TopRight, Color.black, EBackground.RoundedStandard)
            .Material(PanelBlur.GetForShade(_mainPanelsBlur)).OverrideSorting(100);

        _ = AxGetContainerAt((SPanelOptions)TopRightPanel, 0).Background(Color.black, _panelsBg).Material(PanelBlur.GetForShade(_panelsBlur))
            - SImage.Bind(Item10).Dock(EDockType.Fill).OnClick(() => HotkeyGUI.TryEquipItem(Config.Item10.Value))
            - AxButton("⚙", () => HotkeyGUI.ChangeItemAt(Config.Item10), new Vector2(30, 30), AnchorType.BottomRight).Background(EBackground.RoundedStandard)
            - SImage.Texture(cross).Dock(EDockType.Fill).Margin(_crossMargin).OnClick(HotkeyGUI.CrossClick).BindVisibility(bItem10);
        _ = AxGetContainerAt((SPanelOptions)TopRightPanel, 1).Background(Color.black, _panelsBg).Material(PanelBlur.GetForShade(_panelsBlur))
            - SImage.Bind(Item11).Dock(EDockType.Fill).OnClick(() => HotkeyGUI.TryEquipItem(Config.Item11.Value))
            - AxButton("⚙", () => HotkeyGUI.ChangeItemAt(Config.Item11), new Vector2(30, 30), AnchorType.BottomRight).Background(EBackground.RoundedStandard)
            - SImage.Texture(cross).Dock(EDockType.Fill).Margin(_crossMargin).OnClick(HotkeyGUI.CrossClick).BindVisibility(bItem11);
        _ = AxGetContainerAt((SPanelOptions)TopRightPanel, 2).Background(Color.black, _panelsBg).Material(PanelBlur.GetForShade(_panelsBlur))
            - SImage.Bind(Item12).Dock(EDockType.Fill).OnClick(() => HotkeyGUI.TryEquipItem(Config.Item12.Value))
            - AxButton("⚙", () => HotkeyGUI.ChangeItemAt(Config.Item12), new Vector2(30, 30), AnchorType.BottomRight).Background(EBackground.RoundedStandard)
            - SImage.Texture(cross).Dock(EDockType.Fill).Margin(_crossMargin).OnClick(HotkeyGUI.CrossClick).BindVisibility(bItem12);
        _ = AxGetContainerAt((SPanelOptions)TopRightPanel, 3).Background(Color.black, _panelsBg).Material(PanelBlur.GetForShade(_panelsBlur))
            - SImage.Bind(Item13).Dock(EDockType.Fill).OnClick(() => HotkeyGUI.TryEquipItem(Config.Item13.Value))
            - AxButton("⚙", () => HotkeyGUI.ChangeItemAt(Config.Item13), new Vector2(30, 30), AnchorType.BottomRight).Background(EBackground.RoundedStandard)
            - SImage.Texture(cross).Dock(EDockType.Fill).Margin(_crossMargin).OnClick(HotkeyGUI.CrossClick).BindVisibility(bItem13);
        _ = AxGetContainerAt((SPanelOptions)TopRightPanel, 4).Background(Color.black, _panelsBg).Material(PanelBlur.GetForShade(_panelsBlur))
            - SImage.Bind(Item14).Dock(EDockType.Fill).OnClick(() => HotkeyGUI.TryEquipItem(Config.Item14.Value))
            - AxButton("⚙", () => HotkeyGUI.ChangeItemAt(Config.Item14), new Vector2(30, 30), AnchorType.BottomRight).Background(EBackground.RoundedStandard)
            - SImage.Texture(cross).Dock(EDockType.Fill).Margin(_crossMargin).OnClick(HotkeyGUI.CrossClick).BindVisibility(bItem14);
        _ = AxGetContainerAt((SPanelOptions)TopRightPanel, 5).Background(Color.black, _panelsBg).Material(PanelBlur.GetForShade(_panelsBlur))
            - SImage.Bind(Item15).Dock(EDockType.Fill).OnClick(() => HotkeyGUI.TryEquipItem(Config.Item15.Value))
            - AxButton("⚙", () => HotkeyGUI.ChangeItemAt(Config.Item15), new Vector2(30, 30), AnchorType.BottomRight).Background(EBackground.RoundedStandard)
            - SImage.Texture(cross).Dock(EDockType.Fill).Margin(_crossMargin).OnClick(HotkeyGUI.CrossClick).BindVisibility(bItem15);
        _ = AxGetContainerAt((SPanelOptions)TopRightPanel, 6).Background(Color.black, _panelsBg).Material(PanelBlur.GetForShade(_panelsBlur))
            - SImage.Bind(Item16).Dock(EDockType.Fill).OnClick(() => HotkeyGUI.TryEquipItem(Config.Item16.Value))
            - AxButton("⚙", () => HotkeyGUI.ChangeItemAt(Config.Item16), new Vector2(30, 30), AnchorType.BottomRight).Background(EBackground.RoundedStandard)
            - SImage.Texture(cross).Dock(EDockType.Fill).Margin(_crossMargin).OnClick(HotkeyGUI.CrossClick).BindVisibility(bItem16);
        _ = AxGetContainerAt((SPanelOptions)TopRightPanel, 7).Background(Color.black, _panelsBg).Material(PanelBlur.GetForShade(_panelsBlur))
            - SImage.Bind(Item17).Dock(EDockType.Fill).OnClick(() => HotkeyGUI.TryEquipItem(Config.Item17.Value))
            - AxButton("⚙", () => HotkeyGUI.ChangeItemAt(Config.Item17), new Vector2(30, 30), AnchorType.BottomRight).Background(EBackground.RoundedStandard)
            - SImage.Texture(cross).Dock(EDockType.Fill).Margin(_crossMargin).OnClick(HotkeyGUI.CrossClick).BindVisibility(bItem17);
        _ = AxGetContainerAt((SPanelOptions)TopRightPanel, 8).Background(Color.black, _panelsBg).Material(PanelBlur.GetForShade(_panelsBlur))
            - SImage.Bind(Item18).Dock(EDockType.Fill).OnClick(() => HotkeyGUI.TryEquipItem(Config.Item18.Value))
            - AxButton("⚙", () => HotkeyGUI.ChangeItemAt(Config.Item18), new Vector2(30, 30), AnchorType.BottomRight).Background(EBackground.RoundedStandard)
            - SImage.Texture(cross).Dock(EDockType.Fill).Margin(_crossMargin).OnClick(HotkeyGUI.CrossClick).BindVisibility(bItem18);

        BottomLeftPanel = AxCreate3x3Panel("BottomLeftPanel", new Vector2(500, 500), AnchorType.BottomLeft, Color.black, EBackground.RoundedStandard)
            .Material(PanelBlur.GetForShade(_mainPanelsBlur)).OverrideSorting(100);

        _ = AxGetContainerAt((SPanelOptions)BottomLeftPanel, 0).Background(Color.black, _panelsBg).Material(PanelBlur.GetForShade(_panelsBlur))
            - SImage.Bind(Item19).Dock(EDockType.Fill).OnClick(() => HotkeyGUI.TryEquipItem(Config.Item19.Value))
            - AxButton("⚙", () => HotkeyGUI.ChangeItemAt(Config.Item19), new Vector2(30, 30), AnchorType.BottomRight).Background(EBackground.RoundedStandard)
            - SImage.Texture(cross).Dock(EDockType.Fill).Margin(_crossMargin).OnClick(HotkeyGUI.CrossClick).BindVisibility(bItem19);
        _ = AxGetContainerAt((SPanelOptions)BottomLeftPanel, 1).Background(Color.black, _panelsBg).Material(PanelBlur.GetForShade(_panelsBlur))
            - SImage.Bind(Item20).Dock(EDockType.Fill).OnClick(() => HotkeyGUI.TryEquipItem(Config.Item20.Value))
            - AxButton("⚙", () => HotkeyGUI.ChangeItemAt(Config.Item20), new Vector2(30, 30), AnchorType.BottomRight).Background(EBackground.RoundedStandard)
            - SImage.Texture(cross).Dock(EDockType.Fill).Margin(_crossMargin).OnClick(HotkeyGUI.CrossClick).BindVisibility(bItem20);
        _ = AxGetContainerAt((SPanelOptions)BottomLeftPanel, 2).Background(Color.black, _panelsBg).Material(PanelBlur.GetForShade(_panelsBlur))
            - SImage.Bind(Item21).Dock(EDockType.Fill).OnClick(() => HotkeyGUI.TryEquipItem(Config.Item21.Value))
            - AxButton("⚙", () => HotkeyGUI.ChangeItemAt(Config.Item21), new Vector2(30, 30), AnchorType.BottomRight).Background(EBackground.RoundedStandard)
            - SImage.Texture(cross).Dock(EDockType.Fill).Margin(_crossMargin).OnClick(HotkeyGUI.CrossClick).BindVisibility(bItem21);
        _ = AxGetContainerAt((SPanelOptions)BottomLeftPanel, 3).Background(Color.black, _panelsBg).Material(PanelBlur.GetForShade(_panelsBlur))
            - SImage.Bind(Item22).Dock(EDockType.Fill).OnClick(() => HotkeyGUI.TryEquipItem(Config.Item22.Value))
            - AxButton("⚙", () => HotkeyGUI.ChangeItemAt(Config.Item22), new Vector2(30, 30), AnchorType.BottomRight).Background(EBackground.RoundedStandard)
            - SImage.Texture(cross).Dock(EDockType.Fill).Margin(_crossMargin).OnClick(HotkeyGUI.CrossClick).BindVisibility(bItem22);
        _ = AxGetContainerAt((SPanelOptions)BottomLeftPanel, 4).Background(Color.black, _panelsBg).Material(PanelBlur.GetForShade(_panelsBlur))
            - SImage.Bind(Item23).Dock(EDockType.Fill).OnClick(() => HotkeyGUI.TryEquipItem(Config.Item23.Value))
            - AxButton("⚙", () => HotkeyGUI.ChangeItemAt(Config.Item23), new Vector2(30, 30), AnchorType.BottomRight).Background(EBackground.RoundedStandard)
            - SImage.Texture(cross).Dock(EDockType.Fill).Margin(_crossMargin).OnClick(HotkeyGUI.CrossClick).BindVisibility(bItem23);
        _ = AxGetContainerAt((SPanelOptions)BottomLeftPanel, 5).Background(Color.black, _panelsBg).Material(PanelBlur.GetForShade(_panelsBlur))
            - SImage.Bind(Item24).Dock(EDockType.Fill).OnClick(() => HotkeyGUI.TryEquipItem(Config.Item24.Value))
            - AxButton("⚙", () => HotkeyGUI.ChangeItemAt(Config.Item24), new Vector2(30, 30), AnchorType.BottomRight).Background(EBackground.RoundedStandard)
            - SImage.Texture(cross).Dock(EDockType.Fill).Margin(_crossMargin).OnClick(HotkeyGUI.CrossClick).BindVisibility(bItem24);
        _ = AxGetContainerAt((SPanelOptions)BottomLeftPanel, 6).Background(Color.black, _panelsBg).Material(PanelBlur.GetForShade(_panelsBlur))
            - SImage.Bind(Item25).Dock(EDockType.Fill).OnClick(() => HotkeyGUI.TryEquipItem(Config.Item25.Value))
            - AxButton("⚙", () => HotkeyGUI.ChangeItemAt(Config.Item25), new Vector2(30, 30), AnchorType.BottomRight).Background(EBackground.RoundedStandard)
            - SImage.Texture(cross).Dock(EDockType.Fill).Margin(_crossMargin).OnClick(HotkeyGUI.CrossClick).BindVisibility(bItem25);
        _ = AxGetContainerAt((SPanelOptions)BottomLeftPanel, 7).Background(Color.black, _panelsBg).Material(PanelBlur.GetForShade(_panelsBlur))
            - SImage.Bind(Item26).Dock(EDockType.Fill).OnClick(() => HotkeyGUI.TryEquipItem(Config.Item26.Value))
            - AxButton("⚙", () => HotkeyGUI.ChangeItemAt(Config.Item26), new Vector2(30, 30), AnchorType.BottomRight).Background(EBackground.RoundedStandard)
            - SImage.Texture(cross).Dock(EDockType.Fill).Margin(_crossMargin).OnClick(HotkeyGUI.CrossClick).BindVisibility(bItem26);
        _ = AxGetContainerAt((SPanelOptions)BottomLeftPanel, 8).Background(Color.black, _panelsBg).Material(PanelBlur.GetForShade(_panelsBlur))
            - SImage.Bind(Item27).Dock(EDockType.Fill).OnClick(() => HotkeyGUI.TryEquipItem(Config.Item27.Value))
            - AxButton("⚙", () => HotkeyGUI.ChangeItemAt(Config.Item27), new Vector2(30, 30), AnchorType.BottomRight).Background(EBackground.RoundedStandard)
            - SImage.Texture(cross).Dock(EDockType.Fill).Margin(_crossMargin).OnClick(HotkeyGUI.CrossClick).BindVisibility(bItem27);

        BottomRightPanel = AxCreate3x3Panel("BottomRightPanel", new Vector2(500, 500), AnchorType.BottomRight, Color.black, EBackground.RoundedStandard)
            .Material(PanelBlur.GetForShade(_mainPanelsBlur)).OverrideSorting(100);

        _ = AxGetContainerAt((SPanelOptions)BottomRightPanel, 0).Background(Color.black, _panelsBg).Material(PanelBlur.GetForShade(_panelsBlur))
            - SImage.Bind(Item28).Dock(EDockType.Fill).OnClick(() => HotkeyGUI.TryEquipItem(Config.Item28.Value))
            - AxButton("⚙", () => HotkeyGUI.ChangeItemAt(Config.Item28), new Vector2(30, 30), AnchorType.BottomRight).Background(EBackground.RoundedStandard)
            - SImage.Texture(cross).Dock(EDockType.Fill).Margin(_crossMargin).OnClick(HotkeyGUI.CrossClick).BindVisibility(bItem28);
        _ = AxGetContainerAt((SPanelOptions)BottomRightPanel, 1).Background(Color.black, _panelsBg).Material(PanelBlur.GetForShade(_panelsBlur))
            - SImage.Bind(Item29).Dock(EDockType.Fill).OnClick(() => HotkeyGUI.TryEquipItem(Config.Item29.Value))
            - AxButton("⚙", () => HotkeyGUI.ChangeItemAt(Config.Item29), new Vector2(30, 30), AnchorType.BottomRight).Background(EBackground.RoundedStandard)
            - SImage.Texture(cross).Dock(EDockType.Fill).Margin(_crossMargin).OnClick(HotkeyGUI.CrossClick).BindVisibility(bItem29);
        _ = AxGetContainerAt((SPanelOptions)BottomRightPanel, 2).Background(Color.black, _panelsBg).Material(PanelBlur.GetForShade(_panelsBlur))
            - SImage.Bind(Item30).Dock(EDockType.Fill).OnClick(() => HotkeyGUI.TryEquipItem(Config.Item30.Value))
            - AxButton("⚙", () => HotkeyGUI.ChangeItemAt(Config.Item30), new Vector2(30, 30), AnchorType.BottomRight).Background(EBackground.RoundedStandard)
            - SImage.Texture(cross).Dock(EDockType.Fill).Margin(_crossMargin).OnClick(HotkeyGUI.CrossClick).BindVisibility(bItem30);
        _ = AxGetContainerAt((SPanelOptions)BottomRightPanel, 3).Background(Color.black, _panelsBg).Material(PanelBlur.GetForShade(_panelsBlur))
            - SImage.Bind(Item31).Dock(EDockType.Fill).OnClick(() => HotkeyGUI.TryEquipItem(Config.Item31.Value))
            - AxButton("⚙", () => HotkeyGUI.ChangeItemAt(Config.Item31), new Vector2(30, 30), AnchorType.BottomRight).Background(EBackground.RoundedStandard)
            - SImage.Texture(cross).Dock(EDockType.Fill).Margin(_crossMargin).OnClick(HotkeyGUI.CrossClick).BindVisibility(bItem31);
        _ = AxGetContainerAt((SPanelOptions)BottomRightPanel, 4).Background(Color.black, _panelsBg).Material(PanelBlur.GetForShade(_panelsBlur))
            - SImage.Bind(Item32).Dock(EDockType.Fill).OnClick(() => HotkeyGUI.TryEquipItem(Config.Item32.Value))
            - AxButton("⚙", () => HotkeyGUI.ChangeItemAt(Config.Item32), new Vector2(30, 30), AnchorType.BottomRight).Background(EBackground.RoundedStandard)
            - SImage.Texture(cross).Dock(EDockType.Fill).Margin(_crossMargin).OnClick(HotkeyGUI.CrossClick).BindVisibility(bItem32);
        _ = AxGetContainerAt((SPanelOptions)BottomRightPanel, 5).Background(Color.black, _panelsBg).Material(PanelBlur.GetForShade(_panelsBlur))
            - SImage.Bind(Item33).Dock(EDockType.Fill).OnClick(() => HotkeyGUI.TryEquipItem(Config.Item33.Value))
            - AxButton("⚙", () => HotkeyGUI.ChangeItemAt(Config.Item33), new Vector2(30, 30), AnchorType.BottomRight).Background(EBackground.RoundedStandard)
            - SImage.Texture(cross).Dock(EDockType.Fill).Margin(_crossMargin).OnClick(HotkeyGUI.CrossClick).BindVisibility(bItem33);
        _ = AxGetContainerAt((SPanelOptions)BottomRightPanel, 6).Background(Color.black, _panelsBg).Material(PanelBlur.GetForShade(_panelsBlur))
            - SImage.Bind(Item34).Dock(EDockType.Fill).OnClick(() => HotkeyGUI.TryEquipItem(Config.Item34.Value))
            - AxButton("⚙", () => HotkeyGUI.ChangeItemAt(Config.Item34), new Vector2(30, 30), AnchorType.BottomRight).Background(EBackground.RoundedStandard)
            - SImage.Texture(cross).Dock(EDockType.Fill).Margin(_crossMargin).OnClick(HotkeyGUI.CrossClick).BindVisibility(bItem34);
        _ = AxGetContainerAt((SPanelOptions)BottomRightPanel, 7).Background(Color.black, _panelsBg).Material(PanelBlur.GetForShade(_panelsBlur))
            - SImage.Bind(Item35).Dock(EDockType.Fill).OnClick(() => HotkeyGUI.TryEquipItem(Config.Item35.Value))
            - AxButton("⚙", () => HotkeyGUI.ChangeItemAt(Config.Item35), new Vector2(30, 30), AnchorType.BottomRight).Background(EBackground.RoundedStandard)
            - SImage.Texture(cross).Dock(EDockType.Fill).Margin(_crossMargin).OnClick(HotkeyGUI.CrossClick).BindVisibility(bItem35);
        _ = AxGetContainerAt((SPanelOptions)BottomRightPanel, 8).Background(Color.black, _panelsBg).Material(PanelBlur.GetForShade(_panelsBlur))
            - SImage.Bind(Item36).Dock(EDockType.Fill).OnClick(() => HotkeyGUI.TryEquipItem(Config.Item36.Value))
            - AxButton("⚙", () => HotkeyGUI.ChangeItemAt(Config.Item36), new Vector2(30, 30), AnchorType.BottomRight).Background(EBackground.RoundedStandard)
            - SImage.Texture(cross).Dock(EDockType.Fill).Margin(_crossMargin).OnClick(HotkeyGUI.CrossClick).BindVisibility(bItem36);

        SelectionListPanel = _AxCreateSidePanel("SelectionListPanel", "<color=yellow>Owned items list</color>", 700, null, EBackground.None, false)
            .OverrideSorting(100)
            .Active(false);

        SelectionListPanel.Material(PanelBlur.GetForShade(_panelsBlur));
        int counter = 0;
        foreach (var item in ItemDatabaseManager.Items)
        {
            if (item.CanBeHotKeyed)
            {
                _hasItemDict.Add(item, new Observable<bool>(true));
                var hCont = SContainer.Horizontal(0, "EX").Height(50).BindVisibility(_hasItemDict.Values.ElementAt(counter))
                    - SImage.Texture(item.UiData._icon).AspectRatio(UnityEngine.UI.AspectRatioFitter.AspectMode.HeightControlsWidth)
                    - AxMenuButton(item.Name, () => HotkeyGUI.ChangeItem(item.Id));
                AxGetMainContainer((SPanelOptions)SelectionListPanel).Add(hCont);
                counter++;
            }
        }

        TopLeftPanel.SetParent(RootPanel);
        TopRightPanel.SetParent(RootPanel);
        BottomLeftPanel.SetParent(RootPanel);
        BottomRightPanel.SetParent(RootPanel);
        SelectionListPanel.SetParent(RootPanel);
    }

    private static SPanelOptions _AxCreateSidePanel(string id, string title, float hSize, Color? color = null, EBackground style = EBackground.None, bool enableInput = true)
    {
        color ??= Color.black.WithAlpha(0.92f);
        AnchorType anchorType = AnchorType.TopCenter;

        var sidePanel = RegisterNewPanel(id, enableInput)
            .Pivot(0, 0)
            .Anchor(anchorType)
            .Background((Color)color, style)
            .Size(hSize)
            .Position(- hSize / 2, 0)
            .Vertical(5, "EC")
            .VFill();

        var titleContainer = SContainer.Background((Color)color, style).PaddingVertical(10).PHeight(50)
            - AxTextAutoSize(title).Id("menutitle");      
        sidePanel.Add(titleContainer);

        var searchContainer = SContainer.Background((Color)color, style).PaddingVertical(10).PHeight(50);
        _searchBox = AxInputText("", "Search...", _itemSearch).HOffset(10, 10).HideLabel();
        searchContainer.Add(_searchBox);
        sidePanel.Add(searchContainer);

        var scrollBar = SDiv.FlexHeight(1);
        scrollBar.Id("scrollbar");
        sidePanel.Add(scrollBar);

        var settingsScroll = SScrollContainer
        .Dock(EDockType.Fill)
        .Background(Color.black.WithAlpha(0), style)
        .Size(-20, -20)
        .As<SScrollContainerOptions>();
        settingsScroll.ContainerObject.Spacing(10);
        settingsScroll.ContainerObject.PaddingHorizontal(10);
        settingsScroll.Id("scrollcontainer");
        scrollBar.Add(settingsScroll);

        return (SPanelOptions)sidePanel;
    }

    public static void Update()
    {
        if (!LocalPlayer._instance || !HotkeyGUI.ShowPanel) return;

        foreach (KeyValuePair<ItemData, Observable<bool>> item in _hasItemDict)
        {
            if (!string.IsNullOrEmpty(_itemSearch.Value)) break;

            if (LocalPlayer.Inventory.Owns(item.Key.Id))
            {
                _hasItemDict[item.Key].Value = true;
            }
            else _hasItemDict[item.Key].Value = false;
        }

        bItem1.Value = !LocalPlayer.Inventory.Owns(int.Parse(Config.Item1.Value));
        bItem2.Value = !LocalPlayer.Inventory.Owns(int.Parse(Config.Item2.Value));
        bItem3.Value = !LocalPlayer.Inventory.Owns(int.Parse(Config.Item3.Value));
        bItem4.Value = !LocalPlayer.Inventory.Owns(int.Parse(Config.Item4.Value));
        bItem5.Value = !LocalPlayer.Inventory.Owns(int.Parse(Config.Item5.Value));
        bItem6.Value = !LocalPlayer.Inventory.Owns(int.Parse(Config.Item6.Value));
        bItem7.Value = !LocalPlayer.Inventory.Owns(int.Parse(Config.Item7.Value));
        bItem8.Value = !LocalPlayer.Inventory.Owns(int.Parse(Config.Item8.Value));
        bItem9.Value = !LocalPlayer.Inventory.Owns(int.Parse(Config.Item9.Value));
        bItem10.Value = !LocalPlayer.Inventory.Owns(int.Parse(Config.Item10.Value));
        bItem11.Value = !LocalPlayer.Inventory.Owns(int.Parse(Config.Item11.Value));
        bItem12.Value = !LocalPlayer.Inventory.Owns(int.Parse(Config.Item12.Value));
        bItem13.Value = !LocalPlayer.Inventory.Owns(int.Parse(Config.Item13.Value));
        bItem14.Value = !LocalPlayer.Inventory.Owns(int.Parse(Config.Item14.Value));
        bItem15.Value = !LocalPlayer.Inventory.Owns(int.Parse(Config.Item15.Value));
        bItem16.Value = !LocalPlayer.Inventory.Owns(int.Parse(Config.Item16.Value));
        bItem17.Value = !LocalPlayer.Inventory.Owns(int.Parse(Config.Item17.Value));
        bItem18.Value = !LocalPlayer.Inventory.Owns(int.Parse(Config.Item18.Value));
        bItem19.Value = !LocalPlayer.Inventory.Owns(int.Parse(Config.Item19.Value));
        bItem20.Value = !LocalPlayer.Inventory.Owns(int.Parse(Config.Item20.Value));
        bItem21.Value = !LocalPlayer.Inventory.Owns(int.Parse(Config.Item21.Value));
        bItem22.Value = !LocalPlayer.Inventory.Owns(int.Parse(Config.Item22.Value));
        bItem23.Value = !LocalPlayer.Inventory.Owns(int.Parse(Config.Item23.Value));
        bItem24.Value = !LocalPlayer.Inventory.Owns(int.Parse(Config.Item24.Value));
        bItem25.Value = !LocalPlayer.Inventory.Owns(int.Parse(Config.Item25.Value));
        bItem26.Value = !LocalPlayer.Inventory.Owns(int.Parse(Config.Item26.Value));
        bItem27.Value = !LocalPlayer.Inventory.Owns(int.Parse(Config.Item27.Value));
        bItem28.Value = !LocalPlayer.Inventory.Owns(int.Parse(Config.Item28.Value));
        bItem29.Value = !LocalPlayer.Inventory.Owns(int.Parse(Config.Item29.Value));
        bItem30.Value = !LocalPlayer.Inventory.Owns(int.Parse(Config.Item30.Value));
        bItem31.Value = !LocalPlayer.Inventory.Owns(int.Parse(Config.Item31.Value));
        bItem32.Value = !LocalPlayer.Inventory.Owns(int.Parse(Config.Item32.Value));
        bItem33.Value = !LocalPlayer.Inventory.Owns(int.Parse(Config.Item33.Value));
        bItem34.Value = !LocalPlayer.Inventory.Owns(int.Parse(Config.Item34.Value));
        bItem35.Value = !LocalPlayer.Inventory.Owns(int.Parse(Config.Item35.Value));
        bItem36.Value = !LocalPlayer.Inventory.Owns(int.Parse(Config.Item36.Value));

        Item1.Value = ItemDatabaseManager.ItemById(int.Parse(Config.Item1.Value)).UiData._icon;
        Item2.Value = ItemDatabaseManager.ItemById(int.Parse(Config.Item2.Value)).UiData._icon;
        Item3.Value = ItemDatabaseManager.ItemById(int.Parse(Config.Item3.Value)).UiData._icon;
        Item4.Value = ItemDatabaseManager.ItemById(int.Parse(Config.Item4.Value)).UiData._icon;
        Item5.Value = ItemDatabaseManager.ItemById(int.Parse(Config.Item5.Value)).UiData._icon;
        Item6.Value = ItemDatabaseManager.ItemById(int.Parse(Config.Item6.Value)).UiData._icon;
        Item7.Value = ItemDatabaseManager.ItemById(int.Parse(Config.Item7.Value)).UiData._icon;
        Item8.Value = ItemDatabaseManager.ItemById(int.Parse(Config.Item8.Value)).UiData._icon;
        Item9.Value = ItemDatabaseManager.ItemById(int.Parse(Config.Item9.Value)).UiData._icon;
        Item10.Value = ItemDatabaseManager.ItemById(int.Parse(Config.Item10.Value)).UiData._icon;
        Item11.Value = ItemDatabaseManager.ItemById(int.Parse(Config.Item11.Value)).UiData._icon;
        Item12.Value = ItemDatabaseManager.ItemById(int.Parse(Config.Item12.Value)).UiData._icon;
        Item13.Value = ItemDatabaseManager.ItemById(int.Parse(Config.Item13.Value)).UiData._icon;
        Item14.Value = ItemDatabaseManager.ItemById(int.Parse(Config.Item14.Value)).UiData._icon;
        Item15.Value = ItemDatabaseManager.ItemById(int.Parse(Config.Item15.Value)).UiData._icon;
        Item16.Value = ItemDatabaseManager.ItemById(int.Parse(Config.Item16.Value)).UiData._icon;
        Item17.Value = ItemDatabaseManager.ItemById(int.Parse(Config.Item17.Value)).UiData._icon;
        Item18.Value = ItemDatabaseManager.ItemById(int.Parse(Config.Item18.Value)).UiData._icon;
        Item19.Value = ItemDatabaseManager.ItemById(int.Parse(Config.Item19.Value)).UiData._icon;
        Item20.Value = ItemDatabaseManager.ItemById(int.Parse(Config.Item20.Value)).UiData._icon;
        Item21.Value = ItemDatabaseManager.ItemById(int.Parse(Config.Item21.Value)).UiData._icon;
        Item22.Value = ItemDatabaseManager.ItemById(int.Parse(Config.Item22.Value)).UiData._icon;
        Item23.Value = ItemDatabaseManager.ItemById(int.Parse(Config.Item23.Value)).UiData._icon;
        Item24.Value = ItemDatabaseManager.ItemById(int.Parse(Config.Item24.Value)).UiData._icon;
        Item25.Value = ItemDatabaseManager.ItemById(int.Parse(Config.Item25.Value)).UiData._icon;
        Item26.Value = ItemDatabaseManager.ItemById(int.Parse(Config.Item26.Value)).UiData._icon;
        Item27.Value = ItemDatabaseManager.ItemById(int.Parse(Config.Item27.Value)).UiData._icon;
        Item28.Value = ItemDatabaseManager.ItemById(int.Parse(Config.Item28.Value)).UiData._icon;
        Item29.Value = ItemDatabaseManager.ItemById(int.Parse(Config.Item29.Value)).UiData._icon;
        Item30.Value = ItemDatabaseManager.ItemById(int.Parse(Config.Item30.Value)).UiData._icon;
        Item31.Value = ItemDatabaseManager.ItemById(int.Parse(Config.Item31.Value)).UiData._icon;
        Item32.Value = ItemDatabaseManager.ItemById(int.Parse(Config.Item32.Value)).UiData._icon;
        Item33.Value = ItemDatabaseManager.ItemById(int.Parse(Config.Item33.Value)).UiData._icon;
        Item34.Value = ItemDatabaseManager.ItemById(int.Parse(Config.Item34.Value)).UiData._icon;
        Item35.Value = ItemDatabaseManager.ItemById(int.Parse(Config.Item35.Value)).UiData._icon;
        Item36.Value = ItemDatabaseManager.ItemById(int.Parse(Config.Item36.Value)).UiData._icon;
    }
}