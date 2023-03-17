import wikipedia, argparse, os

parser = argparse.ArgumentParser()
parser.add_argument("--show", help="Play on youtube")
parser.add_argument("--search", help="Search on youtube")
args = parser.parse_args()

if args.show:
    try:
        wiki = wikipedia.summary(args.show.lower(), sentences=3)
        print(wiki)

    except Exception:
        print(f"Cannot find anything related to `{args.show}` on wikipedia")

elif args.search:
    query = args.search.replace(" ", "%20")
    os.system(f"start https://en.wikipedia.org/wiki/{query}")

else:
    print("Please provide at least one argument.")
