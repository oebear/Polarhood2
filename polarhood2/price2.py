from bs4 import BeautifulSoup
import requests
import sys

def hello(a):
    # getting html and turning it to text
    params = {
        "hl": "en" # language
        }

    headers = {
        "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHpythTML, like Gecko) Chrome/100.0.4896.60 Safari/537.36",
        'accept': 'application/json'
        }
    
    url = "https://finance.yahoo.com/quote/" + a
    result = requests.get(url, params=params, headers=headers, timeout=100)
    doc = BeautifulSoup(result.text, "html.parser")
    # get all the relevant data
    price = doc.find("fin-streamer", attrs={"class": "Fw(b) Fz(36px) Mb(-4px) D(ib)"}).text
    print(price)

#calling this program with ticker
if __name__ == "__main__":
    a = sys.argv[1]
    hello(a)