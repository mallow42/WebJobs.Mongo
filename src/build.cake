#addin "Cake.Docker"
#addin "Cake.Incubator"

var target = Argument("target", "Default");

Task("Build")
    .Does(() => {
        DotNetCoreBuild("./",  new DotNetCoreBuildSettings
        {
            Configuration = "Release",
        });
    });

Task("RunUnitTests")
    .IsDependentOn("Build")
    .Does(() => {
        DotNetCoreTest("./Mallow.WebJobs.Mongo.UnitTests");
    });

Task("PublishIntegrationTestsFunctions")
    .IsDependentOn("Build")
    .Does(() =>
    {
        var functionsFolder = "./Mallow.WebJobs.Mongo.IntegrationTests.Functions";
        var settings = new DotNetCorePublishSettings
        {
            Configuration = "Release",
            OutputDirectory = $"{functionsFolder}/bin/Publish",
            NoBuild = true
        };

        DotNetCorePublish(functionsFolder, settings);
    });

Task("RunIntegrationTests")
    .IsDependentOn("PublishIntegrationTestsFunctions")
    .Does(() =>
    {
        var original = Context.Environment.WorkingDirectory;

        try
        {
            Context.Environment.WorkingDirectory = MakeAbsolute(Directory("./Mallow.WebJobs.Mongo.IntegrationTests"));
            DockerComposeDown();

            var settings = new DockerComposeUpSettings()
            {
                DetachedMode = true,
                Build = true,
                ForceRecreate = true
            };
            DockerComposeUp(settings);
            DotNetCoreTest();
        }
        finally
        {
            DockerComposeDown();
            Context.Environment.WorkingDirectory = original;
        }
    });

Task("Default")
    .IsDependentOn("RunUnitTests")
    .IsDependentOn("RunIntegrationTests")
    .Does(() => {
    
    });

RunTarget(target);