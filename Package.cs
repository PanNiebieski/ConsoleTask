using System.Collections.Generic;

public class Package
{
    public string PackagedName { get; set; }

    public string Version { get; set; }

    public List<Package> Dependencies { get; set; }

    public Package()
    {
        Dependencies = new List<Package>();
    }
}

