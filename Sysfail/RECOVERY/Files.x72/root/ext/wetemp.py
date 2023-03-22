import argparse, requests

parser = argparse.ArgumentParser()
parser.add_argument("-c", help="City name")
parser.add_argument("-r", help="Which report to show.", default="w", required=True) # w = weather, t = temperature
args = parser.parse_args()

# Get the weather report.
def WeatherReport(City="Bhagalpur"):
    # https://medium.com/analytics-vidhya/forecast-weather-using-python-e6f5519dc3c1
    print(f"Fetching today's weather report for: {City}")

    # Fetch weather details.
    URL = f"https://wttr.in/{City}?format=%C"
    res = requests.get(URL)

    print(res.text)

# Get today's temperature.
def WeatherTemp(City="Bhagalpur"):
    # https://medium.com/analytics-vidhya/forecast-weather-using-python-e6f5519dc3c1
    print(F"Fetching today's temperature report in: {City}")

    # Fetch weather details.
    URL = f"https://wttr.in/{City}?format=%t"
    res = requests.get(URL)
    Temp = res.text[1:] if res.text[0] == "+" else res.text

    print(Temp)

if (args.r == "w"):
    WeatherReport(args.c)

elif (args.r == "t"):
    WeatherTemp(args.c)
