namespace Windore.Simulations2D
{
    /// <summary>
    /// Represents an interface for UI used for simulations
    /// </summary>
    public interface ISimulationUI
    {
        /// <summary>
        /// Sets a SimulationManager for the UI
        /// </summary>
        /// <param name="manager">A new SimulationManager</param>
        void SetSimulationManager(SimulationManager manager);
        /// <summary>
        /// Starts updating the UI with the simulation
        /// </summary>
        void Start();
        /// <summary>
        /// Stops updating the UI with the simulation
        /// </summary>
        void Stop();
    }
}
