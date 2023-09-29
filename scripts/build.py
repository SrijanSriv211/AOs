import shutil, sys, os

def rmdir(*folders):
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

for i in sys.argv:
    print(i)

# if len(sys.argv) > 1:
#     if sys.argv[1] == "clean":
#         rmdir("bin", "obj", "AOs")

#     elif sys.argv[1] == "run":
#         if sys.argv[2:]:
#             os.system(f"dotnet run -p:FileVersion=2.5.{update_build_no()} -- {' '.join(sys.argv[2:])}")

#         else:
#             os.system(f"dotnet run -p:FileVersion=2.5.{update_build_no()}")

#     elif sys.argv[1] == "execute":
#         if os.path.isfile("./AOs/AOs.exe"):
#             os.system(f".\\AOs\\AOs.exe {' '.join(sys.argv[2:])}")

#         else:
#             print("Please build AOs to execute.")
#             print("Type 'build' to build AOs then type 'build execute' to execute the build executable..")

# else:
#     rmdir("AOs")
#     if os.path.exists("AOs") == False:
#         os.mkdir("AOs")

#     rmdir("bin", "obj")
#     os.system(f"dotnet publish --self-contained -p:FileVersion=2.5.{update_build_no()} -c Release -o ./AOs")
#     rmdir("bin", "obj")
