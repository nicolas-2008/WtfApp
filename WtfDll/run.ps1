# Load the presentation framework assembly
Add-Type -AssemblyName PresentationFramework

# Load the assembly containing your MainWindow class
Add-Type -Path "$PSScriptRoot\WtfDll.dll"

# Create an instance of the MainWindow class
$mainWindow = New-Object WtfDll.MainWindow

# Show the MainWindow
$mainWindow.ShowDialog()