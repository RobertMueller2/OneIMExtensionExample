OneIM Extension Example
==

News
--
The main branch was switched to SDK style project files to allow building DLLs for 9.3+ which switched to .NET. Due to multi-targeting, building for older versions is also possible. I cut this at 9.0 LTS, for earlier versions I suggest to use the net-framework-legacy branch.

I have not tested the new project files for all the listed versions, but it worked for 9.0 and 9.3 and building (without testing the DLLs) was also possible for 9.1.3 and 9.2.1.

With .NET, using the AssemblyResolve event for customizer and authenticator modules no longer works. Instead, I provided an `Initialize()` method that copies anticipated relevant assemblies (no guarantee I've caught every possible use case) to the executing assembly's directory for .NET and sets the AssemblyResolve event for .NET framework. Using NuGet packages where available is really the preferred way, but this sample DLL can't possible anticipate which modules an installation should have. It's probably possible to read this from an installation and then dynamically reference NuGet packages. Perhaps I'll add that in the future. That said, I couldn't get VI.Base to load customizer DLLs from referenced NuGet packages.

Speaking of NuGet, in order to use this, I had to add the frontend installation NuGet directory as a NuGet package source in Visual Studio and then install the referenced packages. See below in the Visual Studio section.

What is this?
--

This is a show case Visual Studio project that demonstrates:
  
 - Extension of OneIM API  
 - Extension of Typed Wrappers  
 - Usage of MSBuild stuff to compile against different OneIM versions from the same project directory  
  
The resulting DLLs can also be used in e.g. csharprepl or LinqPad with few dependencies to quickly create a OneIM session.

Why?
--

Occasionally, I have to demonstrate how to extend OneIM with a DLL against my lab installations. I grew tired of copying the directory everytime I install a new OneIM version in my lab. Now I just have to add a new configuration and build.  
  
Also, when I demonstrate this, I can provide the link to this repository, so there's that. ;)  
  
Usage
--

### Visual Studio

 - Before opening the project, copy `localsettings.props.template` to `localsettings.props`.
 - Edit `localsettings.props`
   - Please note, commit e1716568722b78e821af7706a5c2457222708c31 removed the need to specify an explicit PreprocessorVersion per OneIM version.
 - For each version you would like to build against:
   - provide an installation directory (`<OneIMBaseDir>`). This ensures that `VI.DB` and `VI.Base` are found at compile time and further dependencies (customizers, authenticator) can be resolved at runtime
   - provide an assembly suffix as observed in table `DialogScriptAssembly` (`<AssemblySuffix>`). This ensures that `TypedWrappers_<AssemblySuffix>.dll` is found at compile time
   - optional: provide a default connection string for use in the `ExtensionTester` or method `Utils.GetDefaultOneIMSession`
 - OneIM 9.3+: Generate packages config in the frontend's NuGet subdirectory. Use `New-PackageConfig.ps1` to create it. This overwrites an existing `packages.config` file without warning. Without this config, I couldn't use the directory as a NuGet source, but haven't done much troubleshooting here. Script is courtesy of ChatGPT ;)
 - After opening the project,
   - OneIM 9.3+: add the NuGet directory with the new config as a NuGet source and install the packages that the projects reference.
   - optional: Set ExtensionTester as start project for the solution
 - you can use build.bat where you can build a configuration (e.g. OneIM911) without opening Visual Studio. Using MSBuild requires msbuild.cfg, copy one from "msbuild.cfg examples". Dotnet build is also possible.

`GeneratedExtensionSettings.cs` in project OneIMExtensions is created by MSBuild before compiling. Therefore it is initially missing, and upon switching configurations, does not contain the correct directory and connectionstring until building.  
  
Anticipated questions
--

### Can you add SP5 of release 25.4?

I should do this fairly soon after a release.

### I don't want to wait, can I add SP5 of release 25.4?

You could. You would have to edit `localsettings.props` and add a new configuration for the new version.

### Can I compile this in VS Code?

Probably. I have not tried with this project. The repository lacks a `.vscode` directory which is probably needed to make it useful. I might try in the future.

### How do I use this in LinqPad?

#### before 9.3
Add references for

 - `OneIMExtensions.dll`, `TypedWrapperExtension.dll`, `TypedWrappers_<assembly suffix>.dll`
 - probably `VI.DB.dll` and `VI.Base.dll` depending on what you want to do
  
Customizer or Authenticator dependencies should be dynamically resolved, so long as you use the provided assembly resolver. Here is a code example:

```C#
Utils.Initialize();
var logger = Utils.GetLogger();
var session = Utils.GetDefaultOneIMSession();

var c = session.Source().Get<DialogDatabase>(Query.From("DialogDatabase").SelectNone());
c.EditionVersion.Dump();

var pobj = new DbObjectKey("<Key><T>Person</T><P>3d679e00-b66f-483b-92a4-0dc64c5c9d4e</P></Key>");
var p = session.Source().Get(pobj);
p.GetValue("Firstname").String.Dump();

var pt = session.Source().Get<Person>(pobj);
String.Format("{0}, {1}", pt.LastName, pt.FirstName).Dump();
```
  
The first line ensures that customizer and authenticator dependencies are found.

See Linqpad 5 screenshots in the assets subfolder, might link them here eventually. ;)

#### 9.3+

 - Add references for
   - `OneIMExtensions.dll`
   - `OneIMExtensions.deps.json`
   - `TypedWrapperExtensions.dll`
   - `TypedWrapperExtensions.deps.json`
   - `TypedWrappers_<assembly suffix>.dll`
- In the advanced tab, select "Copy all assemblies..." and "with native and platform-specific..."

See Linqpad 8 screenshots in the assets subfolder.

### Can I use a connection dialog?

It's possible, there is an example in the OneIM SDK, but I didn't have much use for it yet at this point. When e.g. used in LinqPad in a lab setting, it only slows me down. I'll admit though it'd be a good showcase feature.

### Can you extend this with foo or bar?

*Theoretically*, yes. But I do not intend for this VS project to be a utility library or cover implementation specific use cases. Like stated above, it's for demonstration and reference purposes. Frankly, OneIM is a job and a hobby, and being the pragmatic idealist that I am, with this personal project I do not wish to compete with my or anyone elses commercial activities. Any likely additions I could think of couldn't be provided under the given licence anyway.
  
The project's primary value from my point of view is the MSBuild stuff, which I figured out over time in my spare time. The OneIM API related parts on the other hand are in no way spectacular, clever or unique. And very likely never will be.

That said, if you do have any suggestions, feel free to open an issue.  

### Can I extend this with foo or bar?

I've added a permissive licence so feel free to do whatever you like. Again, the OneIM specific stuff is entirely basic, but I did spend a bit of my spare time to come up with the build configurations.

