from colorama import init, Fore, Style
from rich.progress import track
import argparse, requests, time

# initialize Command-line arguments.
Parser = argparse.ArgumentParser(description="Check for Latest AOs Updates")
Parser.add_argument("-v", help="Current running version of AOs")
args = Parser.parse_args()

# initialize.
init(autoreset = True)
repo = "Light-Lens/AOs"

# Check for Updates
total_size = 1
for _ in track(range(total_size), description="Checking for Updates...\n"):
    response = requests.get(f"https://api.github.com/repos/{repo}/releases/latest", timeout=1, stream=True, verify=True)
    total_size = int(response.headers.get("Content-Length", 0))
    time.sleep(0.001)

# Extract the latest release filename from the Github API response.
tag_name = response.json()["tag_name"]
assets = response.json()["assets"]
latest_version = ""
for asset in assets:
    filename = asset["name"]
    if filename.isdigit():
        latest_version = filename
        break

if args.v >= latest_version: print("You're up to date.")
else:
    print("Updates are available.")

    print(f"Your version: {Fore.BLUE}{Style.BRIGHT}{args.v}")
    print(f"Latest version: {Fore.BLUE}{Style.BRIGHT}{latest_version}")

    print(f"Get the Latest version here: {Fore.CYAN}{Style.BRIGHT}https://github.com/{repo}/releases/download/{tag_name}/AOs.zip")
