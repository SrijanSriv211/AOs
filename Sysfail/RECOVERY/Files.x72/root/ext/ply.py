import pywhatkit, argparse, urllib.parse, os

parser = argparse.ArgumentParser()
parser.add_argument("--play", help="Play on youtube")
parser.add_argument("--search", help="Search on youtube")
args = parser.parse_args()

if args.play:
    pywhatkit.playonyt(args.play)

elif args.search:
    query = urllib.parse.quote(args.search)
    os.system(f"start https://www.youtube.com/results?search_query={query}")

else:
    print("Please provide at least one argument.")
