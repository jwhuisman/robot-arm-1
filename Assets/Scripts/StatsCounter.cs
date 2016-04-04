using UnityEngine;

public class StatsCounter : MonoBehaviour
{
    public int MovesLeft {
        get { return _movesLeft; }
        set { _movesLeft = value; }
    }
    public int MovesRight
    {
        get { return _movesRight; }
        set { _movesRight = value; }
    }
    public int Grabs
    {
        get { return _grabs; }
        set { _grabs = value; }
    }
    public int PretendGrabs
    {
        get { return _pretendGrabs; }
        set { _pretendGrabs = value; }
    }
    public int Drops
    {
        get { return _drops; }
        set { _drops = value; }
    }
    public int PretendDrops
    {
        get { return _pretendDrops; }
        set { _pretendDrops = value; }
    }
    public int Scans
    {
        get { return _scans; }
        set { _scans = value; }
    }

    private int _movesLeft = 0;
    private int _movesRight = 0;
    private int _grabs = 0;
    private int _pretendGrabs = 0;
    private int _drops = 0;
    private int _pretendDrops = 0;
    private int _scans = 0;
}