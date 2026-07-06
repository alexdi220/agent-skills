// DevExpress WinForms Data Grid (GridControl) — Quickstart (C#)
// Demonstrates: GridView binding, CardView conversion, read-only + auto-filter,
//               per-cell editor switching, unbound column, web-style multi-select,
//               TreeList unbound with checkboxes.
// Package: DevExpress.Win.Grid (+ DevExpress.Win.TreeList for the tree)   Host: XtraForm

using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Card;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;

// ------------------------------------------------------------------
// 1. Bind a GridControl + GridView to in-memory data
// ------------------------------------------------------------------
public partial class MainForm : XtraForm {
    public MainForm() {
        InitializeComponent();

        var grid = new GridControl { Dock = DockStyle.Fill };
        var view = new GridView(grid);
        grid.MainView = view;
        grid.ViewCollection.Add(view);
        Controls.Add(grid);

        grid.DataSource = LoadOrders();        // any IList / BindingList / DataTable
        // Columns auto-create when view.OptionsBehavior.AutoPopulateColumns is true (default).

        view.OptionsView.ShowGroupPanel    = true;
        view.OptionsView.ShowAutoFilterRow = true;
        view.OptionsBehavior.Editable      = false;
    }

    System.Collections.IList LoadOrders() => new System.Collections.Generic.List<object>();
}

// ------------------------------------------------------------------
// 2. Convert a GridView to a CardView
// ------------------------------------------------------------------
public static class CardViewSetup {
    public static void Apply(GridControl grid) {
        var cardView = new CardView(grid);
        grid.ViewCollection.Add(cardView);
        grid.MainView = cardView;
        cardView.CardWidth = 240;
        cardView.OptionsView.ShowQuickCustomizeButton = false;
    }
}

// ------------------------------------------------------------------
// 3. Per-cell editor switching (read-only editor for some rows)
// ------------------------------------------------------------------
public static class PerCellEditor {
    public static void Wire(GridView view, DevExpress.XtraEditors.Repository.RepositoryItem readonlyRepository) {
        view.CustomRowCellEdit += (s, e) => {
            if (e.Column.FieldName == "Value"
                && !(bool)view.GetRowCellValue(e.RowHandle, "AllowEdit"))
                e.RepositoryItem = readonlyRepository;
        };
    }
}

// ------------------------------------------------------------------
// 4. Unbound calculated column
// ------------------------------------------------------------------
public static class UnboundColumn {
    public static void Add(GridView view) {
        GridColumn col = view.Columns.AddVisible("Total");
        col.UnboundDataType = typeof(decimal);
        col.UnboundExpression = "[Quantity] * [UnitPrice] * (1 - [Discount])";
        col.DisplayFormat.FormatType = FormatType.Numeric;
        col.DisplayFormat.FormatString = "c2";
        col.OptionsColumn.AllowEdit = false;
    }
}

// ------------------------------------------------------------------
// 5. Web-style multi-select with a check column
// ------------------------------------------------------------------
public static class MultiSelect {
    public static void Apply(GridView view) {
        view.OptionsSelection.MultiSelect = true;
        view.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
        view.OptionsSelection.ShowCheckBoxSelectorInColumnHeader = true;
    }
}

// ------------------------------------------------------------------
// 6. TreeList unbound mode with checkboxes
// ------------------------------------------------------------------
public static class TreeUnbound {
    public static void Build(TreeList treeList) {
        treeList.Columns.AddVisible("Name");
        treeList.OptionsView.ShowCheckBoxes = true;
        TreeListNode root = treeList.AppendNode(new object[] { "Headquarters" }, null);
        treeList.AppendNode(new object[] { "Sales" },     root, CheckState.Unchecked, tag: null);
        treeList.AppendNode(new object[] { "Marketing" }, root, CheckState.Unchecked, tag: null);
        treeList.ExpandAll();
    }
}
