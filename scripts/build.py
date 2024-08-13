import shutil, sys, os

def get_build_no():
    return int(open("scripts\\build.txt", "r").read()) if os.path.isfile("scripts\\build.txt") else 0

def update_build_no():
    build_no = str(get_build_no() + 1) # increment +1 to the current build no of AOs
    open("scripts\\build.txt", "w").write(build_no)

def precompile_files():
    filedata = [
        ("src/aospch.h.gch", "g++ src/aospch.h"), # compile AOs precompiled headers
        ("src/ico.o", "windres src/ico.rc -O coff -o src/ico.o") # create ico.o containing the data for AOs icon
    ]

    [os.system(cmd) for path, cmd in filedata if os.path.isfile(path) == False]

# https://stackoverflow.com/a/2909998/18121288
SRC_FILES = " ".join([os.path.join(path, "*.cpp") for path, _, files in os.walk("src") if any(name.endswith(".cpp") for name in files)])
INCLUDE_DIRS = "-Isrc/ -Isrc/shared/"
SCRIPT = f"g++ src/ico.o {SRC_FILES} {INCLUDE_DIRS} -DVERSION=2.8 -DBUILD_NUMBER={get_build_no()} -std=c++20 -o bin/AOs.exe"

# create the bin folder
if os.path.isdir("bin") == False:
    os.mkdir("bin")

if not sys.argv[1:]:
    update_build_no()
    precompile_files()
    os.system(SCRIPT)

for i, x in enumerate(sys.argv[1:]):
    if x == "help":
        print("If no argument is passed     -> Build AOs from source")
        print("clean                        -> Remove 'bin', 'obj' folders from the root directory.")
        print("run                          -> Run AOs")
        print("pch                          -> Precompile all headers")
        print("exec                         -> Execute AOs without compiling")

    elif x == "clean":
        [shutil.rmtree(i) for i in ["bin"] if os.path.exists(i)]
        [os.remove(i) for i in ["src/aospch.h.gch", "src/ico.o"] if os.path.isfile(i)]

    elif x == "run":
        os.system(SCRIPT)
        os.system(f"bin\\AOs.exe {" ".join(sys.argv[i+2:])}")

    elif x == "exec":
        os.system(f"bin\\AOs.exe {" ".join(sys.argv[i+2:])}")

    elif x == "pch":
        precompile_files()
