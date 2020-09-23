using System.Collections.Generic;

public class ReadedConfigFile
{
    public string FullPath { get; set; }
    public string FileName { get; set; }

    public List<PackagesInfo> AllPackages { get; set; }

    public PackagesInfo GetPackagesInfoToInstall()
    {
        if (AllPackages.Count >= 1)
        {
            return AllPackages[0];
        }

        return null;
    }

    public PackagesInfo GetPackagesInfoDepedencies()
    {
        if (AllPackages.Count >= 2)
        {
            return AllPackages[1];
        }

        return null;
    }

    public ReadedConfigFile()
    {
        AllPackages = new List<PackagesInfo>();
    }

    public bool IsInputValidCheckDependencies()
    {
        var toInstall = GetPackagesInfoToInstall();
        var toCheckDependcies = GetPackagesInfoDepedencies();

        if (toInstall == null || toCheckDependcies == null)
            return true;

        List<Dependecy> dep = new List<Dependecy>();

        foreach (var installPackage in toInstall.Packages)
        {
            foreach (var dependPackage in toCheckDependcies.Packages)
            {
                if (installPackage.PackagedName
                    == dependPackage.PackagedName &&
                    installPackage.Version
                        == dependPackage.Version)
                {
                    //there is Dependecy
                    Dependecy d1 = new Dependecy();
                    d1.Name = installPackage.PackagedName;
                    d1.Version = installPackage.Version;
                    dep.Add(d1);

                    foreach (var de in dependPackage.Dependencies)
                    {
                        foreach (var anotherdependPackage in
                            toCheckDependcies.Packages)
                        {
                            if (de.PackagedName
                                == anotherdependPackage.PackagedName && de.Version
                                    == anotherdependPackage.Version)
                            {
                                if (installPackage.Version != de.Version)
                                {
                                    // there is Dependecy BUT...

                                    Dependecy d2 = new Dependecy();
                                    d2.Name = de.PackagedName;
                                    d2.Version = de.Version;
                                    bool checkCircular = false;

                                    foreach (var anothercheck in
                                        anotherdependPackage.Dependencies)
                                    {
                                        foreach (var d in dep)
                                        {
                                            if (anothercheck.PackagedName == d.Name)
                                            {
                                                checkCircular = true;
                                            }
                                        }

                                    }

                                    if (checkCircular == false)
                                    {
                                        //there is real Dependecy without CircularReference
                                        dep.Add(d2);
                                    }

                                }

                            }

                        }
                    }

                }

            }

            if (dep.Count >= 2)
            {
                return false;
            }
        }

        return true;
    }

    public class Dependecy
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public string Orignal { get { return Name + Version; } }
    }
}
