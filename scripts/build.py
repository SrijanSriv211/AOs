import shutil, sys, os

def precompile_files():
    filedata = [
        ("src/aospch.h.gch", "g++ src/aospch.h"), # compile AOs precompiled headers
        ("src/ico.o", "windres src/ico.rc -O coff -o src/ico.o") # create ico.o containing the data for AOs icon
    ]

    for path, cmd in filedata:
        if os.path.isfile(path) == False:
            os.system(cmd)

# https://stackoverflow.com/a/2909998/18121288
def compile_AOs():
    cpp_dirs = []
    for path, subdirs, files in os.walk("."):
        for name in files:
            if (name.endswith(".cpp")):
                cpp_dirs.append(path + "\\*.cpp")
                break

    # compile AOs
    all_src_files = " ".join(cpp_dirs)
    script = f"g++ src/ico.o {all_src_files} -Isrc/ -o bin/AOs.exe"
    os.system(script)

# update the build num
def update_build_no():
    build_no = int(open("scripts\\build.txt", "r").read()) if os.path.isfile("scripts\\build.txt") else 0

    with open("scripts\\build.txt", "w") as f:
        f.write(str(build_no + 1)) # increment +1 to the current build no of AOs

# remove folders from the root dir
def rm(folders, files):
    [shutil.rmtree(i) for i in folders if os.path.exists(i)]
    [os.remove(i) for i in files if os.path.isfile(i)]

# create the bin folder
if os.path.isdir("bin") == False:
    os.mkdir("bin")

if not sys.argv[1:]:
    precompile_files()
    compile_AOs()
    update_build_no()

for i in sys.argv[1:]:
    if i == "help":
        print("If no argument is passed     -> Build AOs from source")
        print("clean                        -> Remove 'bin', 'obj' folders from the root directory.")
        print("run                          -> Run AOs")
        print("pch                          -> Precompile all headers")

    elif i == "clean":
        rm(folders=["bin"], files=["src/aospch.h.gch", "src/ico.o"])

    elif i == "run":
        precompile_files()
        compile_AOs()

    elif i == "pch":
        precompile_files()
