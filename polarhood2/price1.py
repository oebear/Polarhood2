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
    url2 = "https://www.economist.com/search?q=" + a
    result = requests.get(url, params=params, headers=headers, timeout=100)
    result2 = requests.get(url2, params=params, headers=headers, timeout=900)
    doc = BeautifulSoup(result.text, "html.parser")
    doc2 = BeautifulSoup(result2.text, "html.parser")
    # get all the relevant data
    price = doc.find("fin-streamer", attrs={"class": "Fw(b) Fz(36px) Mb(-4px) D(ib)"}).text
    Name = doc.find("h1", attrs={"class": "D(ib) Fz(18px)"}).text
    news = doc2.find_all("span", text=True, attrs={"class": "_headline"})
    print(price)
    print(Name)
    text = open('data3.html', 'w', encoding="utf-8")
    text.write(result2.text)
    for b in range(0, 4):
       print(news[b].get_text()) 

#calling this program with ticker
if __name__ == "__main__":
    a = sys.argv[1]
    hello(a)