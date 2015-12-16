using UnityEngine;
using System.Collections;

public abstract class Command
{
    public Command(World world, Animator animator, NetworkListener networkListener)
    {
        World = world;
        Animator = animator;
        NetworkListener = networkListener;
    }

    public bool IsDone { get; set; }
    public World World { get; set; }
    public Animator Animator { get; set; }
    public NetworkListener NetworkListener { get; set; }

    public abstract void Do();
}