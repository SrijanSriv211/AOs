# IMPORTS
from colorama import init, Fore, Style
import argparse
import requests

# Initialize Command-line arguments.
Parser = argparse.ArgumentParser(description="Check for Latest AOs Updates")
Parser.add_argument("-v", help="Enter the current version")
Args = Parser.parse_args()

# Initialize Updater.
init(autoreset = True)

# Check for Updates
if not Args.v:
    print(f"{Fore.RED}{Style.BRIGHT}YOU MUST ENTER A VERSION,")
    print(f"TYPE {Fore.CYAN}{Style.BRIGHT}'-v [VERSION]'")

else:
    response = requests.get("https://api.github.com/repos/Light-Lens/AOs/releases/latest")
    if response.json()["name"] == Args.v: print("You're up to date.")
    else:
        print("Updates available.")

        print(f"Your version: {Fore.BLUE}{Style.BRIGHT}{Args.v}")
        print(f"Latest version: {Fore.BLUE}{Style.BRIGHT}{response.json()['name']}")

        print(f"Get the Latest version here: {Fore.CYAN}{Style.BRIGHT}https://github.com/Light-Lens/AOs/releases")
