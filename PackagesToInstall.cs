using System.Collections.Generic;

public class PackagesInfo
{
    public int NumberPackagesToInstall { get; set; }

    public List<Package> Packages { get; set; }


    public PackagesInfo()
    {
        Packages = new List<Package>();
    }
}
