using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Windore.Simulations2D.Util.SMath;

namespace Windore.Simulations2D.GUI
{
    public class SimulationView : Control
    {
        public static readonly StyledProperty<ObservableCollection<SimulationObject>> SimulationObjectsProperty = AvaloniaProperty.Register<SimulationView, ObservableCollection<SimulationObject>>(nameof(SimulationObjects));
        public static readonly StyledProperty<SimulationObject> SelectedSimulationObjectProperty = AvaloniaProperty.Register<SimulationView, SimulationObject>(nameof(SelectedSimulationObject));

        static SimulationView()
        {
            AffectsRender<SimulationView>(SimulationObjectsProperty);
            AffectsRender<SimulationView>(SelectedSimulationObjectProperty);
        }

        public SimulationView() 
        {
            PointerPressed += SimulationView_PointerPressed;
        }

        private void SimulationView_PointerPressed(object sender, Avalonia.Input.PointerPressedEventArgs e)
        {
            Avalonia.Point pos = e.GetPosition(this);

            double xScale = 1;
            double yScale = 1;

            if (ScaleToSize && double.IsNormal(Width) && double.IsNormal(Height))
            {
                xScale = SimulationSceneWidth / Width;
                yScale = SimulationSceneHeight / Height;
            }

            Shape clickShape = new Shape(new Util.SMath.Point(pos.X * xScale, pos.Y * yScale), 10, 10, true);

            foreach(SimulationObject obj in SimulationObjects) 
            {
                if (obj.IsRemoved) continue;

                if (clickShape.Overlaps(obj.Shape)) 
                {
                    SelectedSimulationObject = obj;
                    obj.OnSelect();
                }
            }
        }

        public ObservableCollection<SimulationObject> SimulationObjects 
        {
            get => GetValue(SimulationObjectsProperty);
            set => SetValue(SimulationObjectsProperty, value);
        }

        public SimulationObject SelectedSimulationObject 
        {
            get => GetValue(SelectedSimulationObjectProperty);
            set => SetValue(SelectedSimulationObjectProperty, value);
        }

        public bool ScaleToSize { get; set; }
        public double SimulationSceneWidth { get; set; } = 1;
        public double SimulationSceneHeight { get; set; } = 1;

        public override void Render(DrawingContext context)
        {
            double xScale = 1;
            double yScale = 1;

            if (ScaleToSize && double.IsNormal(Width) && double.IsNormal(Height))
            {
                xScale = Width / SimulationSceneWidth;
                yScale = Height / SimulationSceneHeight;
            }

            double sizeScale = (xScale + yScale) / 2d;

            foreach (SimulationObject obj in SimulationObjects) 
            {
                if (obj.IsRemoved) continue;

                Shape rendered = obj.Shape;
                Color clr = new Color(obj.Color.Alpha, obj.Color.Red, obj.Color.Green, obj.Color.Blue);
                Rect rect = new Rect((rendered.Position.X - rendered.Width / 2) * xScale, (rendered.Position.Y - rendered.Height / 2) * yScale , rendered.Width * sizeScale, rendered.Height * sizeScale);
                SolidColorBrush brush = new SolidColorBrush(clr);

                if (rendered.IsEllipse) 
                {
                    if (obj == SelectedSimulationObject)
                    {
                        context.DrawGeometry(Brushes.Black, new Pen(Brushes.Black, 5), new EllipseGeometry(rect));
                    }
                    context.DrawGeometry(brush, new Pen(brush), new EllipseGeometry(rect));
                } 
                else
                {
                    if (obj == SelectedSimulationObject)
                    {
                        context.DrawRectangle(Brushes.Black, new Pen(Brushes.Black, 5), rect);
                    }
                    context.FillRectangle(brush, rect);
                }
            }
        }
    }
}
