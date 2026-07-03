// DevExpress WinForms Charts (ChartControl) — Quickstart (C#)
// Demonstrates: bound bar series, axis/title/legend setup, selection,
//               two series, secondary axis, DateTime aggregation, crosshair.
// Package: DevExpress.Win.Charts   Host form: XtraForm

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraCharts;
using DevExpress.XtraEditors;

// ------------------------------------------------------------------
// 1. A bound bar chart with axes, title, legend, selection
// ------------------------------------------------------------------
public partial class MainForm : XtraForm {
    public MainForm() {
        InitializeComponent();

        var chart = new ChartControl { Dock = DockStyle.Fill };
        Controls.Add(chart);

        var sales = new Series("Sales 2026", ViewType.Bar);
        sales.DataSource = LoadMonthlySales();
        sales.ArgumentDataMember = nameof(MonthlySales.Month);
        sales.ValueDataMembers.AddRange(nameof(MonthlySales.Total));
        sales.ArgumentScaleType = ScaleType.DateTime;
        chart.Series.Add(sales);                  // adding the series also picks the XYDiagram

        if (chart.Diagram is XYDiagram diagram) {
            diagram.AxisX.Label.TextPattern = "{A:MMM yyyy}";
            diagram.AxisY.Label.TextPattern = "{V:c0}";
            diagram.AxisY.Title.Text        = "Revenue";
            diagram.AxisY.Title.Visibility  = DefaultBoolean.True;
        }

        chart.Titles.Add(new ChartTitle { Text = "Monthly Revenue" });
        chart.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.Right;
        chart.Legend.AlignmentVertical   = LegendAlignmentVertical.TopOutside;

        chart.SelectionMode       = ElementSelectionMode.Single;
        chart.SeriesSelectionMode = SeriesSelectionMode.Point;
        chart.ObjectSelected     += (s, e) => HandleSelection(e.Object);
    }

    void HandleSelection(object o) { /* react to the selected element */ }

    List<MonthlySales> LoadMonthlySales() => new() {
        new MonthlySales { Month = new DateTime(2026, 1, 1), Total = 30540 },
        new MonthlySales { Month = new DateTime(2026, 2, 1), Total = 28000 },
        new MonthlySales { Month = new DateTime(2026, 3, 1), Total = 33000 },
    };
}

public class MonthlySales {
    public DateTime Month { get; set; }
    public decimal Total { get; set; }
}

// ------------------------------------------------------------------
// 2. Two series on one chart
// ------------------------------------------------------------------
public static class TwoSeries {
    public static void Build(ChartControl chart, object sales) {
        var north = new Series("DevAV North", ViewType.Bar);
        var south = new Series("DevAV South", ViewType.Bar);
        chart.Series.AddRange(new[] { north, south });
        foreach (var s in chart.Series.OfType<Series>()) {
            s.DataSource = sales;
            s.ArgumentDataMember = "Product";
            s.ValueDataMembers.AddRange(s.Name == "DevAV North" ? "RevenueNorth" : "RevenueSouth");
        }
    }
}

// ------------------------------------------------------------------
// 3. Secondary Y-axis for mismatched ranges
// ------------------------------------------------------------------
public static class SecondaryAxis {
    public static void Add(ChartControl chart, Series tempSeries) {
        var diagram = (XYDiagram)chart.Diagram;
        var secondaryY = new SecondaryAxisY("temperature");
        diagram.SecondaryAxesY.Add(secondaryY);
        ((LineSeriesView)tempSeries.View).AxisY = secondaryY;
    }
}

// ------------------------------------------------------------------
// 4. DateTime axis with monthly aggregation
// ------------------------------------------------------------------
public static class DateTimeAggregation {
    public static void Apply(ChartControl chart) {
        var diagram = (XYDiagram)chart.Diagram;
        diagram.AxisX.DateTimeScaleOptions.ScaleMode         = ScaleMode.Automatic;
        diagram.AxisX.DateTimeScaleOptions.MeasureUnit       = DateTimeMeasureUnit.Month;
        diagram.AxisX.DateTimeScaleOptions.AggregateFunction = AggregateFunction.Average;
    }
}

// ------------------------------------------------------------------
// 5. Crosshair label with HTML formatting
// ------------------------------------------------------------------
public static class Crosshair {
    public static void Apply(ChartControl chart) {
        chart.CrosshairOptions.ShowArgumentLine = true;
        chart.CrosshairOptions.ShowValueLine    = true;
        chart.CrosshairOptions.GroupHeaderPattern = "<b>{A:MMM yyyy}</b>";
        chart.Series[0].CrosshairLabelPattern = "Series: {S}<br><color=#FF7200>{V:c0}</color>";
    }
}
