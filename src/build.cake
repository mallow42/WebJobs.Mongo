#addin "Cake.Docker"
#addin "Cake.Incubator"

var target = Argument("target", "Default");

const string integrationTestsFolder =  "./Mallow.WebJobs.Mongo.IntegrationTests";

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
        DockerComposeDown(new DockerComposeDownSettings(){
            WorkingDirectory = integrationTestsFolder,
        });
        DockerComposeUp(new DockerComposeUpSettings()
        {
            WorkingDirectory = integrationTestsFolder,
            DetachedMode = true,
            Build = true,
            ForceRecreate = true
        });
        DotNetCoreTest();
    })
    .Finally(() => {
        DockerComposeDown(new DockerComposeDownSettings(){
            WorkingDirectory = integrationTestsFolder,
        });
    });

Task("Default")
    .IsDependentOn("RunUnitTests")
    .IsDependentOn("RunIntegrationTests");

RunTarget(target);