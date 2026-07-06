// DevExpress WinForms Layout Management — Quickstart (C#)
// Demonstrates: LayoutControl groups/items, DockManager panels, layout persistence.
// Package: DevExpress.Win.Navigation   Host form: XtraForm

using System;
using System.IO;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
using DevExpress.XtraBars.Docking;

// ------------------------------------------------------------------
// 1. LayoutControl with two groups
// ------------------------------------------------------------------
public partial class MainForm : XtraForm {
    public MainForm() {
        InitializeComponent();

        var lc = new LayoutControl { Dock = DockStyle.Fill };
        Controls.Add(lc);

        lc.BeginUpdate();
        try {
            LayoutControlGroup g1 = lc.Root.AddGroup();
            g1.Text = "Personal Info";
            g1.Name = "lcgPersonal";
            g1.AddItem("First Name", new TextEdit { Name = "edFirst" }).Name = "lciFirst";
            g1.AddItem("Last Name",  new TextEdit { Name = "edLast"  }).Name = "lciLast";

            LayoutControlGroup g2 = lc.Root.AddGroup();
            g2.Text = "Contact";
            g2.Name = "lcgContact";
            g2.AddItem("Email", new TextEdit { Name = "edEmail" }).Name = "lciEmail";
            g2.AddItem("Phone", new TextEdit { Name = "edPhone" }).Name = "lciPhone";
        }
        finally {
            lc.EndUpdate();
        }
    }
}

// ------------------------------------------------------------------
// 2. DockManager with three panels
// ------------------------------------------------------------------
public partial class DockForm : XtraForm {
    void BuildDock() {
        var dm = new DockManager { Form = this };

        var left  = dm.AddPanel(DockingStyle.Left);
        left.Text = "Explorer"; left.Width = 220; left.Name = "pnlExplorer";

        var right  = dm.AddPanel(DockingStyle.Right);
        right.Text = "Properties"; right.Width = 240; right.Name = "pnlProperties";

        var bottom = dm.AddPanel(DockingStyle.Bottom);
        bottom.Text = "Output"; bottom.Height = 120; bottom.Name = "pnlOutput";

        left.Controls.Add(new TreeView { Dock = DockStyle.Fill });
    }
}

// ------------------------------------------------------------------
// 3. Persist the DockManager layout
// ------------------------------------------------------------------
public partial class PersistForm : XtraForm {
    DockManager dockManager1;

    void Form_Load(object sender, EventArgs e) {
        if (File.Exists("dock.xml")) dockManager1.RestoreLayoutFromXml("dock.xml");
    }

    void Form_FormClosing(object sender, FormClosingEventArgs e) {
        dockManager1.SaveLayoutToXml("dock.xml");
    }
}
