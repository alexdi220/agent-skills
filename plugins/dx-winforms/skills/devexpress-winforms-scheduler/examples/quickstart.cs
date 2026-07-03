// DevExpress WinForms Scheduler (SchedulerControl) — Quickstart (C#)
// Demonstrates: data storage + mappings, an appointment, DateNavigator pairing,
//               switching views.
// Package: DevExpress.Win.Scheduler   Host form: XtraForm

using System;
using System.Windows.Forms;
using DevExpress.XtraScheduler;
using DevExpress.XtraEditors;

// ------------------------------------------------------------------
// 1. Minimal unbound setup — storage, control, one appointment
// ------------------------------------------------------------------
public partial class MainForm : XtraForm {
    SchedulerControl schedulerControl1;
    SchedulerDataStorage storage;

    public MainForm() {
        InitializeComponent();

        // Create the control and storage in code, then wire them together.
        storage = new SchedulerDataStorage();
        schedulerControl1 = new SchedulerControl {
            Dock        = DockStyle.Fill,
            DataStorage = storage,
            Start       = DateTime.Today
        };
        Controls.Add(schedulerControl1);

        // Unbound mode: no field mappings needed — appointments are created in
        // code. (Mappings apply only to bound mode; see references/data-binding.md.)
        var apt = storage.CreateAppointment(AppointmentType.Normal,
            DateTime.Today.AddHours(10), DateTime.Today.AddHours(11), "Team Sync");
        storage.Appointments.Add(apt);
    }
}

// ------------------------------------------------------------------
// 2. Pair the scheduler with a DateNavigator
// ------------------------------------------------------------------
public static class NavigatorSetup {
    public static void Pair(DevExpress.XtraScheduler.DateNavigator dateNavigator1,
                            SchedulerControl schedulerControl1) {
        dateNavigator1.SchedulerControl = schedulerControl1;
    }
}

// ------------------------------------------------------------------
// 3. Switch to Month view
// ------------------------------------------------------------------
public static class ViewSetup {
    public static void Month(SchedulerControl schedulerControl1) {
        schedulerControl1.ActiveViewType = SchedulerViewType.Month;
    }
}
