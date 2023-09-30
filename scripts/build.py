import PyInstaller.__main__, shutil, sys, os

def rmdirs(*folders):
    for i in folders:
        if os.path.exists(i):
            shutil.rmtree(i)

def get_build_no():
    if os.path.isfile("scripts\\build.txt"):
        return str(int(open("scripts\\build.txt", "r").read()) + 1)

    return "0"

def update_build_no():
    build_no = get_build_no()
    with open("scripts\\build.txt", "w") as f:
        f.write(build_no)

    return build_no

def build_UPR(output_dir):
    options = [
        "src\\AOs\\UPR.py",
        "--distpath", output_dir,
        "--icon", "img\\UPR.ico",
        "--clean",
        "--onefile"
    ]

    PyInstaller.__main__.run(options)

    rmdirs("build")
    os.remove("UPR.spec")

def run_AOs(argv, build_no):
    if os.path.isfile("bin\\Debug\\net7.0\\UPR.exe") == False:
        build_UPR("bin\\Debug\\net7.0")

    if argv[2:]:
        os.system(f"dotnet run -p:FileVersion=2.5.{build_no} -- {' '.join(argv[2:])}")

    else:
        os.system(f"dotnet run -p:FileVersion=2.5.{build_no}")

def build_AOs():
    rmdirs("AOs")
    if os.path.exists("AOs") == False:
        os.mkdir("AOs")

    os.system(f"dotnet publish --self-contained -p:FileVersion=2.5.{update_build_no()} -c Release -o ./AOs")
    build_UPR("AOs")

if len(sys.argv) > 1:
    # print help message
    if sys.argv[1] == "help" or sys.argv[1] == "-h" or sys.argv[1] == "--help" or sys.argv[1] == "-?" or sys.argv[1] == "??":
        print("If no argument is passed     -> Build AOs from source")
        print("execute                      -> Run AOs release executable")
        print("clean                        -> Remove 'bin', 'obj' folders from the root directory.")
        print("srun                         -> Run AOs but don't update the build no.")
        print("run                          -> Run AOs and update the build no.")

    # delete the following folder
    elif sys.argv[1] == "clean":
        rmdirs("bin", "obj", "build")

    # run and update the build no.
    elif sys.argv[1] == "run":
        run_AOs(sys.argv, update_build_no())

    # 'srun' -> silent run, run but don't update the build no.
    elif sys.argv[1] == "srun":
        run_AOs(sys.argv, get_build_no())

    elif sys.argv[1] == "execute":
        if os.path.isfile("./AOs/AOs.exe") == False:
            print("Building AOs")
            build_AOs()

        print("Executing AOs")
        os.system(f".\\AOs\\AOs.exe {' '.join(sys.argv[2:])}")

else:
    build_AOs()
