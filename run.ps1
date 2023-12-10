# Load the presentation framework assembly
Add-Type -AssemblyName PresentationFramework

# Load the assembly containing your MainWindow class
Add-Type -Path "C:\Path\To\Your\WpfApp.dll" # Replace with the actual path to your DLL

# Create an instance of the MainWindow class
$mainWindow = New-Object YourNamespace.MainWindow

# Show the MainWindow
$mainWindow.ShowDialog()