from colorama import Style, Fore, init
import requests, sys

init(autoreset = True)

# Extract the latest release filename from the Github API response.
response = requests.get("https://api.github.com/repos/Light-Lens/AOs/releases/latest")

# Get all the required json data
tag_name = response.json()["tag_name"]
assets = response.json()["assets"]
download_link = assets[1]["browser_download_url"]
current_version = sys.argv[1]

# assign the first asset's name that is a digit to the latest_version variable. If no such asset is found, latest_version will be set to None.
latest_version = next((asset["name"] for asset in assets if asset["name"].isdigit()), None)

if int(current_version) >= int(latest_version):
    print("You're up to date.")

else:
    print("Updates are available.")

    print(f"Your version: {Fore.WHITE}{Style.BRIGHT}{current_version}")
    print(f"Latest version: {Fore.WHITE}{Style.BRIGHT}{latest_version}")

    # print(f"Get the Latest version here: {Fore.CYAN}{Style.BRIGHT}https://github.com/Light-Lens/AOs/releases/download/{tag_name}/AOs.zip")
    print(f"Get the Latest version here: {Fore.CYAN}{Style.BRIGHT}{download_link}")
