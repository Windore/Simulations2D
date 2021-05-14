using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Windore.Simulations2D.GUI
{
    public class PopupWindow: Window
    {
        private TextBlock messageTB;
        private TextBlock errorTB;
        private Button okBtn;
        private Button yesBtn;
        private Button noBtn;

        public enum PopupResult 
        {
            Ok,
            Yes,
            No
        }

        public enum PopupButtons 
        {
            Ok,
            YesNo
        }

        public PopupWindow()
        {
            this.InitializeComponent();
        }

        public PopupWindow(PopupButtons btns, string message, string error="") 
        {
            this.InitializeComponent();

            messageTB.Text = message;
            errorTB.Text = error;

            if (string.IsNullOrWhiteSpace(error))
                errorTB.IsVisible = false;

            if (btns == PopupButtons.Ok) 
            {
                yesBtn.IsVisible = false;
                noBtn.IsVisible = false;
            }
            else 
            {
                okBtn.IsVisible = false;
            }

            okBtn.Click += (_,__) => {
                Close(PopupResult.Ok);
            };

            yesBtn.Click += (_,__) => {
                Close(PopupResult.Yes);
            };

            noBtn.Click += (_,__) => {
                Close(PopupResult.No);
            };
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            messageTB = this.FindControl<TextBlock>("messageTB");
            errorTB = this.FindControl<TextBlock>("errorTB");
            okBtn = this.FindControl<Button>("okBtn");
            yesBtn = this.FindControl<Button>("yesBtn");
            noBtn = this.FindControl<Button>("noBtn");
        }
    }
}