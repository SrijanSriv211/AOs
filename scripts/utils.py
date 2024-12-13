import hashlib, shutil, json, os

def calc_total_time(seconds):
    min, sec = divmod(seconds, 60)
    hour, min = divmod(min, 60)
    hours, minutes, seconds = int(hour), int(min), int(sec)

    t = [
        f"{hours} hour" + ("s" if hours > 1 else "") if hours > 0 else None,
        f"{minutes} minute" + ("s" if minutes > 1 else "") if minutes > 0 else None,
        f"{seconds} second" + ("s" if seconds > 1 else "") if seconds > 0 else None
    ]
    t = list(filter(None, t))

    print("total time:", ", ".join(t) if t else "0 seconds")

def mkdirs(*dirs):
    [os.mkdir(dir) for dir in dirs if not os.path.isdir(dir)]

def rm(files):
    [shutil.rmtree(i) if os.path.isdir(i) else os.remove(i) for i in files if os.path.exists(i)]

def join(lst, prefix="", separator=" "):
    return separator.join([prefix + i for i in lst])

# https://stackoverflow.com/a/18351977/18121288
# Though not widely known, str.endswith also accepts a tuple. You don't need to loop.
def get_files(path: str, ext: tuple):
    return [os.path.join(path, file) for path, _, files in os.walk(path) for file in files if file.endswith(ext)]

def create_unique_name(filepath: str):
    return filepath.replace("/", "_").replace("\\", "_").replace(":", "_")

def precompile_files(precompiles):
    [os.system(cmd) for path, cmd in precompiles if not os.path.isfile(path)]

def hash_file(file):
    md5_h = hashlib.md5()

    with open(file, "rb") as f:
        for chunk in iter(lambda: f.read(4096), b""):
            md5_h.update(chunk)

    return md5_h.hexdigest()

def read_index_cache() -> list:
    with open("scripts\\index.json", "r") as f:
        return json.load(f)

def save_hash(name, hash):
    cache = read_index_cache()
    index = next((i for i, x in enumerate(cache) if x["name"] == name), None)

    if index == None:
        cache.append({
            "name": name,
            "o_name": f"obj\\{create_unique_name(name)}.o",
            "hash": hash
        })

    else:
        cache[index]["hash"] = hash

    with open("scripts\\index.json", "w") as f:
        json.dump(cache, f, indent=4)

def load_hash(name):
    cache = read_index_cache()
    index = next((i for i, x in enumerate(cache) if x["name"] == name), None)
    return cache[index]["hash"] if index != None else None

def remove_hash(cache):
    with open("scripts\\index.json", "w") as f:
        json.dump(cache, f, indent=4)

def create_index_cache():
    cpp_files = get_files("src", (".cpp", ".c"))
    cache = [
        {
            "name": i,
            "o_name": f"obj\\{create_unique_name(i)}.o",
            "hash": ""
        }
        for i in cpp_files
    ]

    with open("scripts\\index.json", "w") as f:
        json.dump(cache, f, indent=4)
