import shutil, glob, sys, os

def rmdir(*folders):
    for i in folders:
        if os.path.exists(i):
            shutil.rmtree(i)

def update_build_no(dump_file_path=""):
    # Get a list of all files in the current directory with names consisting only of digits
    files = glob.glob("[0-9]*")

    # Filter the list to only include files (not directories) and pick the first one
    file_with_numbers_name = next(filter(lambda f: not os.path.isdir(f), files), None)

    # If a file with a name consisting only of digits exists in the current directory
    if file_with_numbers_name:
        print("Updating build number")

        new_name = str(int(file_with_numbers_name) + 1)
        os.rename(file_with_numbers_name, new_name)

        if dump_file_path != "":
            os.system(f"copy {new_name} {dump_file_path}\\{new_name} /Y")

    else:
        print("No build file found")
        os.system("echo.>0")

if len(sys.argv) > 1:
    if sys.argv[1] == "clean":
        rmdir("bin", "obj", "AOs")

    elif sys.argv[1] == "run":
        update_build_no()
        os.system(f"dotnet run -- {' '.join(sys.argv[2:])}")

else:
    rmdir("AOs")
    if os.path.exists("AOs") == False:
        os.mkdir("AOs")

    rmdir("bin", "obj")
    update_build_no("AOs")
    os.system("dotnet publish --self-contained -c Release -o ./AOs")
