# Conditional Formatting — WinForms PivotGridControl (DevExpress v26.1)

## Overview

The `PivotGridControl` supports Microsoft Excel-inspired conditional formatting via a collection of
**format rules** (`PivotGridControl.FormatRules`). Each rule:

1. Targets a **data (measure) field** — `PivotGridFormatRule.Measure`
2. Selects which **cells are checked** — `PivotGridFormatRule.Settings`
3. Specifies the **condition and appearance** — `FormatRuleBase.Rule`

---

## Supported Format Rule Types

| `Rule` class | Condition | Field Intersection | Data Cells |
|---|---|---|---|
| `FormatConditionRuleValue` | Value comparison (Equal, LessThan, Between, …) | ✓ | ✓ |
| `FormatConditionRuleExpression` | Custom criterion expression | ✓ | ✓ |
| `FormatConditionRuleContains` | Matches one of the constants | ✓ | ✓ |
| `FormatConditionRuleAboveBelowAverage` | Above / below intersection average | ✓ | — |
| `FormatConditionRule2ColorScale` | 2-color gradient by distribution | ✓ | — |
| `FormatConditionRule3ColorScale` | 3-color gradient by distribution | ✓ | — |
| `FormatConditionRuleTopBottom` | Top/bottom N values or percentage | ✓ | — |
| `FormatConditionRuleDataBar` | In-cell proportional bar | ✓ | — |
| `FormatConditionRuleIconSet` | Icon classification by threshold | ✓ | — |

---

## Settings (Which Cells Are Affected)

| `FormatRuleSettings` subclass | Meaning |
|---|---|
| `FormatRuleFieldIntersectionSettings` | Cells at the intersection of specific row and column fields |
| `FormatRuleTotalTypeSettings` | Cells of a specified type (data, total, grand total) |

---

## Step-by-Step: Add a Format Rule in Code

```csharp
using DevExpress.XtraPivotGrid;
using DevExpress.XtraEditors;

// --- Example: Data Bar on a field intersection ---

var rule = new PivotGridFormatRule();

// 1. Set the measure (data field)
rule.Measure = fieldExtendedPrice;

// 2. Specify the intersection (cells where Year column meets SalesPerson row)
rule.Settings = new FormatRuleFieldIntersectionSettings
{
    Column = fieldOrderYear,
    Row    = fieldSalesPerson
};

// 3. Choose and configure the rule type
rule.Rule = new FormatConditionRuleDataBar
{
    PredefinedName = "Yellow Gradient"
};

// 4. Add to the control
pivotGridControl1.FormatRules.Add(rule);
```

---

## Value Comparison Rule

```csharp
var rule = new PivotGridFormatRule
{
    Measure  = fieldSales,
    Settings = new FormatRuleFieldIntersectionSettings
    {
        Column = fieldYear,
        Row    = fieldCategory
    }
};

rule.Rule = new FormatConditionRuleValue
{
    Condition      = FormatCondition.GreaterOrEqual,
    Value1         = 10000.0,
    Appearance     = { BackColor = Color.LightGreen, ForeColor = Color.DarkGreen }
};

pivotGridControl1.FormatRules.Add(rule);
```

---

## Color Scale Rule

```csharp
var rule = new PivotGridFormatRule { Measure = fieldSales };
rule.Settings = new FormatRuleFieldIntersectionSettings
{
    Column = fieldYear,
    Row    = fieldCategory
};

rule.Rule = new FormatConditionRule2ColorScale
{
    // Uses predefined red→green gradient
    PredefinedName = "Red, White"
};

pivotGridControl1.FormatRules.Add(rule);
```

---

## Top/Bottom N Rule

```csharp
var rule = new PivotGridFormatRule { Measure = fieldSales };
rule.Settings = new FormatRuleFieldIntersectionSettings { Row = fieldCategory };

rule.Rule = new FormatConditionRuleTopBottom
{
    TopBottom  = FormatConditionTopBottomType.Top,
    Rank       = 3,       // top 3 values
    Appearance = { BackColor = Color.Gold }
};

pivotGridControl1.FormatRules.Add(rule);
```

---

## Icon Set Rule

```csharp
var rule = new PivotGridFormatRule { Measure = fieldSales };
rule.Settings = new FormatRuleFieldIntersectionSettings { Row = fieldCategory };

var iconRule = new FormatConditionRuleIconSet
{
    IconSet = new FormatConditionIconSet()
};
// Each icon is mapped to a threshold range via Value + ValueComparison.
iconRule.IconSet.Icons.Add(new FormatConditionIconSetIcon {
    PredefinedName = "Arrows3_1.png", Value = 0,  ValueComparison = FormatConditionComparisonType.GreaterOrEqual
});
iconRule.IconSet.Icons.Add(new FormatConditionIconSetIcon {
    PredefinedName = "Arrows3_2.png", Value = 33, ValueComparison = FormatConditionComparisonType.GreaterOrEqual
});
iconRule.IconSet.Icons.Add(new FormatConditionIconSetIcon {
    PredefinedName = "Arrows3_3.png", Value = 67, ValueComparison = FormatConditionComparisonType.GreaterOrEqual
});
iconRule.IconSet.ValueType = FormatConditionValueType.Percent;

rule.Rule = iconRule;

pivotGridControl1.FormatRules.Add(rule);
```

---

## Expression-Based Rule

```csharp
var rule = new PivotGridFormatRule { Measure = fieldSales };
rule.Settings = new FormatRuleFieldIntersectionSettings
{
    Column = fieldYear,
    Row    = fieldCategory
};

rule.Rule = new FormatConditionRuleExpression
{
    Expression = "[Sales] > 5000 AND [Category] = 'Beverages'",
    Appearance = { BackColor = Color.LightBlue }
};

pivotGridControl1.FormatRules.Add(rule);
```

---

## Enable/Disable and Describe Rules

```csharp
rule.Enabled     = false;           // temporarily disable
rule.Description = "Highlight top regions";

// Validate that a rule is correctly configured
bool valid = rule.IsValid;
```

---

## Runtime Conditional Formatting UI

End-users can manage rules via a context menu and a Rules Manager dialog:

```csharp
// Enable the Format Rules context menu
pivotGridControl1.OptionsMenu.EnableFormatRulesMenu = true;
// Users right-click → "Format Rules" → add/edit/delete rules interactively.
```

> **Tip**: Add a `BarManager` to the form to enable the multi-column sub-menu layout.

---

## Remove Rules

```csharp
// Remove a specific rule
pivotGridControl1.FormatRules.Remove(rule);

// Clear all rules
pivotGridControl1.FormatRules.Clear();
```

---

## Limitations

- `FormatConditionRule2ColorScale`, `FormatConditionRule3ColorScale`, and `FormatConditionRuleIconSet`
  are **not** rendered in Print Preview or exported documents.
- Data-aware export mode does not support conditional formatting.
  For XLS/XLSX export with rules, use **WYSIWYG** export mode.
- Icon and Data Bar rules are not printed/exported in WYSIWYG mode either.

---

## Key API Reference

| Member | Description |
|---|---|
| `PivotGridControl.FormatRules` | Collection of all format rules |
| `PivotGridFormatRule` | A single format rule |
| `PivotGridFormatRule.Measure` | Target data field |
| `PivotGridFormatRule.Settings` | Which cells the rule applies to |
| `PivotGridFormatRule.Rule` | Condition/appearance (`FormatConditionRuleBase` descendant) |
| `PivotGridFormatRule.Enabled` | Enable/disable the rule |
| `PivotGridFormatRule.IsValid` | Whether the rule is correctly configured |
| `PivotGridFormatRule.Description` | Human-readable description |
| `FormatRuleFieldIntersectionSettings` | Applies to a row×column field intersection |
| `FormatRuleFieldIntersectionSettings.Row` | Row field constraint |
| `FormatRuleFieldIntersectionSettings.Column` | Column field constraint |
| `FormatRuleTotalTypeSettings` | Applies to cells of a certain type (`ApplyToCell`, `ApplyToTotalCell`, `ApplyToGrandTotalCell`, `ApplyToCustomTotalCell`) |
| `FormatConditionRuleValue` | Value comparison |
| `FormatConditionRuleExpression` | Criterion expression |
| `FormatConditionRule2ColorScale` / `Rule3ColorScale` | Color gradients |
| `FormatConditionRuleDataBar` | In-cell bar |
| `FormatConditionRuleIconSet` | Icon classification |
| `FormatConditionRuleTopBottom` | Top/bottom rank |
| `FormatConditionRuleAboveBelowAverage` | Above/below average |
| `rule.Rule.PredefinedName` | Predefined style name (e.g. "Yellow Gradient") |
| `rule.Rule.Appearance` | `AppearanceObject` for custom colors/fonts |
| `OptionsMenu.EnableFormatRulesMenu` | Show context menu for runtime rule editing |

---

## Source

- Conditional Formatting: https://docs.devexpress.com/content/WindowsForms/1883?md=true
- Format Rules Page (designer): https://docs.devexpress.com/content/WindowsForms/1830?md=true
- GitHub example: https://github.com/DevExpress-Examples/winforms-pivot-grid-apply-format-rules-to-data-cells
