using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using System;
using System.Collections.ObjectModel;
using Avalonia.Interactivity;

namespace Windore.Simulations2D.GUI
{
    public class SimulationWindow : Window, ISimulationUI
    {
        private SimulationManager manager;
        private SimulationView view;
        private DispatcherTimer timer;
        private ScrollViewer scrollViewer;
        private Button scaleSwitchBtn;
        private Button pauseSwitchBtn;
        private StackPanel sidePanel;
        private Grid layoutGrid;
        private StackPanel controlsStackPanel;
        public double SidePanelWidth 
        {
            get => sidePanel.Width;
            set 
            {
                sidePanel.Width = value;

                layoutGrid.ColumnDefinitions = new ColumnDefinitions($"{sidePanel.Width},Auto,50");
                UpdateLayout();
            }
        } 

        public SimulationWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1d / 60d);
            timer.Tick += (_, __) => Tick();

            layoutGrid.ColumnDefinitions = new ColumnDefinitions($"{sidePanel.Width},Auto,50");
            UpdateLayout();


            view.SimulationObjects = new ObservableCollection<SimulationObject>();
            ShowScaledToView();
            scaleSwitchBtn.Content = "Show real size";
            pauseSwitchBtn.Content = "Pause";
            
            double prevWidth = Width;
            double prevHeight = Height;
            LayoutUpdated += (_,__) => 
            {
                // UpdateLayout should be called only when Width or Height changes
                if (Width == prevWidth && Height == prevHeight) return;
                prevWidth = Width;
                prevHeight = Height;

                UpdateLayout();
            };

            bool isClosing = false;
            bool popupVisible = false;
            Closing += async (s, e) => 
            {
                // This weird thing with isClosing bool is used to let the popup window return a result
                
                // Basically if the window is closing make sure that closing is wanted
                // then set isClosing to true and call Close method again
                if (isClosing) return;

                e.Cancel = true;

                // Don't show multiple closing dialogues
                if (popupVisible) return;
                
                popupVisible = true;
                PopupWindow closingDialog = new PopupWindow(PopupWindow.PopupButtons.YesNo, "Are you sure you want to quit? This will stop the simulation.");
                PopupWindow.PopupResult result = await closingDialog.ShowDialog<PopupWindow.PopupResult>(this);
                
                if (result == PopupWindow.PopupResult.Yes) 
                {
                   manager.StopSimulation();
                   isClosing = true;
                   Close();
                }
                else 
                {
                   popupVisible = false;
                }
            };
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            view = this.Find<SimulationView>("simulationView");
            scrollViewer = this.Find<ScrollViewer>("scrollViewer");
            scaleSwitchBtn = this.Find<Button>("scaleSwitchBtn");
            pauseSwitchBtn = this.Find<Button>("pauseSwitchBtn");
            sidePanel = this.Find<StackPanel>("sidePanel");
            layoutGrid = this.Find<Grid>("layoutGrid");
            controlsStackPanel = this.Find<StackPanel>("controlsStackPanel");
        }

        private void UpdateLayout() 
        {
            // Avalonia bindings are not used here because to my knowledge you cannot modify the value some value is bound to.
            controlsStackPanel.Width = Width - sidePanel.Width - 50;
            controlsStackPanel.Height = Height;

            scrollViewer.Width = controlsStackPanel.Width;
            scrollViewer.Height = controlsStackPanel.Height - 50;

            if (view.ScaleToSize)
            {
                view.Width = scrollViewer.Width;
                view.Height = scrollViewer.Height;
            }
        }

        private void SwitchViewMode(object sender, RoutedEventArgs e) 
        {
            if (view.ScaleToSize) 
            {
                ShowInRealSize();
                scaleSwitchBtn.Content = "Show scaled";
            }
            else 
            {
                ShowScaledToView();
                scaleSwitchBtn.Content = "Show real size";
            }
        }

        private void SwitchPause(object sender, RoutedEventArgs e)
        {
            if (manager.SimulationRunning)
            {
                manager.StopSimulation();
                pauseSwitchBtn.Content = "Resume";

                // Update the window since often it will be just a little bit late.
                Tick();
            }
            else
            {
                manager.StartSimulation();
                pauseSwitchBtn.Content = "Pause";
            }
        }

        private void AdvanceOneUpdate(object sender, RoutedEventArgs e)
        {
            if (!manager.SimulationRunning) 
            {
                manager.SimulationScene.Update();
                Tick();
            }
        }

        public void SetSidePanelContent(IControl content) 
        {
            sidePanel.Children.Clear();
            sidePanel.Children.Add(content);
        }

        public void AddSidePanelContent(IControl content) 
        {
            sidePanel.Children.Add(content);
        }

        public void ClearSidePanel() 
        {
            sidePanel.Children.Clear();
        }

        public void SetSimulationManager(SimulationManager manager)
        {
            this.manager = manager;
            
            view.SimulationSceneWidth = manager.SimulationScene.Width;
            view.SimulationSceneHeight = manager.SimulationScene.Height;
        }

        public void ShowScaledToView() 
        {
            view.Height = scrollViewer.Height;
            view.Width = scrollViewer.Width;
            view.ScaleToSize = true;

            scrollViewer.HorizontalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Hidden;
            scrollViewer.VerticalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Hidden;
        }

        public void ShowInRealSize() 
        {
            view.Height = manager.SimulationScene.Height;
            view.Width = manager.SimulationScene.Width;
            view.ScaleToSize = false;

            scrollViewer.HorizontalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Visible;
            scrollViewer.VerticalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Visible;
        }

        public void Start()
        {
            timer.Start();
        }   

        public void Stop()
        {
            timer.Stop();
        }

        private void Tick() 
        {
            view.SimulationObjects = new ObservableCollection<SimulationObject>(manager.SimulationScene.SimulationObjects);
        }
    }
}
