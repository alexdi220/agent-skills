// DevExpress WinForms MVVM — Quickstart (C#)
// Demonstrates: compile-time ViewModel codegen, MVVMContext fluent binding,
//               services (message box), confirmation behavior, messenger.
// Package: DevExpress.Win (MVVM in DevExpress.Utils.MVVM / DevExpress.Mvvm)
// Compile-time codegen requires .NET 6+.

using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.Mvvm;
using DevExpress.Mvvm.CodeGenerators;
using DevExpress.Utils.MVVM;
using DevExpress.XtraEditors;

// ------------------------------------------------------------------
// 1. ViewModel — compile-time generated properties and commands
// ------------------------------------------------------------------
[GenerateViewModel]
partial class MainViewModel {
    [GenerateProperty]
    string userName = string.Empty;

    [GenerateCommand]
    void Save() { /* persist data */ }
    bool CanSave() => !string.IsNullOrEmpty(UserName);

    [GenerateCommand]
    async Task LoadAsync() {
        var data = await FetchDataAsync();
        UserName = data.Name;
    }

    Task<(string Name, int _)> FetchDataAsync() => Task.FromResult(("Alice", 0));

    // Service usage from the ViewModel
    protected IMessageBoxService MessageBoxService => this.GetService<IMessageBoxService>();
    public void Greet() => MessageBoxService.ShowMessage($"Hello, {UserName}!");
}

// ------------------------------------------------------------------
// 2. View — controls, MVVMContext, and bindings created in code
//    (designer-free so this file compiles as-is)
// ------------------------------------------------------------------
public class MainForm : XtraForm {
    public MainForm() {
        Text = "MVVM Quickstart";

        // Create the controls in code (no designer-generated fields)
        var textEdit1 = new TextEdit  { Location = new Point(12, 12), Width = 220 };
        var btnSave   = new SimpleButton { Text = "Save",   Location = new Point(12, 48) };
        var btnLoad   = new SimpleButton { Text = "Load",   Location = new Point(96, 48) };
        var btnCancel = new SimpleButton { Text = "Cancel", Location = new Point(180, 48) };
        Controls.AddRange(new Control[] { textEdit1, btnSave, btnLoad, btnCancel });

        // Create the MVVMContext in code and point it at this form
        var mvvmContext1 = new MVVMContext { ContainerControl = this };
        mvvmContext1.ViewModelType = typeof(MainViewModel);
        var fluent = mvvmContext1.OfType<MainViewModel>();

        fluent.SetBinding(textEdit1, te => te.Text, vm => vm.UserName);
        fluent.BindCommand(btnSave, vm => vm.Save);
        fluent.BindCommand(btnLoad, vm => vm.LoadAsync);
        fluent.BindCancelCommand(btnCancel, vm => vm.LoadAsync);

        // Confirm before an irreversible action
        mvvmContext1
            .WithEvent<FormClosingEventArgs>(this, "FormClosing")
            .Confirmation(b => {
                b.Caption = "Exit";
                b.Text    = "Unsaved changes will be lost. Exit anyway?";
            });
    }
}

// ------------------------------------------------------------------
// 3. Broadcast a notification between ViewModels (Messenger)
// ------------------------------------------------------------------
public class DataRefreshMessage { }

public class ReceiverViewModel {
    public ReceiverViewModel() {
        Messenger.Default.Register<DataRefreshMessage>(this, _ => RefreshGrid());
    }
    void RefreshGrid() { }
    public void Send() => Messenger.Default.Send(new DataRefreshMessage());
    public void Cleanup() => Messenger.Default.Unregister(this);
}
