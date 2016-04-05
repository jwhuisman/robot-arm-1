using UnityEngine;

public class StatsCounter : MonoBehaviour
{
    public int Total {
        get { return _total; }
        set { _total = value; }
    }
    public int Queued
    {
        get { return _queued; }
        set { _queued = value; }
    }
    public int MovesLeft
    {
        get { return _movesLeft; }
        set { _movesLeft = value; Total++; }
    }
    public int MovesRight
    {
        get { return _movesRight; }
        set { _movesRight = value; Total++; }
    }
    public int Grabs
    {
        get { return _grabs; }
        set { _grabs = value; Total++; }
    }
    public int PretendGrabs
    {
        get { return _pretendGrabs; }
        set { _pretendGrabs = value; Total++; }
    }
    public int Drops
    {
        get { return _drops; }
        set { _drops = value; Total++; }
    }
    public int PretendDrops
    {
        get { return _pretendDrops; }
        set { _pretendDrops = value; Total++; }
    }
    public int Scans
    {
        get { return _scans; }
        set { _scans = value; Total++; }
    }

    private int _total = 0;
    private int _queued = 0;
    private int _movesLeft = 0;
    private int _movesRight = 0;
    private int _grabs = 0;
    private int _pretendGrabs = 0;
    private int _drops = 0;
    private int _pretendDrops = 0;
    private int _scans = 0;
}