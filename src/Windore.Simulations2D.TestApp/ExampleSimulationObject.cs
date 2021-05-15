using Windore.Simulations2D.Util.SMath;
using Windore.Simulations2D.Util;
using Windore.Simulations2D.Data;

namespace Windore.Simulations2D.TestApp
{
    // This is a custom SimulationObject, you will always have to define one.
    public class ExampleSimulationObject : SimulationObject
    {
        // This boolean is used to keep track if this object is infected
        private bool inf = false;
        internal bool IsInfected
        {
            get => inf;
            set
            {
                // If this object gets infected its color and shape are changed.
                inf = value;
                if (inf)
                {
                    Shape = new Shape(Position, 7, 7, false);
                    Color = new Color(255, 0, 0);
                }
            }
        }

        [DataPoint("JustANumber")]
        public double Number { get; }

        // The random target position of the object
        public Point TargetPosition { get; private set; }
        private float speed;

        // A constructor which calls the base constructor setting the color to be green and the shape to be a circle.
        // The object's starting point is (500, 500) and size 7
        public ExampleSimulationObject(bool infected) : base(new Shape(new Point(500, 500), 7, 7, true), new Color(0, 255, 0)) //: base(new Point(500, 500), Shape.Circle, , 7) 
        {
            // Set the position to be a new random position.
            // Hard coding the min and max values of x and y is not that smart, 
            // and a better practice would be to use the width and height of the SimulationScene
            Position = ExampleSimulationManager.Instance.SimulationRandom.Point(1000, 1000);
            IsInfected = infected;
            speed = 0.5f;

            // create a new Random target position for the object
            NewTargetPosition();

            Number = ExampleSimulationManager.Instance.SimulationRandom.Double(0, 10);
        }

        private void NewTargetPosition() 
        {
            // By using a singe SRandom object a single randomness seed can be used to for the whole simulation
            TargetPosition = ExampleSimulationManager.Instance.SimulationRandom.Point(1000, 1000);
        }

        // This method is the most important method of all simulation. It updates the state of this object
        public override void Update()  
        {
            // If this object is infected...
            if (IsInfected) 
            {
                // ... get nearby simulation objects in a range of 20...
                foreach(ExampleSimulationObject o in GetSimulationObjectsInRange(20)) 
                {
                    // ...And have a chance of 0.08% of infecting them per update.
                    if (!o.IsInfected)
                        o.IsInfected = ExampleSimulationManager.Instance.SimulationRandom.Boolean(new Percentage(0.08));
                }

                // Also this object has a chance of 0.02% of dying.
                if (ExampleSimulationManager.Instance.SimulationRandom.Boolean(new Percentage(0.02))) 
                {
                    Remove();
                }

                // Increment the number of infected objects counter
                ExampleSimulationManager.Instance.InfectedCounter++;
            }
            
            // Use the MoveTowards method to move towards the target position
            MoveTowards(TargetPosition, speed);
            if (TargetPosition.Equals(Position)) // If target is reached, create a new target
            {
                NewTargetPosition();
            }
        }
        
        // This method gets called everytime this SimulationObject is clicked.
        protected override void OnSelect() 
        {
            // Here it's used to set the Selected property of the manager.
            ExampleSimulationManager.Instance.Selected = this;
        }
    }
}