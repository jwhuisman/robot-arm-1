using Assets.Models.WorldData;
using Assets.Scripts;
using Assets.Scripts.WorldData;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public GameObject blockPrefab;

	void Start()
    {
        _world = new World();

        _leftmostVisibleStack = GetLeftmostVisibleStack();
        _rightmostVisibleStack = GetRightmostVisibleStack();

        for (int i = _leftmostVisibleStack; i <= _rightmostVisibleStack; i++)
        {
            InstantiateStack(i);
        }
	}

    void Update()
    {
        UpdateLeftmostStacks();
        UpdateRightmostStacks();
    }

    private void UpdateLeftmostStacks()
    {
        int leftmostVisibleStack = GetLeftmostVisibleStack();

        for (int i = leftmostVisibleStack; i < _leftmostVisibleStack; i++)
        {
            InstantiateStack(i);
        }

        for (int i = _leftmostVisibleStack; i < leftmostVisibleStack; i++)
        {
            RemoveStack(i);
        }

        _leftmostVisibleStack = leftmostVisibleStack;
	}

    private void UpdateRightmostStacks()
    {
        int rightmostVisibleStack = GetRightmostVisibleStack();

        for (int i = rightmostVisibleStack; i > _rightmostVisibleStack; i--)
        {
            InstantiateStack(i);
        }

        for (int i = _rightmostVisibleStack; i > rightmostVisibleStack; i--)
        {
            RemoveStack(i);
        }

        _rightmostVisibleStack = rightmostVisibleStack;
    }

    private int GetLeftmostVisibleStack()
    {
        if (Camera.main.transform.position.z >= 0)
        {
            throw new InvalidOperationException("No blocks are visible. Did you move the camera behind the blocks?");
        }

        Vector3 viewportPosition = new Vector3(0, 0, -Camera.main.transform.position.z);
        Vector3 worldPosition = Camera.main.ViewportToWorldPoint(viewportPosition);
        return (int) worldPosition.x - 1;
    }

    private int GetRightmostVisibleStack()
    {
        if (Camera.main.transform.position.z >= 0)
        {
            throw new InvalidOperationException("No blocks are visible. Did you move the camera behind the blocks?");
        }

        Vector3 viewportPosition = new Vector3(1, 1, -Camera.main.transform.position.z);
        Vector3 worldPosition = Camera.main.ViewportToWorldPoint(viewportPosition);
        return (int) worldPosition.x + 2;
    }

    private void InstantiateStack(int i)
    {
        var stack = _world.Stacks.Where(s => s.Id == i).Single();

        foreach (var block in stack.Blocks)
        {
            InstantiateBlock(block);
        }
    }

    private void InstantiateBlock(Block block)
    {
        if (_blocks.ContainsKey(block))
        {
            throw new InvalidOperationException("Block is already instantiated.");
        }

        var blockObject = GameObject.Instantiate(blockPrefab);
        blockObject.transform.position = new Vector3(block.X, block.Y);
        _blocks[block] = blockObject;
    }

    private void RemoveStack(int i)
    {
        var stack = _world.Stacks.Where(s => s.Id == i).Single();

        foreach (var block in stack.Blocks)
        {
            RemoveBlock(block);
        }
    }

    private void RemoveBlock(Block block)
    {
        if (!_blocks.ContainsKey(block))
        {
            throw new InvalidOperationException("Can't remove a block that doesn't exist.");
        }

        GameObject blockObject = _blocks[block];
        GameObject.Destroy(blockObject);
        _blocks.Remove(block);
    }

    private World _world;
    private int _leftmostVisibleStack;
    private int _rightmostVisibleStack;
    private Dictionary<Block, GameObject> _blocks = new Dictionary<Block, GameObject>();
}