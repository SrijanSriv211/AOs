import shutil, sys, os

# remove folders from the root dir
def rmdirs(folders):
    [shutil.rmtree(i) for i in folders if os.path.exists(i)]

# get the current build number of AOs
def get_build_no():
    return open("scripts\\build.txt", "r").read() if os.path.isfile("scripts\\build.txt") else "0"

# Update the build number of AOs
def update_build_no():
    build_no = str(int(get_build_no()) + 1) # increment +1 to the current build no of AOs

    with open("scripts\\build.txt", "w") as f:
        f.write(build_no)

    return build_no

# run AOs
def run_AOs(build_no):
    os.system(f"dotnet run -p:FileVersion=2.7.{build_no} -- {' '.join(sys.argv[2:])}")

# Build AOs in publish mode.
def build_AOs():
    if os.path.exists("AOs") == False:
        os.mkdir("AOs")

    os.system(f"dotnet publish -p:FileVersion=2.7.{update_build_no()} -c Release -o ./AOs")
    rmdirs(["bin", "obj"])

if len(sys.argv) > 1:
    # print help message
    if sys.argv[1] == "help" or sys.argv[1] == "-h" or sys.argv[1] == "--help" or sys.argv[1] == "-?" or sys.argv[1] == "??":
        print("If no argument is passed     -> Build AOs from source")
        print("clean                        -> Remove 'bin', 'obj' folders from the root directory.")
        print("srun                         -> Run AOs but don't update the build no.")
        print("run                          -> Run AOs and update the build no.")

    # delete the following folder
    elif sys.argv[1] == "clean":
        rmdirs(["bin", "obj"])

    # run and update the build no.
    elif sys.argv[1] == "run":
        run_AOs(update_build_no())

    # 'srun' -> silent run, run but don't update the build no.
    elif sys.argv[1] == "srun":
        run_AOs(get_build_no())

else:
    build_AOs()
