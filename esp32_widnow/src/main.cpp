#include <Arduino.h>
#include <Adafruit_Sensor.h>
#include <DHT.h>
#include <ESP32Servo.h>
#include <Wire.h>
#include <WiFi.h>
#include <string.h>
#include <HTTPClient.h>
#include <iostream>
#include <map>
#include <unordered_map>
#include <iostream>
#include <future>
#include <string>
#include <ArduinoJson.h>




WiFiServer server(80);
#define magnet_switch 19  
#define alarm_sound 26
#define servo_pin  16
#define water 35
#define button 17
#define DHTPIN 33  
#define DHTTYPE DHT11  
DHT dht(DHTPIN, DHTTYPE);
std::string str;
Servo servo1;
int rain;
int h;
int t; 
boolean button1;
boolean but_flag;
String header;
String url = "http://192.168.4.2:81/WindowStatus";
String urlInput = "http://192.168.4.2:81/WindowStatus/getUserInput";
boolean is_protected = false;
bool globalOpenCheck;
bool is_rain = false; 
bool is_alarm = false;
char jsonOutput[128];
const char* ssid     = "ESP32-Network";
const char* password = "11111111";
unsigned long currentTime = millis();
unsigned long previousTime = 0;
const long timeoutTime = 500;

  
boolean is_window_open()
{
  int alarm_read = digitalRead(magnet_switch);
   Serial.print("IS WINDOW OPEN ");
   Serial.print(alarm_read);
   Serial.println(" ");
  return alarm_read;
}

boolean win_close()
{
  Serial.print("FUNC CLOSE");
  Serial.println(" ");
  servo1.attach(servo_pin);
  servo1.write(120);
  delay(1200);
  servo1.detach();
  if (is_window_open() == 1)
    {
      return false;
    }
  else
  {
      return true;
  }
}
void setup()
{
  Serial.begin(115200);
  pinMode(alarm_sound, OUTPUT);
  servo1.attach(servo_pin);
  pinMode(water, INPUT);
  pinMode(DHTPIN,INPUT);
  pinMode(button, INPUT_PULLUP);
  pinMode(magnet_switch, INPUT_PULLUP);
  dht.begin();
  WiFi.softAP(ssid,password);
  Serial.println("Connecting");
  Serial.println("IP address: ");
  Serial.println(WiFi.softAPIP());
  server.begin();
  win_close();
}



void alarm()
{ 
  is_alarm = true;
  return;
  int buzzer = alarm_sound;
  Serial.print("FUNC ALARM");
  Serial.println(" ");
   while (is_window_open() == 1)
   { // Run for 5 seconds
    tone(buzzer, 4000); // Set the buzzer to a frequency (adjust as needed)
    delay(100); // Play the sound for 100 milliseconds (adjust as needed)
    noTone(buzzer); // Stop the sound
    delay(100); // Pause for 100 milliseconds (adjust as needed)
   }
}



void win_open()
{
    Serial.print("FUNC OPEN");
    Serial.println(" ");
    servo1.attach(servo_pin);
    servo1.write(75);
    delay(500); 
    servo1.detach();
}
  
  void loop()
  {
    is_window_open();
    button1 = !digitalRead(button);
    rain = analogRead(water);
    h = dht.readHumidity();
    t = dht.readTemperature();
    
  
    Serial.print(F("Humidity: "));
    Serial.print(h);
    Serial.print(F("%  Temperature: "));
    Serial.print(t);
    Serial.print(F("°C "));
    Serial.println(" ");
    Serial.print("BUTTON ");
    Serial.print(button1);
    Serial.println(" ");
    Serial.print("BUT FLAG ");
    Serial.print(but_flag);
    Serial.println(" ");
    Serial.print("WATER LEVEL ");
    Serial.print(rain);
    Serial.println(" ");
    button1 = 0;
    if (button1 == 1  )
    {
      but_flag = !but_flag;
      delay(200);
    }
   
    if (but_flag == 0 && globalOpenCheck != but_flag)
    {
      boolean is_closed =  win_close();
    }
    else if (but_flag == 1 && globalOpenCheck != but_flag)
    { 
      win_open();
    }
  
    if (rain > 250 )
    {
      is_rain = true;
      Serial.print("RAIND DETECTED ");
      
    }
    else
    {
      is_rain = false;
      }
    if (is_window_open() == 1 && but_flag ==0 && is_protected == true )
    {
      alarm();
    }
    else if (is_window_open() == 0 && is_protected == true)
    {
      is_alarm = false;
    }
    globalOpenCheck = but_flag;
    Serial.println(but_flag);
    unsigned long currentMillis = millis();
    WiFiClient client = server.available();   // Listen for incoming clients
    if (WiFi.softAPgetStationNum() > 0 && currentMillis - previousTime > timeoutTime) 
    {
      previousTime = currentMillis;
      Serial.println("TEST");
      Serial.println("User connected to ESP32's local WiFi");
        HTTPClient client;
          client.begin(url.c_str());
          client.addHeader("Content-Type", "application/json");
          String temperatureStr = "\"" + String(t) + "\"";
          String humidityStr = "\"" + String(h) + "\"";
          String isOpenStr = is_window_open() ? "1" : "0";
          isOpenStr ="\"" + isOpenStr + "\"";
          String is_protectedStr = is_protected ? "1" : "0";
          is_protectedStr ="\"" + is_protectedStr + "\"";
          String jsonString = "{\"temparature\": " + temperatureStr + ", \"humidity\": " + humidityStr + ", \"isOpen\": " + isOpenStr + ", \"isRain\": " + is_rain + ", \"isProtected\": " + is_protectedStr + ",  \"isAlarm\": " + is_alarm + "}";
          Serial.println("JSON STRING: ");
          Serial.println(jsonString);
          int httpResponseCode = client.POST(jsonString);
          Serial.println("HTTP POST  CODE: ");
          Serial.println(httpResponseCode);
          if (httpResponseCode > 0) {
            String response = client.getString();
            Serial.println("Response:");
            Serial.println(response);
          } 
          else 
          {
            
            Serial.println("Error: No response received");
            
          }
        client.end();
        client.begin(urlInput.c_str()); 
       
        httpResponseCode = client.GET();
       if (httpResponseCode > 0) 
        { 
            bool isOpen = true;
            std::string jsonStr = client.getString().c_str();
            
            DynamicJsonDocument doc(1024); // створіть об'єкт JSON-документа
           DeserializationError error = deserializeJson(doc, jsonStr);
            is_protected = doc["isProtected"];
            but_flag = doc["isOpen"]; 
            Serial.println(but_flag);
            
        } 
        else 
        {
            Serial.println("Error: No response received");
        }
     //     
      
    }
  
  }


  





  