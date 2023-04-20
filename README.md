OneIM Extension Example
==

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
 - For each version you would like to build against:
   - provide an installation directory (`<OneIMBaseDir>`). This ensures that `VI.DB` and `VI.Base` are found at compile time and further dependencies (customizers, authenticator) can be resolved at runtime
   - provide an assembly suffix as observed in table `DialogScriptAssembly` (`<AssemblySuffix>`). This ensures that `TypedWrappers_<AssemblySuffix>.dll` is found at compile time
   - optional: provide a default connection string for use in the `ExtensionTester` or method `Utils.GetDefaultOneIMSession`
 - After opening the project,
   - optional: Set ExtensionTester as start project for the solution

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

Add references for 

 - `OneIMExtensions.dll`, `TypedWrapperExtension.dll`, `TypedWrappers_<assembly suffix>.dll`
 - probably `VI.DB.dll` and `VI.Base.dll` depending on what you want to do
  
Customizer or Authenticator dependencies should be dynamically resolved, so long as you use the provided assembly resolver. Here is a code example:

```C#
Utils.SetAssemblyResolve();
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

### Can I use a connection dialog?

It's possible, but I didn't have much use for it yet at this point. When e.g. used in LinqPad in a lab setting, it only slows me down. I'll admit though it'd be a good showcase feature.

### Can you extend this with foo or bar?

*Theoretically*, yes. But I do not intend for this VS project to be a utility library or cover implementation specific use cases. Like stated above, it's for demonstration and reference purposes. Frankly, OneIM is a job and a hobby, and being the pragmatic idealist that I am, with this personal project I do not wish to compete with my or anyone elses commercial activities.  
  
The project's primary value from my point of view is the MSBuild stuff, which I figured out over time in my spare time. The OneIM API related parts on the other hand are in no way spectacular, clever or unique. And very likely never will be.

That said, if you do have any suggestions, feel free to open an issue.  

### Can I extend this with foo or bar?

I've added a permissive licence so feel free to do whatever you like. Again, the OneIM specific stuff is entirely basic, but I did spend a bit of my spare time to come up with the build configurations.

