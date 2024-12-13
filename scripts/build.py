from utils import *
import time, sys

CONFIG = {
    "SRC_PATH": "src",
    "ICON_PATH": "src/ico.o",
    "INCLUDES": ["src/", "src/shared/", "src/vendor/"],
    "DEFINES": ["VERSION=2024.3", "STD=2.8"],
    "EXTRAS": "-lws2_32",
    "STD": "c++20",
    "OUTPATH": "bin\\AO.exe",
    "OPTIMIZATION": "-O2",
    "PRECOMPILES": [
        ("src/aopch.h.gch", "g++ src/aopch.h"), # compile AO precompiled headers
        ("src/ico.o", "windres src/ico.rc -O coff -o src/ico.o") # create ico.o containing the data for AO icon
    ]
}
COMMON = lambda: f"{join(CONFIG["INCLUDES"], "-I")} {CONFIG["EXTRAS"]} {join(CONFIG["DEFINES"], "-D")} {CONFIG["OPTIMIZATION"]} -std={CONFIG["STD"]} -Wall"

def get_objs(file: str):
    o_name = f"obj\\{create_unique_name(file)}.o"

    current_hash = hash_file(file)
    previous_hash = load_hash(file)

    if not os.path.isfile(o_name) or current_hash != previous_hash:
        save_hash(file, hash_file(file))

        if file.endswith(".cpp"):
            os.system(f"g++ -c {file} {COMMON()} -o {o_name}")

        elif file.endswith(".c"):
            os.system(f"gcc -c {file} -o {o_name}")

    return o_name

def delete_stale_objects():
    valid_files = {i for i in get_files(CONFIG["SRC_PATH"], (".cpp", ".c"))}
    objects = {i["name"] for i in read_index_cache()}
    cache = read_index_cache()

    for file in list(objects - valid_files):
        index = next((i for i, x in enumerate(cache) if x["name"] == file), None)
        rm([cache[index]["o_name"]])
        cache.pop(index)
        remove_hash(cache)

def compile_ao():
    start = time.perf_counter()

    delete_stale_objects()
    obj_files = [get_objs(f) for f in get_files(CONFIG["SRC_PATH"], (".cpp", ".c"))]
    os.system(f"g++ {CONFIG["ICON_PATH"]} {join(obj_files)} {COMMON()} -o {CONFIG["OUTPATH"]}")

    calc_total_time(time.perf_counter() - start)

def run_ao(args):
    os.system(f"{CONFIG["OUTPATH"]} {join(args)}")
    pass

###############################################################################################################################
########################################################## MAIN CODE ##########################################################
###############################################################################################################################

mkdirs("bin", "obj")
create_index_cache() if not os.path.isfile("scripts\\index.json") else None

if not sys.argv[1:]:
    CONFIG["OPTIMIZATION"] = "-O2"
    precompile_files(CONFIG["PRECOMPILES"])
    compile_ao()

for i, x in enumerate(sys.argv[1:]):
    if x == "clean":
        rm(sys.argv[i+2:] if sys.argv[i+2:] else ["bin/", "obj/", "src/aopch.h.gch", "src/ico.o", "scripts/index.json"])

    elif x == "pch":
        precompile_files(CONFIG["PRECOMPILES"])

    elif x == "run":
        CONFIG["OPTIMIZATION"] = ""
        compile_ao()
        input("press enter to continue.")
        run_ao(sys.argv[i+2:])

    elif x == "exec":
        run_ao(sys.argv[i+2:])
        break
