// DevExpress WinForms Ribbon and Bars — Quickstart (C#)
// Demonstrates: Ribbon UI (pages/groups, item+links, QAT, status bar) and the
//               classic BarManager menu/toolbar stack.
// Package: DevExpress.Win.Navigation   Host form: RibbonForm (for the Ribbon)

using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;

// ------------------------------------------------------------------
// 1. Ribbon UI — one item, links in a group and the Quick Access Toolbar
// ------------------------------------------------------------------
public partial class MainForm : RibbonForm {
    DevExpress.Utils.Svg.SvgImageCollection svgImageCollection1;

    public MainForm() {
        InitializeComponent();

        var ribbon = new RibbonControl { Images = svgImageCollection1 };
        Controls.Add(ribbon);

        var home = new RibbonPage("Home");
        ribbon.Pages.Add(home);
        var file = new RibbonPageGroup("File");
        home.Groups.Add(file);

        // Create the item once; add links wherever it should appear.
        var itemNew = ribbon.Items.CreateButton("New");
        itemNew.Id = ribbon.Manager.GetNewItemId();           // required for layout (de)serialization
        itemNew.ImageOptions.SvgImage = svgImageCollection1["new"];
        itemNew.RibbonStyle = RibbonItemStyles.Large;
        itemNew.ItemClick += (s, e) => MessageBox.Show("New!");

        file.ItemLinks.Add(itemNew);
        ribbon.Toolbar.ItemLinks.Add(itemNew);                // also in the QAT

        // Status bar
        var status = new RibbonStatusBar(ribbon) { Parent = this };
        var statusLabel = new BarStaticItem { Caption = "Ready", Alignment = BarItemLinkAlignment.Right };
        ribbon.Items.Add(statusLabel);
        status.ItemLinks.Add(statusLabel);
    }
}

// ------------------------------------------------------------------
// 2. Classic Bars UI — BarManager with a menu bar and shortcuts
// ------------------------------------------------------------------
public partial class ClassicForm : DevExpress.XtraEditors.XtraForm {
    void BuildBars() {
        var bars = new BarManager { Form = this };
        bars.BeginUpdate();

        var menu  = new Bar(bars, "Main")  { DockStyle = BarDockStyle.Top, DockRow = 0 };
        var tools = new Bar(bars, "Tools") { DockStyle = BarDockStyle.Top, DockRow = 1 };
        var status = new Bar(bars, "Status") {
            DockStyle = BarDockStyle.Bottom,
            OptionsBar = { AllowQuickCustomization = false, DrawDragBorder = false, UseWholeRow = true }
        };
        bars.MainMenu = menu;
        bars.StatusBar = status;

        var fileMenu = new BarSubItem(bars, "&File");
        var newCmd   = new BarButtonItem(bars, "&New") { ItemShortcut = new BarShortcut(Keys.Control | Keys.N) };
        newCmd.ItemClick += (_, _) => CreateNewDocument();
        fileMenu.AddItem(newCmd);
        menu.AddItem(fileMenu);

        bars.EndUpdate();
    }

    void CreateNewDocument() { }
}
