// DevExpress WinForms Pivot Grid (PivotGridControl) — Quickstart (C#)
// Demonstrates: field areas, data binding, date hierarchy grouping, currency format.
// Package: DevExpress.Win.PivotGrid   Host form: XtraForm

using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraPivotGrid;

// ------------------------------------------------------------------
// 1. Minimal pivot — assign fields to areas and bind data
// ------------------------------------------------------------------
public partial class Form1 : XtraForm {
    PivotGridControl pivotGridControl1;

    public Form1() {
        InitializeComponent();

        pivotGridControl1.BeginUpdate();
        try {
            pivotGridControl1.OptionsData.DataProcessingEngine = PivotDataProcessingEngine.Optimized;
            pivotGridControl1.DataSource = GetSalesData();   // IList / DataTable / BindingSource
            pivotGridControl1.Fields.AddDataSourceColumn("Country",  PivotArea.FilterArea);
            pivotGridControl1.Fields.AddDataSourceColumn("Category", PivotArea.RowArea);
            pivotGridControl1.Fields.AddDataSourceColumn("Year",     PivotArea.ColumnArea);
            pivotGridControl1.Fields.AddDataSourceColumn("Sales",    PivotArea.DataArea);
        }
        finally {
            pivotGridControl1.EndUpdate();   // always unlock, even if setup throws
        }
        pivotGridControl1.BestFit();
    }

    List<SalesRecord> GetSalesData() => new() {
        new SalesRecord { Country = "USA", Category = "Bikes", Year = 2025, Sales = 12000m },
        new SalesRecord { Country = "USA", Category = "Bikes", Year = 2026, Sales = 15000m },
        new SalesRecord { Country = "DE",  Category = "Parts", Year = 2026, Sales = 8000m  },
    };
}

public class SalesRecord {
    public string Country { get; set; } = "";
    public string Category { get; set; } = "";
    public int Year { get; set; }
    public decimal Sales { get; set; }
}

// ------------------------------------------------------------------
// 2. Date hierarchy (Year → Quarter) in the column area
// ------------------------------------------------------------------
public static class DateHierarchy {
    public static void Build(PivotGridControl pivot) {
        var fY = pivot.Fields.AddDataSourceColumn("OrderDate", PivotArea.ColumnArea);
        fY.Caption = "Year";
        ((DataSourceColumnBinding)fY.DataBinding).GroupInterval = PivotGroupInterval.DateYear;
        fY.AreaIndex = 0;

        var fQ = pivot.Fields.AddDataSourceColumn("OrderDate", PivotArea.ColumnArea);
        fQ.Caption = "Quarter";
        ((DataSourceColumnBinding)fQ.DataBinding).GroupInterval = PivotGroupInterval.DateQuarter;
        fQ.AreaIndex = 1;

        // Group them so they move together
        var g = new PivotGridGroup();
        g.AddRange(new[] { fY, fQ });
        pivot.Groups.Add(g);
    }
}

// ------------------------------------------------------------------
// 3. Currency format on a data field
// ------------------------------------------------------------------
public static class FieldFormat {
    public static void Currency(PivotGridField fieldSales) {
        fieldSales.CellFormat.FormatType   = DevExpress.Utils.FormatType.Numeric;
        fieldSales.CellFormat.FormatString = "c2";
    }
}
