// DevExpress WinForms Property Grid (PropertyGridControl) — Quickstart (C#)
// Demonstrates: SelectedObject, attributes/categories, nested expandable props,
//               DX collection editor, expanding a row, Office view with tabs.
// Package: DevExpress.Win.Navigation   Host form: XtraForm

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraVerticalGrid;

// ------------------------------------------------------------------
// 1. Show an object's properties
// ------------------------------------------------------------------
public partial class SettingsForm : XtraForm {
    PropertyGridControl propertyGridControl1;

    public SettingsForm() {
        InitializeComponent();
        propertyGridControl1.SelectedObject = new AppSettings();
    }
}

public class AppSettings {
    [Category("General")]
    [DisplayName("Application Title")]
    [Description("Shown in the title bar.")]
    public string Title { get; set; } = "My App";

    [Category("General")]
    public bool StartMinimized { get; set; }

    [Category("Data")]
    [DisplayName("Database")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public DatabaseSettings Database { get; set; } = new DatabaseSettings();

    [Category("Data")]
    [DisplayName("Allowed Paths")]
    public List<string> AllowedPaths { get; set; } = new();
}

[TypeConverter(typeof(ExpandableObjectConverter))]
public class DatabaseSettings {
    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 5432;
    public override string ToString() => $"{Host}:{Port}";
}

// ------------------------------------------------------------------
// 2. Use the DevExpress collection editor; expand a row on load
// ------------------------------------------------------------------
public static class GridSetup {
    public static void Apply(PropertyGridControl grid) {
        grid.OptionsCollectionEditor.UseDXCollectionEditor = true;

        grid.DataSourceChanged += (s, e) =>
            grid.GetRowByFieldName("Database").Expanded = true;
    }
}

// ------------------------------------------------------------------
// 3. Office view with category tabs
// ------------------------------------------------------------------
public static class OfficeView {
    public static void Apply(PropertyGridControl grid) {
        grid.ActiveViewType = PropertyGridView.Office;
        grid.TabPanelCustomize += (s, e) => {
            var tab1 = new Tab { Caption = "General" };
            tab1.CategoryNames.Add("General");

            var tab2 = new Tab { Caption = "Data" };
            tab2.CategoryNames.Add("Data");

            e.Tabs.Add(tab1);
            e.Tabs.Add(tab2);
        };
    }
}
