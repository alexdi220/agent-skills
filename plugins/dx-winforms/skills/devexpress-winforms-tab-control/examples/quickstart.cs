// DevExpress WinForms Tab Control (XtraTabControl) — Quickstart (C#)
// Demonstrates: static pages, runtime add/remove, closable pages, header buttons,
//               vertical headers, page icons, wizard-style navigation.
// Package: DevExpress.Win.Navigation   Host form: XtraForm

using System;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraTab;
using DevExpress.XtraTab.ViewInfo;
using DevExpress.XtraEditors;

// ------------------------------------------------------------------
// 1. Two static pages with content
// ------------------------------------------------------------------
public partial class MainForm : XtraForm {
    public MainForm() {
        InitializeComponent();

        var tabControl = new XtraTabControl { Dock = DockStyle.Fill };
        Controls.Add(tabControl);

        var pageGeneral = new XtraTabPage { Text = "General" };
        pageGeneral.Controls.Add(new SimpleButton {
            Text = "Save", Size = new Size(120, 32), Location = new Point(12, 12)
        });
        var pageAdvanced = new XtraTabPage { Text = "Advanced" };

        // The first page added becomes the initial SelectedTabPage.
        tabControl.TabPages.AddRange(new[] { pageGeneral, pageAdvanced });

        tabControl.SelectedPageChanged += (s, e) => Text = "Active: " + e.Page.Text;
    }
}

// ------------------------------------------------------------------
// 2. Add / remove pages at runtime
// ------------------------------------------------------------------
public static class PageOps {
    public static void Demo(XtraTabControl tab) {
        var newPage = new XtraTabPage { Text = "New Page" };
        tab.TabPages.Add(newPage);     // append
        tab.TabPages.RemoveAt(1);      // remove the 2nd page
    }
}

// ------------------------------------------------------------------
// 3. Closable pages — hide on close (or remove)
// ------------------------------------------------------------------
public static class ClosablePages {
    public static void Enable(XtraTabControl tab) {
        tab.ClosePageButtonShowMode = ClosePageButtonShowMode.InAllTabPageHeaders;
        tab.CloseButtonClick += (s, e) => {
            var arg = e as ClosePageButtonEventArgs;
            (arg.Page as XtraTabPage).PageVisible = false;   // or tab.TabPages.Remove(...)
        };
    }
}

// ------------------------------------------------------------------
// 4. Header buttons for overflow (Prev / Next / Close)
// ------------------------------------------------------------------
public static class HeaderButtonsSetup {
    public static void Apply(XtraTabControl tab) {
        tab.HeaderButtons = TabButtons.Prev | TabButtons.Next | TabButtons.Close;
        tab.HeaderButtonsShowMode = TabButtonShowMode.Always;
    }
}

// ------------------------------------------------------------------
// 5. Vertical headers on the left edge
// ------------------------------------------------------------------
public static class SideHeaders {
    public static void Apply(XtraTabControl tab) {
        tab.HeaderLocation = TabHeaderLocation.Left;
        tab.HeaderOrientation = TabOrientation.Vertical;
    }
}

// ------------------------------------------------------------------
// 6. Page header icon (SVG)
// ------------------------------------------------------------------
public static class PageIcon {
    public static void Apply(XtraTabPage page, DevExpress.Utils.Svg.SvgImage icon) {
        page.ImageOptions.SvgImage = icon;
        page.ImageOptions.SvgImageSize = new Size(16, 16);   // SVG defaults to 32x32
    }
}

// ------------------------------------------------------------------
// 7. Wizard-style navigation — hidden headers, custom Prev/Next buttons
// ------------------------------------------------------------------
public partial class WizardForm : XtraForm {
    XtraTabControl xtraTabControl1;
    SimpleButton buttonPrev, buttonNext;

    void Init() {
        xtraTabControl1.ShowTabHeader = DefaultBoolean.False;   // hide headers
    }

    void buttonPrev_Click(object sender, EventArgs e) {
        if (xtraTabControl1.SelectedTabPageIndex != 0)
            xtraTabControl1.SelectedTabPageIndex--;
    }

    void buttonNext_Click(object sender, EventArgs e) {
        if (xtraTabControl1.SelectedTabPageIndex != xtraTabControl1.TabPages.Count - 1)
            xtraTabControl1.SelectedTabPageIndex++;
    }

    void xtraTabControl1_SelectedPageChanged(object sender, TabPageChangedEventArgs e) {
        buttonPrev.Enabled = xtraTabControl1.SelectedTabPageIndex != 0;
        buttonNext.Enabled = xtraTabControl1.SelectedTabPageIndex != xtraTabControl1.TabPages.Count - 1;
    }
}
