// ---------------- Constants ----------------

string target = Argument( "target", "taste" );

const string pretzelExe = "./_pretzel/src/Pretzel/bin/Debug/net6.0/Pretzel.dll";
const string pluginDir = "./_plugins";
const string categoryPlugin = "./_plugins/Pretzel.Categories.dll";
const string extensionPlugin = "./_plugins/Pretzel.SethExtensions.dll";
const string activityPubPlugin = "./_plugins/KristofferStrube.ActivityStreams.dll";
const string websitePlugin = "./_plugins/WebsitePlugin.dll";

DirectoryPath siteDir = Directory( "_site" );

ushort port = Argument<ushort>( "port", 8080 );

// ---------------- Tasks ----------------

Task( "taste" )
.Does(
    () =>
    {
        RunPretzel( "taste", false );
    }
).Description( "Calls pretzel taste to try the site locally" );

Task( "generate" )
.Does(
    () =>
    {
        EnsureDirectoryExists( siteDir );
        CleanDirectory( siteDir );
        RunPretzel( "bake", true );
    }
).Description( "Builds the site for publishing." );

Task( "build_pretzel" )
.Does(
    () =>
    {
        BuildPretzel();
    }
).Description( "Compiles Pretzel" );

Task( "build_plugin" )
.Does(
    () =>
    {
        BuildPlugin();
    }
).Description( "Builds the site-specific plugin" );

Task( "build_all" )
.IsDependentOn( "build_pretzel" )
.IsDependentOn( "build_plugin" )
.IsDependentOn( "taste" );

// ---------------- Functions  ----------------

void BuildPretzel()
{
    Information( "Building Pretzel..." );

    DotNetBuildSettings settings = new DotNetBuildSettings
    {
        Configuration = "Debug"
    };

    DotNetBuild( "./_pretzel/src/Pretzel.sln", settings );

    EnsureDirectoryExists( pluginDir );

    // Move Pretzel.Categories.
    {
        FilePathCollection files = GetFiles( "./_pretzel/src/Pretzel.Categories/bin/Debug/net6.0/Pretzel.Categories.*" );
        CopyFiles( files, Directory( pluginDir ) );
    }

    // Move Pretzel.SethExtensions
    {
        FilePathCollection files = GetFiles( "./_pretzel/src/Pretzel.SethExtensions/bin/Debug/net6.0/Pretzel.SethExtensions.*" );
        CopyFiles( files, Directory( pluginDir ) );
    }

    // Move ActivityPub
    {
        FilePathCollection files = GetFiles( "./_pretzel/src/ActivityStreams/src/KristofferStrube.ActivityStreams/bin/Debug/net6.0/KristofferStrube.ActivityStreams.*" );
        CopyFiles( files, Directory( pluginDir ) );
    }

    Information( "Building Pretzel... Done!" );
}

void BuildPlugin()
{
    CheckPretzelDependency();

    Information( "Building Plugin..." );

    var settings = new DotNetPublishSettings
    {
        Configuration = "Debug",
        NoBuild = false,
        NoRestore = false
    };

    DotNetPublish( "./_WebsitePlugin/WebsitePlugin/WebsitePlugin.csproj", settings );

    EnsureDirectoryExists( pluginDir );
    FilePathCollection files = GetFiles( "./_WebsitePlugin/WebsitePlugin/bin/Debug/net6.0/WebsitePlugin.*" );
    CopyFiles( files, Directory( pluginDir ) );

    Information( "Building Plugin... Done!" );
}

void RunPretzel( string argument, bool abortOnFail )
{
    CheckPretzelDependency();
    CheckSitePluginDependency();

    bool fail = false;
    string onStdOut( string line )
    {
        if( string.IsNullOrWhiteSpace( line ) )
        {
            return line;
        }
        else if( line.StartsWith( "Failed to render template" ) )
        {
            fail = true;
        }

        Console.WriteLine( line );

        return line;
    }

    string args = $"\"{pretzelExe}\" {argument} --debug";
    if( argument != "bake" )
    {
        args += $" -p {port}";
    }

    ProcessSettings settings = new ProcessSettings
    {
        Arguments = ProcessArgumentBuilder.FromString( args ),
        Silent = false,
        RedirectStandardOutput = abortOnFail,
        RedirectedStandardOutputHandler = onStdOut
    };

    int exitCode = StartProcess( "dotnet", settings );
    if( exitCode != 0 )
    {
        throw new Exception( $"Pretzel exited with exit code: {exitCode}" );
    }

    if( abortOnFail && fail )
    {
        throw new Exception( "Failed to render template" );   
    }
}

void CheckPretzelDependency()
{
    if(
        ( FileExists( pretzelExe ) == false ) ||
        ( FileExists( categoryPlugin ) == false ) ||
        ( FileExists( extensionPlugin ) == false ) ||
        ( FileExists( activityPubPlugin ) == false )
    )
    {
        BuildPretzel();
    }
}

void CheckSitePluginDependency()
{
    if( FileExists( websitePlugin ) == false )
    {
        BuildPlugin();
    }
}

RunTarget( target );
