This litte tool is very easy to use.

Requirements:
1. NuGet.exe console application have to be in the same directory like this application.
	- Is provided by this application, but can maybe outdated some time. 


How to:
1. Open the project which you want to create as a NuGet package
2. Open the project properties
3. Open the "Buildevents"
4. Under the Postbuild section enter a string looking like the example below, that's it


Example: 
1. "C:\Path\CVNuPackage.exe" "$(TargetPath)"
2. "C:\Path\CVNuPackage.exe" "$(TargetPath)" "" "-OutputDirectory \"C:\My NuGet Path\""


Explanation:
First parameter: Path to this little application
Second parameter: Path to the .dll or .exe file from which you want to take the version number
Optional (Required if a 4th parameter is given) third parameter:
	1. Leave complete, like the first example above. 
		- ".nuspec" file is expected in the same directory like this tool
	2. Specify a .nuspec file path with filename (".nuspec" extension is not required)
		- If your ".nuspec" file is in a different directory that this tool
	3. If a 4th parameter is given you have two possibilities
		1)