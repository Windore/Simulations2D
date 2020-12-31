# Tutorial

This tutorial will go over the basics of creating a simulation.

## Table of Contents
* [Creating a simulation](#creating-a-simulation)
    * [Simulation Objects](#simulation-objects)
    * [Simulation Scene](#simulation-scene)
    * [Simulation Manager](#simulation-manager)
* [Utilities](#utilities)
    * [SRandom](#srandom)
    * [SMath](#smath)
## Creating a Simulation
### Simulation Objects
Simulation Objects are the most important part of every simulation as should be expected. When creating a simulation you must first create your custom simulation objects. Simulation object will be shortened to SO later in this tutorial.

```cs
// Inherit the simulation object class
public class CustomSimulationObject : SimulationObject
{
    // And implement abstract methods
    public override void Update()
    {

    }
}

```

The `Update` method contains code which is responsible for updating the state of the SO. Everything the SO does should be added here. Let's add some code to the `Update` method.

```cs
public override void Update()
{
    
}
```
### Simulation Scene
### Simulation Manager
## Utilities
### SRandom
### SMath
#### Point
#### Percentage
