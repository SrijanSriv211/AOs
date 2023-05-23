import subprocess

def check_dotnet_version(version):
    command = ['dotnet', '--list-runtimes']
    
    try:
        output = subprocess.check_output(command, universal_newlines=True)
        if version in output:
            print(f".NET {version} is installed.")
        else:
            print(f".NET {version} is not installed.")
    except subprocess.CalledProcessError:
        print(".NET is not installed.")

# Specify the version you want to check
dotnet_version = "7.0"

check_dotnet_version(dotnet_version)
