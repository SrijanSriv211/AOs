import argparse, urllib.parse, os

parser = argparse.ArgumentParser()
parser.add_argument("--engine", help="Select other search engines")
parser.add_argument("--search", help="Search Google", required=True)
args = parser.parse_args()

engines = {
    "bing": "https://www.bing.com/search?q=",
    "google": "https://www.google.com/search?q=",
    "duckduckgo": "https://duckduckgo.com/?q="
}

engine = engines["google"]
if args.search:
    if args.engine: engine = engines[args.engine]

    query = urllib.parse.quote(args.search)
    os.system(f"start {engine}{query}")
