import shutil, sys, os

def rmdir(*folders):
    for i in folders:
        if os.path.exists(i):
            shutil.rmtree(i)

rmdir("AOs")
if len(sys.argv) == 2 and sys.argv[1] == "clean":
    rmdir("bin", "obj", "AOs")

if os.path.exists("AOs") == False:
    os.mkdir("AOs")

rmdir("bin", "obj")

if __name__=="__main__":
    os.system("dotnet publish --self-contained -c Release -o ./AOs")
