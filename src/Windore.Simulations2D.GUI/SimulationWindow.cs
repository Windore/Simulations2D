using Windore.Simulations2D;

namespace Windore.Simulations2D.GUI
{
    public class SimulationWindow : ISimulationUI
    {
        private SimulationManager manager;

        public void SetSimulationManager(SimulationManager manager)
        {
            this.manager = manager;
        }

        public void Start()
        {
            throw new System.NotImplementedException();
        }

        public void Stop()
        {
            throw new System.NotImplementedException();
        }
    }
}
