// DevExpress WinForms AccordionControl — Quickstart (C#)
// Demonstrates: side-navigation setup, groups/items, NavigationFrame, Hamburger Menu,
//               data-driven items, embedded control, search panel.
// Package: DevExpress.Win.Navigation   Host form: XtraForm

using System.Windows.Forms;
using DevExpress.XtraBars.Navigation;
using DevExpress.XtraEditors;

// ------------------------------------------------------------------
// 1. Minimal side navigation — groups and items
// ------------------------------------------------------------------
public partial class MainForm : XtraForm {
    public MainForm() {
        InitializeComponent();

        var accordion = new AccordionControl {
            Dock = DockStyle.Left,
            Width = 220,
            ShowFilterControl = ShowFilterControl.Auto   // Ctrl+F search
        };
        Controls.Add(accordion);

        accordion.BeginUpdate();

        var grpMain = new AccordionControlElement(ElementStyle.Group) {
            Text = "Main", Expanded = true
        };
        var itmDashboard = new AccordionControlElement(ElementStyle.Item) { Text = "Dashboard", Tag = "dashboard" };
        var itmReports   = new AccordionControlElement(ElementStyle.Item) { Text = "Reports",   Tag = "reports" };
        grpMain.Elements.AddRange(new[] { itmDashboard, itmReports });

        var grpSettings = new AccordionControlElement(ElementStyle.Group) { Text = "Settings" };
        grpSettings.Elements.Add(new AccordionControlElement(ElementStyle.Item) { Text = "Profile", Tag = "profile" });

        accordion.Elements.AddRange(new[] { grpMain, grpSettings });
        accordion.AllowItemSelection = true;
        accordion.EndUpdate();

        accordion.ElementClick += (s, e) => {
            if (e.Element.Style == ElementStyle.Item)
                NavigateTo(e.Element.Tag as string);
        };
    }

    void NavigateTo(string route) { /* update content area */ }
}

// ------------------------------------------------------------------
// 2. Side navigation driving a NavigationFrame
// ------------------------------------------------------------------
public partial class NavFrameForm : XtraForm {
    void Build() {
        var navFrame  = new NavigationFrame { Dock = DockStyle.Fill };
        var accordion = new AccordionControl { Dock = DockStyle.Left };
        Controls.AddRange(new Control[] { navFrame, accordion });
        accordion.SendToBack();

        var page1 = new NavigationPage { Caption = "Dashboard" };
        navFrame.Pages.Add(page1);

        var item1 = new AccordionControlElement(ElementStyle.Item) { Text = "Dashboard", Tag = page1 };
        accordion.Elements.Add(item1);

        accordion.ElementClick += (s, e) => {
            if (e.Element.Tag is NavigationPage page)
                navFrame.SelectedPage = page;
        };
    }
}

// ------------------------------------------------------------------
// 3. Hamburger Menu (overlay display mode)
// ------------------------------------------------------------------
public static class HamburgerSetup {
    public static void Apply(AccordionControl accordion) {
        accordion.ViewType = AccordionControlViewType.HamburgerMenu;
        accordion.OptionsHamburgerMenu.DisplayMode = AccordionControlDisplayMode.Overlay;
    }
}

// ------------------------------------------------------------------
// 4. Data-driven items
// ------------------------------------------------------------------
public static class DataDriven {
    public static void Populate(AccordionControl accordion, System.Collections.Generic.IEnumerable<Section> sections) {
        accordion.BeginUpdate();
        accordion.Elements.Clear();
        foreach (var section in sections) {
            var grp = new AccordionControlElement(ElementStyle.Group) { Text = section.Name, Expanded = true };
            foreach (var item in section.Items)
                grp.Elements.Add(new AccordionControlElement(ElementStyle.Item) { Text = item.Label, Tag = item.Id });
            accordion.Elements.Add(grp);
        }
        accordion.EndUpdate();
    }
}

public class Section {
    public string Name { get; set; } = "";
    public System.Collections.Generic.List<(string Label, string Id)> Items { get; set; } = new();
}

// ------------------------------------------------------------------
// 5. Item with an embedded control (ContentContainer)
// ------------------------------------------------------------------
public static class EmbeddedControl {
    public static void Add(AccordionControl accordion, AccordionControlElement settingsItem) {
        var container = new AccordionContentContainer { Padding = new Padding(-1) };  // skin-aware padding
        container.Controls.Add(new ToggleSwitch {
            Dock = DockStyle.Fill,
            Properties = { OnText = "Enabled", OffText = "Disabled" }
        });
        accordion.Controls.Add(container);          // required in addition to assigning ContentContainer
        settingsItem.ContentContainer = container;
    }
}
